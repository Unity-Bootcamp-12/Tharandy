using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<GameManager>();
                if (_instance == null)
                {
                    GameObject manager = new(typeof(GameManager).Name);
                    _instance = manager.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    private static GameManager _instance;

    // 나중에 Generater로 분리할 것들
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemySpawnPointPrefab;
    [SerializeField] private GameObject _inGame;
    [SerializeField] private Transform _spawnPosition;

    [SerializeField] private TextChangeUI _timerUI;
    [SerializeField] private TextChangeUI _countDownUI;
    [SerializeField] private TextChangeUI _scoreUI;
    [SerializeField] private LifeUI _lifeUI;
    [SerializeField] private RankingUI _rankingUI;

    [SerializeField] private Vector2 _enemySpawnRangeX = new(-4.5f, 4.5f);
    [SerializeField] private Vector2 _enemySpawnRangeY = new(-2.0f, 2.0f);

    [SerializeField] private int _enemySpawnPointCount = 12;
    [SerializeField] private float _enemySpawnInterval = 1.4f;
    [SerializeField] private int _playTime = 60;
    [SerializeField] private int _maxLife = 3;

    private List<Enemy> _enemyPool = new();
    private List<Transform> _enemySpawnPointList = new();
    private Enemy[] _activeEnemyList;

    private Coroutine _spawnEnemyCoroutine;
    private Coroutine _gamePlayCoroutine;

    private int _currentLife;
    private int _score;

    [Header("Ending")]
    [SerializeField] private EndingManager _endingManager;

    private bool _isLose = false;
    private void Start()
    {
        _inGame.SetActive(true);
        _activeEnemyList = new Enemy[_enemySpawnPointCount];
        _gamePlayCoroutine = StartCoroutine(GamePlayCoroutine());

        _timerUI.gameObject.SetActive(false);
        _countDownUI.gameObject.SetActive(false);
        _scoreUI.gameObject.SetActive(false);
        _lifeUI.gameObject.SetActive(false);
        _lifeUI.SetUI(_maxLife, _maxLife);
        _rankingUI.gameObject.SetActive(false);

        _score = 0;
        _currentLife = _maxLife;
    }

    private void GenerateSpawnPoint()
    {
        int limit = 0;

        while (_enemySpawnPointList.Count < _enemySpawnPointCount)
        {
            limit++;
            if (limit > 100)
            {
                Debug.LogError("Unable to generate enough spawn points.");
                break;
            }

            float x = Random.Range(_enemySpawnRangeX.x, _enemySpawnRangeX.y);
            float y = Random.Range(_enemySpawnRangeY.x, _enemySpawnRangeY.y);

            bool canSpawn = true;

            foreach (Transform spawnPoint in _enemySpawnPointList)
            {
                if (Mathf.Abs(spawnPoint.position.x - x) < 1.25f &&
                    Mathf.Abs(spawnPoint.position.y - y) < 1.25f)
                {
                    canSpawn = false;
                }
            }

            if (canSpawn)
            {
                Transform spawnPoint = Instantiate(_enemySpawnPointPrefab).transform;
                spawnPoint.position = new Vector3(x, y, _spawnPosition.position.z);
                _enemySpawnPointList.Add(spawnPoint);
            }
        }
    }

    private bool CanSpawnEnemy(int spawnPointIndex)
    {
        return _activeEnemyList[spawnPointIndex] == null;
    }

    public bool GenerateEnemy(int spawnPointIndex)
    {
        Enemy enemy = GetEnemyFromPool();
        enemy.transform.position = _enemySpawnPointList[spawnPointIndex].position;
        Debug.Log($"enemy : {enemy.transform.position}");
        _activeEnemyList[spawnPointIndex] = enemy;
        enemy.SpawnPointIndex = spawnPointIndex;
        enemy.gameObject.SetActive(true);

        return true;
    }

    private Enemy GetEnemyFromPool()
    {
        Enemy returnEnemy;

        if (_enemyPool.Count <= 0)
        {
            _enemyPool.Add(Instantiate(_enemyPrefab).GetComponent<Enemy>());
        }

        returnEnemy = _enemyPool[0];
        _enemyPool.Remove(returnEnemy);

        returnEnemy.gameObject.SetActive(false);

        return returnEnemy;
    }

    public void ReturnEnemyToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        _activeEnemyList[enemy.SpawnPointIndex] = null;

        _enemyPool.Add(enemy);
    }

    public void AddScore(int score)
    {
        _score += score;
        _scoreUI.SetText(_score.ToString());
    }

    public void ReduceLife()
    {
        _currentLife--;
        if (_currentLife <= 0)
        {
            GameLose();
        }

        _lifeUI.SetUI(_maxLife, _currentLife);
    }

    private void DeactivateEnemies()
    {
        if (_activeEnemyList == null)
        {
            return;
        }

        foreach (var enemy in _activeEnemyList)
        {
            if (enemy == null)
            {
                continue;
            }
            enemy.gameObject.SetActive(false);
        }
    }

    public void GameLose()
    {
        if (_isLose)
        {
            return;
        }

        _isLose = true;
        DeactivateEnemies();
        _endingManager.GameLose();
        StopCoroutine(_spawnEnemyCoroutine);
        RankingManager.Instance.AddScore(_score);
        ShowRankingUI();
    }

    private void ShowRankingUI()
    { 
        _rankingUI.gameObject.SetActive(true);
        _rankingUI.SetRankingScore(RankingManager.Instance.GetScores());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemy.Hit();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Time.timeScale = 1.0f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Time.timeScale = 2.0f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Time.timeScale = 3.0f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Time.timeScale = 0.5f;
        }

    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_enemySpawnInterval);

            int spawnCount = Random.Range(1, 3);

            for (int n = 0; n < spawnCount; n++)
            {
                //중복 방지로 공간 수 만큼 시도 후 실패하면 종료
                for (int i = 0; i < _enemySpawnPointCount; i++)
                {
                    int spawnPointIndex = Random.Range(0, _enemySpawnPointCount);
                    if (CanSpawnEnemy(spawnPointIndex))
                    {
                        GenerateEnemy(spawnPointIndex);
                        break;
                    }
                }
            }
        }
    }

    private IEnumerator GamePlayCoroutine()
    {
        yield return new WaitForSeconds(3f);

        _countDownUI.gameObject.SetActive(true);

        for (int i = 3; i >= 0; i--)
        {
            _countDownUI.SetText(i.ToString());
            yield return new WaitForSeconds(1f);
        }

        GenerateSpawnPoint();

        _countDownUI.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);

        _timerUI.gameObject.SetActive(true);
        _scoreUI.gameObject.SetActive(true);
        _lifeUI.gameObject.SetActive(true);
        _spawnEnemyCoroutine = StartCoroutine(SpawnEnemyCoroutine());

        for (int i = _playTime; i >= 0; i--)
        {
            _timerUI.SetText(i.ToString());
            yield return new WaitForSeconds(1f);
        }

        DeactivateEnemies();
        StopCoroutine(_spawnEnemyCoroutine);

        if (!_isLose)
        {
            _endingManager.GameWin();
        }
    }

    public void RestartGame()
    {
        Debug.Log("RestartGame Clicked");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

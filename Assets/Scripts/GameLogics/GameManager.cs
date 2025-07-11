using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] private TextChangeUI _timerUI;
    [SerializeField] private TextChangeUI _countDownUI;
    [SerializeField] private TextChangeUI _scoreUI;

    [SerializeField] private Vector2 _enemySpawnRangeX = new(-4.5f, 4.5f);
    [SerializeField] private Vector2 _enemySpawnRangeY = new(-2.0f, 2.0f);

    [SerializeField] private int _enemySpawnPointCount = 12;
    [SerializeField] private float _enemySpawnInterval = 1.4f;
    [SerializeField] private int _playTime = 60;

    private List<Enemy> _enemyPool = new();
    private List<Transform> _enemySpawnPointList = new();
    private Enemy[] _activeEnemyList;

    private Coroutine _spawnEnemyCoroutine;
    private Coroutine _gamePlayCoroutine;

    private int _score;

    private void Start()
    {
        _inGame.SetActive(true);
        _activeEnemyList = new Enemy[_enemySpawnPointCount];
        _gamePlayCoroutine = StartCoroutine(GamePlayCoroutine());

        _timerUI.gameObject.SetActive(false);
        _countDownUI.gameObject.SetActive(false);
        _scoreUI.gameObject.SetActive(false);
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
                spawnPoint.position = new Vector3(x, y, 0f);
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
        enemy.gameObject.SetActive(true);
        enemy.transform.position = _enemySpawnPointList[spawnPointIndex].position;
        _activeEnemyList[spawnPointIndex] = enemy;
        enemy.SpawnPointIndex = spawnPointIndex;

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
    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_enemySpawnInterval);

            int spawnCount = Random.Range(1, 4);

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
        _spawnEnemyCoroutine = StartCoroutine(SpawnEnemyCoroutine());

        for (int i = _playTime; i >= 0; i--)
        {
            _timerUI.SetText(i.ToString());
            yield return new WaitForSeconds(1f);
        }

        StopCoroutine(_spawnEnemyCoroutine);
    }
}

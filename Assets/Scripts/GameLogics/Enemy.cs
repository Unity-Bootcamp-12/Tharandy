using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    readonly int APPEARANCE_ANIMATION = Animator.StringToHash("IsAppearance");
    readonly int PUNCHING_ANIMATION = Animator.StringToHash("IsPunching");

    [SerializeField] private int _enemyScore = 100;
    [SerializeField] private Transform _enemyModel;
    [SerializeField] private float _appearanceDuration = 2.0f;
    [SerializeField] private float _punchingDuration = 1.5f;

    private Animator _modelAnimator;

    private Vector3 _startPosition;

    public int SpawnPointIndex { get; set; }

    private Coroutine _currentCoroutine;

    private void Awake()
    {
        _modelAnimator = _enemyModel.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }

        _startPosition = transform.position;
        _enemyModel.position = _startPosition;
        _enemyModel.localEulerAngles = new Vector3(0, 180.0f, 0);
        _currentCoroutine = StartCoroutine(EnemyAppearCoroutine());
    }

    private void OnDisable()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }

        _enemyModel.localEulerAngles = new Vector3(0, 180.0f, 0);
    }

    public void Hit()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
        GameManager.Instance.ReturnEnemyToPool(this);
        GameManager.Instance.AddScore(_enemyScore);
    }

    private IEnumerator EnemyAppearCoroutine()
    {
        if (_modelAnimator != null)
        {
            _modelAnimator.SetBool(APPEARANCE_ANIMATION, true);
        }

        Vector3 startPosition = _startPosition;
        Vector3 targetPosition = startPosition + new Vector3(0, 0, -1.0f);

        float elapsed = 0f;
        while (elapsed < _appearanceDuration)
        {
            _enemyModel.position = Vector3.Lerp(startPosition, targetPosition, elapsed / _appearanceDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _enemyModel.position = targetPosition;

        _modelAnimator.SetTrigger(PUNCHING_ANIMATION);

        yield return new WaitForSeconds(_punchingDuration);

        GameManager.Instance.ReduceLife();
        GameManager.Instance.ReturnEnemyToPool(this);
    }
}

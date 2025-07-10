using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _enemyScore = 100;
    public int SpawnPointIndex { get; set; }

    private Coroutine _currentCoroutine;

    private void OnEnable()
    {
        _currentCoroutine = StartCoroutine(EnemyCoroutine());
    }

    private void OnDisable()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
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

    private IEnumerator EnemyCoroutine()
    {
        yield return new WaitForSeconds(3.5f);

        GameManager.Instance.ReturnEnemyToPool(this);
    }
}

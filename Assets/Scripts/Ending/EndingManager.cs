using System;
using System.Collections;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _winPrefabs;
    [SerializeField] private GameObject[] _defeatPrefabs;
    [SerializeField] private Canvas _inGameCanvas;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _destination;

    public void GameWin()
    {
        _inGameCanvas.enabled = false;
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayBGM("Win");
        foreach (GameObject winPrefab in _winPrefabs)
        {
            Instantiate(winPrefab);
        }
        StartCoroutine(C_WaitForEnding());
    }

    private IEnumerator C_WaitForEnding()
    {
        yield return new WaitForSeconds(2.0f);
        UIManager.Instance.ShowEndingPanel(true);
    }

    public void GameLose()
    {
        _inGameCanvas.enabled = false;
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayBGM("Lose");
        foreach (GameObject defeatPrefab in _defeatPrefabs)
        {
            EndingThanos endingThanos = Instantiate(defeatPrefab, _spawnPoint.localPosition, _spawnPoint.rotation).GetComponent<EndingThanos>();
            endingThanos.destination = _destination;
        }
    }
}

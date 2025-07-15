using System;
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
        foreach (GameObject winPrefab in _winPrefabs)
        {
            Instantiate(winPrefab);
        }
    }
    public void GameLose()
    {
        _inGameCanvas.enabled = false;
        foreach (GameObject defeatPrefab in _defeatPrefabs)
        {
            EndingThanos endingThanos = Instantiate(defeatPrefab, _spawnPoint.localPosition, _spawnPoint.rotation).GetComponent<EndingThanos>();
            endingThanos.destination = _destination;
        }
    }
}

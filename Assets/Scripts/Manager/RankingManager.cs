using UnityEngine;

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<RankingManager>();
                if (_instance == null)
                {
                    GameObject manager = new(typeof(RankingManager).Name);
                    _instance = manager.AddComponent<RankingManager>();
                }
            }
            return _instance;
        }
    }

    private static RankingManager _instance;

    private int[] _scoreList;
    private const int MAX_SCORES_LENGTH = 10;

    public void Start()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        _scoreList = new int[MAX_SCORES_LENGTH];
    }

    public void AddScore(int score)
    {
        for (int i = 0; i < MAX_SCORES_LENGTH; i++)
        {
            if (score > _scoreList[i])
            {
                // Shift scores down
                for (int j = MAX_SCORES_LENGTH - 1; j > i; j--)
                {
                    _scoreList[j] = _scoreList[j - 1];
                }
                _scoreList[i] = score;
                break;
            }
        }
    }

    public int[] GetScores()
    {
        return _scoreList;
    }
}

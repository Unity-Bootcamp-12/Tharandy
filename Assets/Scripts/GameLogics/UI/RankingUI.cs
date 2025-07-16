using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RankingUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _rankingScoreText;
    [SerializeField] private Button _retryButton;

    private void Start()
    {
        _retryButton.onClick.AddListener(OnRetryButtonClick);
    }

    public void SetRankingScore(int[] score)
    {
        StringBuilder sb = new();

        for (int i = 0; i < score.Length; i++)
        {
            if (score[i] > 0)
            {
                sb.AppendLine($"{score[i]}");
            }
            else
            { 
                sb.AppendLine("-");
            }
        }

        _rankingScoreText.text = sb.ToString();
    }

    public void OnRetryButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //gameObject.SetActive(false);
    }
}
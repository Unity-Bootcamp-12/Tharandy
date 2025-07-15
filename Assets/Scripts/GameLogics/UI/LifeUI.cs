using UnityEngine;

public class LifeUI : MonoBehaviour
{
    [SerializeField] private GameObject[] _lifeObjectList;

    public void SetUI(int maxLife, int currentLife)
    {
        for (int i = 0; i < maxLife; i++)
        {
            _lifeObjectList[i].SetActive(i < currentLife);
        }
    }
}

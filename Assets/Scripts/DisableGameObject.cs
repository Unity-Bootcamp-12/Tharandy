using UnityEngine;

public class DisableGameObject : MonoBehaviour
{
    public void OnPunchingAnimationEnd()
    {
        gameObject.SetActive(false);
    }
}

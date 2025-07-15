using UnityEngine;

public class DisableGameObject : MonoBehaviour
{
    public void OnPunchingAnimationEnd()
    {
        SoundManager.Instance.PlaySfx("ThanosPunch");
        gameObject.SetActive(false);
    }
}

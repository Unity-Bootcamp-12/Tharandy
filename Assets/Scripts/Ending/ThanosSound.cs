using UnityEngine;

public class ThanosSound : MonoBehaviour
{
    public void ThanosWalk()
    {
        SoundManager.Instance.PlaySfx("ThanosWalk");
    }

    public void ThanosFinger()
    {
        SoundManager.Instance.PlaySfx("ThanosFinger");
    }
    public void ThanosPunch()
    {
        SoundManager.Instance.PlaySfx("ThanosPunch");
    }
}

using UnityEngine;
using UnityEngine.Formats.Alembic.Importer;

public class AlembicPlayerController : MonoBehaviour
{
    public AlembicStreamPlayer streamPlayer;
    public float speed = 1f;
    private float currentTime = 0f;

    void Update()
    {
        if (streamPlayer != null)
        {
            currentTime += Time.deltaTime * speed;

            // 루프 처리
            if (currentTime > streamPlayer.Duration)
            { 
                currentTime = 0f; 
            }

            streamPlayer.UpdateImmediately(currentTime);
        }
    }
}

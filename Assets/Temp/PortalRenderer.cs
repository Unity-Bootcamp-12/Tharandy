using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PortalRenderer : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] _sprites; 

    private int index;

    private void Awake()
    {
        index = 0;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        index++;

        if (index >= _sprites.Length)
        {
            index = 0;
        }
        
        _spriteRenderer.sprite = _sprites[index];
    }
    private void Update()
    {
        index++;
 
        if (index >= _sprites.Length)
        {
            index = 53;
        }

        _spriteRenderer.sprite = _sprites[index];
    }
}

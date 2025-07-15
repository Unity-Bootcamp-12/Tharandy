using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PortalRenderer : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;

    private SpriteRenderer _spriteRenderer;
    private int _index;
    private bool _isStart = true;

    private void Awake()
    {
        _index = 0;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _index++;
        if (_index >= _sprites.Length)
        {
            _index = 0;
        }

        _spriteRenderer.sprite = _sprites[_index];
        _isStart = false;
    }

    private void OnEnable()
    {
        _index = 0;
        _isStart = true;

        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        if (_sprites.Length > 0)
        {
            _spriteRenderer.sprite = _sprites[_index];
        }
        _isStart = false;
    }

    private void Update()
    {
        if (_isStart)
        {
            return;
        }

        _index++;

        if (_index >= _sprites.Length)
        {
            _index = 53;
        }

        _spriteRenderer.sprite = _sprites[_index];
    }
}

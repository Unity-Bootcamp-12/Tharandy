using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PortalRenderer : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private GameObject _thanos;
    [SerializeField] private float _appearanceDuration = 1.0f;

    private SpriteRenderer _spriteRenderer;
    private int index;
    private bool _isStart = true;
    private Transform StartPosition;

    private Animator _animator;

    readonly int _thanos_Appearance = Animator.StringToHash("IsAppearance");


    private void Awake()
    {
        index = 0;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = _thanos.GetComponent<Animator>();
        StartPosition = _thanos.transform;
    }

    private void Start()
    {
        index++;
        if (index >= _sprites.Length)
        {
            index = 0;
        }

        _spriteRenderer.sprite = _sprites[index];
        _isStart = false;
    }

    private void OnEnable()
    {
        index = 0;
        //_thanos.SetActive(true);
        _thanos.transform.position = StartPosition.position;
        _thanos.transform.rotation = StartPosition.rotation;

        StartCoroutine(AppearanceThanos());
        _isStart = true;
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        if (_sprites.Length > 0)
        {
            _spriteRenderer.sprite = _sprites[index];
        }
        _isStart = false;
    }

    private void Update()
    {
        if (_isStart)
        {
            return;
        }

        index++;

        if (index >= _sprites.Length)
        {
            index = 53;
        }

        _spriteRenderer.sprite = _sprites[index];
    }

    private IEnumerator AppearanceThanos()
    {
        if (_animator != null)
        {
            _animator.SetBool(_thanos_Appearance, true);
        }

        Vector3 startPos = StartPosition.position;
        Vector3 targetPos = startPos + new Vector3(0, 0, -1.0f);

        float elapsed = 0f;
        while (elapsed < _appearanceDuration)
        {
            _thanos.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / _appearanceDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _thanos.transform.position = targetPos;

        PlayPunchingAnimation();
    }

    private void PlayPunchingAnimation()
    {
        if (_animator != null)
        {
            _animator.SetBool(_thanos_Appearance, false);
        }
    }

    public void OnPunchingAnimationEnd()
    {
        if (_thanos != null)
        {
            //_thanos.SetActive(false);
        }
    }
}

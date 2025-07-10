using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Button _buttonStart;
    [SerializeField] private Button _buttonQuit;
    [SerializeField] private Button _buttonSettings;
    [SerializeField] private GameObject _fadeinPanel;
    [SerializeField] private GameObject _gameTitleCanvas;
    [SerializeField] private GameObject _Anamo;
    [SerializeField] private GameObject _babyThanos;

    private float _fadeDuration = 2.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (_gameTitleCanvas == null)
        {
            _gameTitleCanvas = GameObject.Find("GameTitleCanvas");
        }

        if (_fadeinPanel == null)
        {
            _fadeinPanel = GameObject.Find("FadeInPanel");
        }

        if (_buttonStart == null)
        {
            _buttonStart = GameObject.Find("ButtonStart").GetComponent<Button>();
            _buttonStart.onClick.AddListener(OnStartButtonClick);
        }

        if (_buttonQuit == null)
        {
            _buttonQuit = GameObject.Find("ButtonQuit").GetComponent<Button>();
            _buttonQuit.onClick.AddListener(OnQuitButtonClick);
        }

        if (_buttonSettings == null)
        {
            _buttonSettings = GameObject.Find("ButtonSettings").GetComponent<Button>();
            _buttonSettings.onClick.AddListener(OnSettingsButtonClick);
        }

        if (_babyThanos == null)
        {
            _babyThanos = GameObject.Find("BabyThanos");
        }

        if (_Anamo == null)
        {
            _Anamo = GameObject.Find("Anamo");
        }
    }

    public void OnStartButtonClick()
    {
        StartCoroutine(C_FadeInAndStartGame());
    }

    private IEnumerator C_FadeInAndStartGame()
    {
        Image fadeImage = _fadeinPanel.GetComponent<Image>();
        Color color = fadeImage.color;
        color.a = 0f; // 시작은 완전 투명
        fadeImage.color = color;

        float elapsedTime = 0f;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / _fadeDuration); // 0~1 사이로 보정
            color.a = alpha;
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;

        yield return new WaitForSeconds(2.0f);
        _gameTitleCanvas.SetActive(false);

        // 오브젝트 파괴 (필요시)
        if (_babyThanos != null) Destroy(_babyThanos);
        if (_Anamo != null) Destroy(_Anamo);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }

    public void OnSettingsButtonClick()
    {
        Application.Quit();
    }
}

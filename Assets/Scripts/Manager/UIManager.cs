using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject _gameManagerObject; // 인스펙터에서 무조건 연결

    [SerializeField] private Button _buttonStart;
    [SerializeField] private Button _buttonQuit;
    [SerializeField] private Button _buttonSettings;

    [SerializeField] private GameObject _fadeinPanel;
    [SerializeField] private GameObject _gameTitleCanvas;
    [SerializeField] private GameObject _babyThanos;
    [SerializeField] private GameObject _curtains;
    [SerializeField] private GameObject _titleMainPanel;

    [SerializeField] private Animator _curtainsAnimator;
    [SerializeField] private Animator _thanosAnimator;

    private float _fadeDuration = 2.0f;
    readonly int _curtainOpenTrigger = Animator.StringToHash("CurtainOpen");
    readonly int _curtainCloseTrigger = Animator.StringToHash("CurtainClose");
    readonly int _thanosDanceBoolean = Animator.StringToHash("IsDancing");
    readonly int _thanosAyaTrigger = Animator.StringToHash("Thanos_Aya");
    readonly int _thanosFingersnapTrigger = Animator.StringToHash("Thanos_Fingersnap");


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

        if (_titleMainPanel == null)
        {
            _titleMainPanel = GameObject.Find("TitleMainPanel");
        }

        if (_babyThanos == null)
        {
            _babyThanos = GameObject.Find("BabyThanos");
        }

        if (_curtains == null)
        {
            _curtains = GameObject.Find("Curtains");
        }   

        if (_curtainsAnimator == null && _curtains != null)
        {
            _curtainsAnimator = _curtains.GetComponent<Animator>();
        }

        if (_thanosAnimator == null && _babyThanos != null)
        {
            _thanosAnimator = _babyThanos.GetComponent<Animator>();
        }
    }

    public void OnStartButtonClick()
    {
        _titleMainPanel.SetActive(false);
        _curtainsAnimator.SetTrigger(_curtainOpenTrigger);
        _thanosAnimator.SetBool(_thanosDanceBoolean, true);

        StartCoroutine(WaitForCurtainAnimationAndStartGame());
    }

    private IEnumerator WaitForCurtainAnimationAndStartGame()
    {
        yield return new WaitForSeconds(3.0f);

        yield return StartCoroutine(C_FadeInAndStartGame());
    }

    private IEnumerator C_FadeInAndStartGame()
    {
        Image fadeImage = _fadeinPanel.GetComponent<Image>();
        Color color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;

        float elapsedTime = 0f;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / _fadeDuration);
            color.a = alpha;
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;

        yield return new WaitForSeconds(2.0f);
        _gameTitleCanvas.SetActive(false);

        if (_babyThanos != null)
        {
            _thanosAnimator.SetBool(_thanosDanceBoolean, false);
            _babyThanos.SetActive(false);
        }

        if (_curtains != null)
        {
            Destroy(_curtains);
        }

        Camera.main.transform.position = new Vector3(0, 0, -7);
        _gameManagerObject.SetActive(true);
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

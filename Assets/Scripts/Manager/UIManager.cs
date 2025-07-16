using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject _gameManagerObject; // 인스펙터에서 무조건 연결

    [SerializeField] private Button _buttonStart;
    [SerializeField] private Button _buttonQuit;

    [SerializeField] private GameObject _fadeinPanel;
    [SerializeField] private GameObject _gameTitleCanvas;
    [SerializeField] private GameObject _babyThanos;
    [SerializeField] private GameObject _curtains;
    [SerializeField] private GameObject _titleMainPanel;

    [SerializeField] private GameObject _gameEndingObject;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _loseImage;
    [SerializeField] private GameObject _buttonGroup;
    [SerializeField] private Button _buttonRestart;
    [SerializeField] private Button _buttonExit;

    [SerializeField] private Animator _curtainsAnimator;
    [SerializeField] private Animator _thanosAnimator;

    private float _fadeDuration = 2.0f;
    readonly int CURTAIN_OPEN = Animator.StringToHash("CurtainOpen");
    readonly int IS_DANCING = Animator.StringToHash("IsDancing");
    private Image _fadeImage;

    [SerializeField] private List<GameObject> _newCurtains;

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

        _newCurtains = new List<GameObject>();
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

        //if (_buttonSettings == null)
        //{
        //    _buttonSettings = GameObject.Find("ButtonSettings").GetComponent<Button>();
        //    _buttonSettings.onClick.AddListener(OnSettingsButtonClick);
        //}

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

        if (_gameEndingObject == null)
        {
            _gameEndingObject = GameObject.Find("GameEnding");
        }

        if (_losePanel == null)
        {
            _losePanel = GameObject.Find("LosePanel");
        }

        if (_winPanel == null)
        {
            _winPanel = GameObject.Find("WinPanel");
        }

        if (_buttonGroup == null)
        {
            _buttonGroup = GameObject.Find("ButtonGroup");
        }

        if (_buttonRestart == null)
        {
            _buttonRestart = GameObject.Find("ButtonRestart").GetComponent<Button>();
        }

        if (_buttonExit == null)
        {
            _buttonExit = GameObject.Find("ButtonExit").GetComponent<Button>();
        }

        _buttonRestart.onClick.AddListener(() => { GameManager.Instance.RestartGame(); });
        _buttonExit.onClick.AddListener(() => { Application.Quit(); });

        GameObject alembicPlayerController = GameObject.Find("StageCurtainL");
        GameObject alembicPlayerController2 = GameObject.Find("StageCurtainR");
        _newCurtains.Add(alembicPlayerController);
        _newCurtains.Add(alembicPlayerController2);
        SetCurtain(false);
        SoundManager.Instance.PlayBGM("Title");
    }

    public void OnStartButtonClick()
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayBGM("InGame");
        _thanosAnimator.SetBool(IS_DANCING, true);
        _titleMainPanel.SetActive(false);

        StartCoroutine(WaitForCurtainAnimationAndStartGame());
        SetCurtain(true);
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
            _thanosAnimator.SetBool(IS_DANCING, false);
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

    public void ShowEndingPanel(bool isWin)
    {
        _gameEndingObject.SetActive(true);

        if (isWin)
        {
            _winPanel.SetActive(true);
            _losePanel.SetActive(false);
            _fadeImage = GameObject.Find("WinBackground").GetComponent<Image>();
        }
        else
        {
            _winPanel.SetActive(false);
            _losePanel.SetActive(true);
            _fadeImage = GameObject.Find("LoseBackground").GetComponent<Image>();
        }

        StartCoroutine(C_FadeInPanel(_fadeImage));
    }

    private IEnumerator C_FadeInPanel(Image fadeInImage)
    {
        Color color = fadeInImage.color;
        color.a = 0f;
        fadeInImage.color = color;

        float elapsedTime = 0f;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / _fadeDuration);
            color.a = alpha;
            fadeInImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeInImage.color = color;

        yield return new WaitForSeconds(1.0f);
        _loseImage.SetActive(true);
        _buttonGroup.SetActive(true);
    private void SetCurtain(bool flag)
    {
        foreach (var curtain in _newCurtains)
        {
            if (curtain != null)
            {
                AlembicPlayerController controller = curtain.GetComponent<AlembicPlayerController>();
                controller.enabled = flag;
            }
        }
    }
}

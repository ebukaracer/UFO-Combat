using Racer.LoadManager;
using Racer.SaveManager;
using Racer.SoundManager;
using Racer.Utilities;
using TMPro;
using UnityEngine;

internal class UIControllerMain : MonoBehaviour
{
    private SoundManager _soundManager;
    private MainUITween _mainUITween;

    private bool _isValueChanged;
    private bool _isInfoPaneOpen, _isUserOpen;

    private int _maxWaveAmount;
    private int _soundIndex, _shakeIndex, _inputIndex, _guideIndex;

    private float _speedAmount;
    private readonly float _minSpeed = 5, _maxSpeed = 8;

    private readonly string[] _soundOptions = { "Sound: On", "Sound: Off" }; // 'On' by default
    private readonly string[] _shakeOptions = { "Shake: On", "Shake: Off" }; // 'On' by default
    private readonly string[] _inputOptions = { "Mouse", "Keyboard" }; // 'Mouse' by default

    [SerializeField, Multiline] private string[] infoGuides;

    [Space(5), Header("TEXTS")]
    [SerializeField] private TextMeshProUGUI soundText;
    [SerializeField] private TextMeshProUGUI shakeText;
    [SerializeField] private TextMeshProUGUI inputText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI guideText;

    [Space(5), Header("AUDIOS")]
    [SerializeField] private AudioClip mainMusic;
    [SerializeField] private AudioClip titleSfx;

    [Space(5), Header("OTHERS")]
    [SerializeField] private ParticleSystem titleFx;
    [SerializeField] private GameObject claimTitleBtn;

    private void Awake()
    {
        _mainUITween = GetComponent<MainUITween>();
        _soundManager = SoundManager.Instance;

        RetrieveValues();

        _soundManager.GetMusic().clip = mainMusic;
        _soundManager.PlayMusic();

        SoundLogic();
    }

    private void Start()
    {
        InitTexts();
    }

    #region UI Animation
    public void InfoPane()
    {
        _isInfoPaneOpen = !_isInfoPaneOpen;

        _mainUITween.DisplayInfoUI(_isInfoPaneOpen);
    }

    public void UserPane()
    {
        _isUserOpen = !_isUserOpen;

        _mainUITween.DisplayUserUI(_isUserOpen);
    }
    #endregion

    #region Options Pane
    public void ToggleSound()
    {
        _soundIndex++;

        _soundIndex %= _soundOptions.Length;

        soundText.text = _soundOptions[_soundIndex];

        _isValueChanged = true;

        SoundLogic();

    }

    public void ToggleShake()
    {
        _shakeIndex++;

        _shakeIndex %= _shakeOptions.Length;

        shakeText.text = _shakeOptions[_shakeIndex];

        _isValueChanged = true;
    }

    public void ToggleInput()
    {
        _inputIndex++;

        _inputIndex %= _inputOptions.Length;

        inputText.text = _inputOptions[_inputIndex];

        _isValueChanged = true;
    }

    public void SwitchSpeed(bool isIncrease)
    {
        if (isIncrease)
            Increase();
        else
            Decrease();

        speedText.SetText("Speed: {0}", _speedAmount);

        void Increase()
        {
            if (_speedAmount < _maxSpeed)
                _speedAmount += .5f;
        }

        void Decrease()
        {
            if (_speedAmount > _minSpeed)
                _speedAmount -= .5f;
        }

        _isValueChanged = true;
    }

    public void SwitchGuide()
    {
        _guideIndex++;

        var turn = _guideIndex %= infoGuides.Length - 3;

        if (turn == 0)
            _guideIndex = 1;

        guideText.text = infoGuides[_guideIndex];
    }


    public void SwitchTheme()
    {
        // Yet to be Implemented...
    }

    public void ResetGame()
    {
        SaveManager.ClearAllPrefs();

        // Caution, Reloads game after erasing to apply changes
        LoadManager.Instance.LoadSceneAsync(0);
    }
    #endregion

    public void ShowCustomInfo(int index)
    {
        if (!_isInfoPaneOpen)
        {
            _guideIndex = 0;

            guideText.text = infoGuides[index];

            InfoPane();
        }
        else
            InfoPane();
    }

    public void HideAllPanes()
    {
        if (_isUserOpen)
            UserPane();

        if (_isInfoPaneOpen)
            InfoPane();
    }

    private void SoundLogic()
    {
        // On
        if (_soundIndex <= 0)
        {
            _soundManager.EnableMusic(true);
            _soundManager.EnableSfx(true);
        }
        // Off
        else
        {
            _soundManager.EnableMusic(false);
            _soundManager.EnableSfx(false);
        }
    }

    /// <summary>
    /// Saves changed values in the option pane.
    /// </summary>
    /// <remarks>
    /// Happens as user exits(game) or leaves the option pane via the 'Back Button'.
    /// </remarks>
    public void SaveValues()
    {
        if (!_isValueChanged)
            return;

        SaveManager.SaveFloat(Metrics.SpeedCount, _speedAmount);
        SaveManager.SaveInt(Metrics.InputIndex, _inputIndex);
        SaveManager.SaveInt(Metrics.SoundIndex, _soundIndex);
        SaveManager.SaveInt(Metrics.ShakeIndex, _shakeIndex);

        _isValueChanged = false;
    }

    /// <summary>
    /// Retrieves all saved-values.
    /// </summary>
    /// <remarks>
    /// Happens at every start of the game or whenever user is in this scene.
    /// </remarks>
    private void RetrieveValues()
    {
        _speedAmount = SaveManager.GetFloat(Metrics.SpeedCount, _minSpeed);
        _inputIndex = SaveManager.GetInt(Metrics.InputIndex);
        _soundIndex = SaveManager.GetInt(Metrics.SoundIndex);
        _shakeIndex = SaveManager.GetInt(Metrics.ShakeIndex);

        // TODO: Possible duplicate!
        _maxWaveAmount = CcEncrypter.DecryptAndGet(Metrics.EncryptId,
            SaveManager.GetInt(Metrics.SaveId, 1),
            Metrics.Offset);

        // Just in case!
        if (_maxWaveAmount == -1)
            _maxWaveAmount = 0;

        if (IsTitleReady && SaveManager.GetBool(Metrics.HasClaimed))
        {
            DisplayTitleLabel(false);
            claimTitleBtn.ToggleActive(false);
        }
        else
            claimTitleBtn.ToggleActive(true);
    }

    /// <summary>
    /// Initializes the UI-text values using their saved values.
    /// </summary>
    private void InitTexts()
    {
        speedText.SetText("Speed: {0}", _speedAmount);
        waveText.SetText("Max Wave: {0}", _maxWaveAmount);

        shakeText.text = _shakeOptions[_shakeIndex];
        inputText.text = _inputOptions[_inputIndex];
        soundText.text = _soundOptions[_soundIndex];
    }

    public void ClaimTitle()
    {
        if (IsTitleReady)
        {
            ShowCustomInfo(infoGuides.Length - 1);

            DisplayTitleLabel(true);
            claimTitleBtn.ToggleActive(false);
        }
        else
            ShowCustomInfo(infoGuides.Length - 2);
    }

    private void DisplayTitleLabel(bool firstTimeInit)
    {
        _mainUITween.DisplayTitleIcon();

        if (!firstTimeInit) return;

        _soundManager.PlaySfx(titleSfx);
        titleFx.Play();

        SaveManager.SaveBool(Metrics.HasClaimed, true);
    }

    private bool IsTitleReady => _maxWaveAmount >= Metrics.ProGamerCount;

    public void PlayGame()
    {
        HideAllPanes();

        LoadManager.Instance.LoadSceneAsync(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        SaveValues();
    }
}

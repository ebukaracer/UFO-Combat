using Racer.LoadManager;
using Racer.SaveManager;
using Racer.SoundManager;
using Racer.Utilities;
using TMPro;
using UnityEngine;

internal class UIControllerGame : SingletonPattern.StaticInstance<UIControllerGame>
{
    private SoundManager _soundManager;
    private GameUITween _uiTween;

    private readonly string[] _menuTexts = { "Paused", "Game over" };
    private readonly string[] _infoTexts = { "[Space] to ", "Pause", "Resume" };

    private int _savedWave;
    private bool _hasMaxWave;
    private bool _isGameover;

    public int CurrentWave { get; private set; } = 1;

    [field: SerializeField,
    Space(5), Header("REFERENCES")]
    public FillBar PickupFill { get; private set; }

    [Space(5), Header("TEXTS")]
    [SerializeField] private TextMeshProUGUI[] waveText;
    [SerializeField] private TextMeshProUGUI maxWaveText;
    [SerializeField] private TextMeshProUGUI menuText;
    [SerializeField] private TextMeshProUGUI infoText;

    [Space(5), Header("CLIPS")]
    [SerializeField] private AudioClip sfxHint;
    [SerializeField] private AudioClip sfxXplode;
    [SerializeField] private AudioClip sfxNewWave;

    [Space(5), Header("OTHERS")]
    [SerializeField] private ParticleSystem fxNewWave;
    [SerializeField] private AudioClip gameMusic;


    protected override void Awake()
    {
        base.Awake();

        _uiTween = GetComponent<GameUITween>();

        _soundManager = SoundManager.Instance;

        _savedWave = CcEncrypter.DecryptAndGet(Metrics.EncryptId,
            SaveManager.GetInt(Metrics.SaveId, 1),
            Metrics.Offset);


        // Just in case!
        if (_savedWave == -1)
            _savedWave = 0;

        _soundManager.GetMusic().clip = gameMusic;
        _soundManager.PlayMusic();

        // If Sound was not muted before...
        if (SaveManager.GetInt(Metrics.SoundIndex) <= 0)
            _soundManager.EnableMusic(true);
    }
    private void Start()
    {
        PickupFill.OnDecreaseStarted += PickupFill_OnDecreaseStarted;
        PickupFill.OnDecreaseFinished += PickupFill_OnDecreaseFinished;

        GameManager.OnCurrentState += GameManager_OnCurrentState;
    }

    private void GameManager_OnCurrentState(GameStates state)
    {
        switch (state)
        {
            case GameStates.Playing:
                _uiTween.DisplayMainUI(true);
                _soundManager.PlaySfx(sfxHint);
                break;

            case GameStates.GameOver:
                {
                    _isGameover = true;
                    CalculateWave();
                    menuText.text = _menuTexts[1];
                    _uiTween.DisplayMainUI(false);
                    _uiTween.DisplayMenuUI(true);
                    Invoke(nameof(OnGameOver), .25f); // weird
                    _soundManager.EnableMusic(false);
                }
                break;

            case GameStates.Exit:
                _soundManager.EnableMusic(false);
                _soundManager.GetSnapShot(0).TransitionTo(0);
                break;
        }
    }

    private void PickupFill_OnDecreaseFinished()
    {
        _uiTween.PlayBulletUI(false);
    }

    private void PickupFill_OnDecreaseStarted()
    {
        _uiTween.PlayBulletUI(true);
    }

    public void Pause(bool isPause)
    {
        if (isPause)
        {
            infoText.text = _infoTexts[0] + _infoTexts[2];
            menuText.text = _menuTexts[0];
            _uiTween.DisplayMenuUI(true);
            _soundManager.GetSnapShot(1).TransitionTo(0);
        }
        else
        {
            _uiTween.DisplayMenuUI(false);
            infoText.text = _infoTexts[0] + _infoTexts[1];
            _soundManager.GetSnapShot(0).TransitionTo(0);
        }

        _soundManager.PlaySfx(sfxHint);
    }

    public void OnGameOver()
    {
        _soundManager.PlaySfx(sfxHint);

        if (!_hasMaxWave) return;

        _uiTween.PlayNewWaveUI();

        _soundManager.PlaySfx(sfxNewWave);

        fxNewWave.Play();
    }

    public void IncrementWave(int amt)
    {
        _soundManager.PlaySfx(sfxXplode);

        CurrentWave += amt;

        waveText[0].SetText("Wave: {0}", CurrentWave);
        waveText[1].text = waveText[0].text;

        _uiTween.DisplayWaveUI();
    }

    private void CalculateWave()
    {
        if (CurrentWave > _savedWave)
        {
            _hasMaxWave = true;

            SetWaveText(CurrentWave);

            SaveManager.SaveInt(Metrics.SaveId, CurrentWave);

            SaveManager.SaveString(Metrics.EncryptId,
                CcEncrypter.SaveAndEncrypt(CurrentWave, Metrics.Offset));
        }
        else
            SetWaveText(_savedWave);
    }

    private void SetWaveText(int value)
    {
        maxWaveText.SetText("Max Wave: {0}", value);
    }

    // Exits or restarts the current scene.
    public void LeaveGame(int sceneIndex)
    {
        LoadManager.Instance.LoadSceneAsync(sceneIndex);

        if (!_isGameover)
            GameManager.Instance.SetGameState(GameStates.Exit);

        _uiTween.DisplayMenuUI(false);
    }

    private void OnDestroy()
    {
        GameManager.OnCurrentState -= GameManager_OnCurrentState;

        PickupFill.OnDecreaseStarted -= PickupFill_OnDecreaseStarted;
        PickupFill.OnDecreaseFinished -= PickupFill_OnDecreaseFinished;
    }
}

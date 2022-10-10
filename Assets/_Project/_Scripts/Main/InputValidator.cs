using Racer.SaveManager;
using TMPro;
using UnityEngine;

/// <summary>
/// This handles player's username text input.
/// </summary>
internal class InputValidator : MonoBehaviour
{
    private MainUITween _mainUITween;

    // Default text if no input was entered.
    private const string DefaultText = "USER_001";
    private string _inputText;

    // Input Field
    [SerializeField] private TMP_InputField inputField;

    // Text Label
    [SerializeField] private TextMeshProUGUI nameText;


    private void Awake()
    {
        _mainUITween = GetComponent<MainUITween>();
    }

    private void Start()
    {
        _inputText = SaveManager.GetString(Metrics.Username);

        ShowInputFieldStartup();

        inputField.onEndEdit.AddListener(InitInputFieldStartup);

        inputField.ActivateInputField();

        InitInputTextField();
    }

    /// <summary>
    /// Stores and saves player's inputted username in a temporary variable.
    /// Initialized the very first time game is loaded.
    /// </summary>
    /// <param name="t">input text(username)</param>
    private void InitInputFieldStartup(string t)
    {
        _inputText = string.IsNullOrWhiteSpace(t) ? DefaultText : t;

        SaveManager.SaveString("Username", _inputText);
    }

    /// <summary>
    /// Exits the input field via an exit button.
    /// This is assigned to a exit-button on the input-field panel
    /// </summary>
    public void ExitInputFieldStartup()
    {
        _mainUITween.DisplayUserInputUI(false);

        InitInputTextField();
    }

    /// <summary>
    /// Enables the input-field(startup) the very first time game loads.
    /// </summary>
    private void ShowInputFieldStartup()
    {
        if (string.IsNullOrEmpty(_inputText))
            _mainUITween.DisplayUserInputUI(true);
    }


    /// <summary>
    /// Loads and Initializes the value entered in the two input-fields when game starts
    /// </summary>
    private void InitInputTextField()
    {
        nameText.text = _inputText;
    }
}
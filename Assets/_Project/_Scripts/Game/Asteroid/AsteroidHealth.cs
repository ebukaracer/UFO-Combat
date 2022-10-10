using TMPro;
using UnityEngine;

internal class AsteroidHealth : MonoBehaviour
{
    // Default values
    private int _minHealth = 10, _maxHealth = 16;

    private UIControllerGame _uIController;

    public int FinalHealth { get; private set; }
    public bool IsDead { get; private set; }

    [field: SerializeField, Tooltip("This field is read-only!")]
    public int InitialHealth { get; private set; }

    [SerializeField] private TextMeshPro healthNum;


    private void Start()
    {
        _uIController = UIControllerGame.Instance;
    }

    private void OnEnable()
    {
        InitialHealth = GenerateInitialHealth();

        FinalHealth = InitialHealth;

        IsDead = false;

        SetText(InitialHealth);
    }

    public void TakeDamage(int dmgAmt)
    {
        FinalHealth -= dmgAmt;

        SetText(FinalHealth);

        if (FinalHealth <= 0)
            IsDead = true;
    }

    private void SetText(int t) => healthNum.SetText("{0}", t);

    private int GenerateInitialHealth()
    {
        // Based on game's current wave or highscore
        if ((!_uIController) || _uIController.CurrentWave <= Metrics.MidDifficultyAmount)
            return Random.Range(_minHealth, _maxHealth);

        _minHealth = 15;

        _maxHealth = 26;

        return Random.Range(_minHealth, _maxHealth);
    }
}

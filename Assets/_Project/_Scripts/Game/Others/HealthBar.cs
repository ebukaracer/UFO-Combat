using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

// TODO Change from Async to Coroutine
// TODO Refactor from base script location, merge with this.
internal class HealthBar : MonoBehaviour
{
    private const float MinHealth = 0;
    private float _currentHealth;
    private bool _isHealthFull;
    private bool _isBusy;

    public event Action OnHealthEmpty;
    public int HealthCount { get; private set; }

    [Header("HEALTH VALUES")]
    [Range(1, 100)] 
    [SerializeField] private float initialHealth = 100f;
    [SerializeField] private float healthBarSmoothTime = .3f;

    [Space(5), SerializeField] private Image[] healthBars;


    private void Awake()
    {
        HealthCount = healthBars.Length;

        _currentHealth = initialHealth * HealthCount;

        _isHealthFull = true;
    }

    /// <summary>
    /// Increases one of the health-bars by a specified amount.
    /// </summary>
    public async void IncreaseHealth(float amount)
    {
        if (_isHealthFull || _isBusy)
            return;

        _currentHealth = _currentHealth % initialHealth == 0 ? MinHealth : _currentHealth;
        _currentHealth += amount;
        _currentHealth = Mathf.Min(initialHealth, _currentHealth);

        var finalHealth = _currentHealth / initialHealth;

        for (int i = HealthCount - 1; i >= 0; i--)
        {
            if (healthBars[i].fillAmount < 1)
            {
                await SmoothHealthBar(healthBars[i], finalHealth);
                break;
            }
        }

        // Health is full if the first health-bar is full.
        if (healthBars[0].fillAmount >= 1)
            _isHealthFull = true;
    }

    /// <summary>
    /// Decreases one of the health-bars by a specified amount.
    /// </summary>
    public async void DecreaseHealth(float amount)
    {
        if (_isBusy)
            return;

        _isHealthFull = false;

        _currentHealth = _currentHealth % initialHealth == 0 ? initialHealth : _currentHealth;
        _currentHealth -= amount;
        _currentHealth = Mathf.Max(MinHealth, _currentHealth);

        var finalHealth = _currentHealth / initialHealth;

        for (int i = 0; i < HealthCount; i++)
        {
            if (healthBars[i].fillAmount > 0)
            {
                await SmoothHealthBar(healthBars[i], finalHealth);
                break;
            }
        }
    }

    private async Task SmoothHealthBar(Image fill, float amount)
    {
        var preChangeAmount = fill.fillAmount;

        float elapsed = 0;

        while (elapsed < healthBarSmoothTime)
        {
            _isBusy = true;

            elapsed += Time.deltaTime;

            fill.fillAmount = Mathf.Lerp(preChangeAmount, amount, elapsed / healthBarSmoothTime);

            await Task.Yield();
        }

        _isBusy = false;

        fill.fillAmount = amount;

        // Health is empty if the last health-bar is empty.
        if (healthBars[HealthCount - 1].fillAmount <= 0)
            OnHealthEmpty?.Invoke();
    }
}
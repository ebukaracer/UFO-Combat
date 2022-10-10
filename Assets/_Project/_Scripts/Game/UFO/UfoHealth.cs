using Racer.ObjectPooler;
using Racer.Utilities;
using UnityEngine;

internal class UfoHealth : PoolObject
{
    private Animator _animator;
    private ParticleSystem _xplodeFx;

    private UfoSpawner _ufoSpawner;
    private HealthSpawner _healthSpawner;
    private Missile _missile;

    private bool _isDead;
    private int _currentHealth;

    [SerializeField, Tooltip("Read-only field")]
    private int initialHealth = 10;


    private void Awake()
    {
        _xplodeFx = GameManager.Instance.TagSet.UfoHurtFx;

        _ufoSpawner = GameManager.Instance.TagSet.UfoSpawner;

        _healthSpawner = GameManager.Instance.TagSet.HealthSpawner;

        _animator = GetComponent<Animator>();
    }

    private void OnEnable() => Init();

    private void Init()
    {
        initialHealth = (int)Random.Range(10, maxInclusive: 20);

        _currentHealth = initialHealth;

        _isDead = false;
    }

    // Take Damage on collision with missile.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isDead)
            return;

        if (!collision.CompareTag(Tags.Missile)) return;

        _animator.SetTrigger(Utility.GetAnimId("Hurt"));

        // Damage amount depends on type of missile.
        TakeDamage(collision.TryGetComponent(out _missile) ? _missile.DmgPerHit : 1, transform.position);
    }

    private void TakeDamage(int amount, Vector2 pos = default)
    {
        _currentHealth -= amount;

        if (_currentHealth > 0) return;

        _isDead = true;

        PlayFx(pos);

        _ufoSpawner.SpawnUfo();

        _healthSpawner.SpawnHealth(transform.position);

        UIControllerGame.Instance.IncrementWave(1);

        Despawn();
    }

    private void PlayFx(Vector2 pos)
    {
        _xplodeFx.transform.position = pos;

        _xplodeFx.Play();
    }

    public override void Despawn()
    {
        base.Despawn();

        GetComponent<Ufo>().OnDeSpawn();
    }
}

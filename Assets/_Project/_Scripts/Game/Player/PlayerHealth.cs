using Racer.ObjectPooler;
using Racer.SaveManager;
using Racer.SoundManager;
using UnityEngine;

internal class PlayerHealth : MonoBehaviour
{
    private PlayerCollision _playerCollision;
    private AsteroidHealth _asteroidHealth;

    private bool _isDead;
    private bool _isShakeOn;

    [SerializeField] private HealthBar healthBar;

    [Space(5), SerializeField]
    private ParticleSystem[] dmgFxs;

    [Space(5), Header("CLIPS")]
    [SerializeField] private AudioClip sfxHealth;
    [SerializeField] private AudioClip sfxDamage;
    [SerializeField] private AudioClip sfxDamageHit;
    [SerializeField] private AudioClip sfxDestroy;


    private void Awake()
    {
        _playerCollision = GetComponent<PlayerCollision>();
    }

    private void Start()
    {
        healthBar.OnHealthEmpty += HealthBar_OnHealthEmpty;
        _playerCollision.OnCollisionCallback += PlayerCollision_CollisionCallback;

        _isShakeOn = SaveManager.GetInt(Metrics.ShakeIndex) == 0;
    }


    private void HealthBar_OnHealthEmpty()
    {
        _isDead = true;

        GameManager.Instance.SetGameState(GameStates.GameOver);
        SoundManager.Instance.PlaySfx(sfxDestroy);
    }


    // Health Modification
    private void PlayerCollision_CollisionCallback(Collider2D col)
    {
        if (_isDead) return;

        var go = col.gameObject;

        // Health Increase
        if (go.CompareTag(Tags.Health))
        {
            if (go.TryGetComponent(out PoolObject healthPickup))
            {
                healthBar.IncreaseHealth(HealthGainAmt);

                healthPickup.Despawn();

                SoundManager.Instance.PlaySfx(sfxHealth);
            }
        }

        // Health Decrease
        if (go.CompareTag(Tags.HomingObj))
        {
            if (go.TryGetComponent(out HomingObject hmObj))
            {
                TakeDamage(25);

                Shake(.5f);

                hmObj.Despawn();

                SoundManager.Instance.PlaySfx(sfxDamageHit);
            }

            PlayHurtFx(col.ClosestPoint(transform.position), 1);
        }

        // Health Decrease
        if (!go.CompareTag(Tags.Asteroid)) return;

        if (go.TryGetComponent(out _asteroidHealth))
        {
            TakeDamage(_asteroidHealth.FinalHealth);

            Shake();

            SoundManager.Instance.PlaySfx(sfxDamage);
        }

        PlayHurtFx(col.ClosestPoint(transform.position), 0);
    }

    private static int HealthGainAmt => Random.Range(15, 26);

    private void Shake(float magnitude = .25f)
    {
        if (!_isShakeOn) return;

        GameManager.Instance.ShakeCam(.25f, magnitude);
    }

    private void TakeDamage(float dmgAmt) => healthBar.DecreaseHealth(dmgAmt);

    // Particle Fx
    private void PlayHurtFx(Vector2 position, int fxIndex)
    {
        dmgFxs[fxIndex].transform.position = position;
        dmgFxs[fxIndex].Play();
    }

    private void OnDestroy()
    {
        healthBar.OnHealthEmpty -= HealthBar_OnHealthEmpty;
        _playerCollision.OnCollisionCallback -= PlayerCollision_CollisionCallback;
    }
}

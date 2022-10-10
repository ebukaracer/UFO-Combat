using Racer.SoundManager;
using Racer.Utilities;
using System.Collections;
using Racer.ObjectPooler;
using UnityEngine;

internal class Asteroid : PoolObject
{
    private Rigidbody2D _rb2D;
    private SpriteRenderer _spR;
    private GameManager _gameManager;

    private AsteroidHealth _asteroidHealth;
    private Missile _missile;

    private Vector2 _lastFrameVel;
    private Vector2 _currentScale;

    private bool _isGameover;
    private readonly float[] _initialBounceDirections = { -1, 1 };

    [SerializeField, Tooltip("Scale Animation time")]
    private float getToTime = .15f;

    [Header("Motion")]

    [Space(5), SerializeField]
    private float minVel;
    [SerializeField] private float initialVelMultiplier;
    [SerializeField] private float torqueAmount;

    [Space(5), SerializeField] private AudioClip sfx;

    // One-time reference throughout this gameobject's lifetime.
    private void Awake()
    {
        _currentScale = transform.localScale;

        _spR = GetComponent<SpriteRenderer>();

        _rb2D = GetComponent<Rigidbody2D>();

        _asteroidHealth = GetComponent<AsteroidHealth>();

        _gameManager = GameManager.Instance;

        GameManager.OnCurrentState += GameManager_OnCurrentState;
    }

    private void GameManager_OnCurrentState(GameStates state)
    {
        _isGameover = state.Equals(GameStates.GameOver) || state.Equals(GameStates.Exit);
    }

    // Called when an asteroid is released from pool,
    // Called from another script.
    public void Init()
    {
        Attacker.TotalPresentAttackers.Add(gameObject);

        ChangeColor();

        StartCoroutine(ScaleUpFx(transform));


        IEnumerator ScaleUpFx(Transform clone)
        {
            var initialScale = _currentScale;

            var finalScale = clone.transform.localScale = Vector3.zero;

            float elapsed = 0;

            while (elapsed < getToTime)
            {
                elapsed += Time.deltaTime;

                clone.transform.localScale = Vector3.Lerp(finalScale, initialScale, elapsed / getToTime);

                yield return 0;
            }

            clone.transform.localScale = initialScale;

            _rb2D.velocity = new Vector2(_initialBounceDirections[Random.Range(0, 2)],
                                      -Random.Range(2, maxInclusive: 4)) * initialVelMultiplier;
        }
    }

    private void Update()
    {
        _lastFrameVel = _rb2D.velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Bound) || collision.CompareTag(Tags.Ground) || collision.CompareTag(Tags.Player))
        {
            if (_isGameover)
            {
                StartCoroutine(ScaleDownFx(transform));

                return;
            }

            var point = collision.bounds.ClosestPoint(transform.position);

            var normal = transform.position - point;

            Bounce(normal.normalized);
        }

        // Decreases health on collision with a missile.
        if (collision.CompareTag(Tags.Missile) && !_asteroidHealth.IsDead)
        {
            if (collision.gameObject.TryGetComponent(out _missile))
                _asteroidHealth.TakeDamage(_missile.DmgPerHit);

            // Spawns a random missile-pickup when destroyed.
            if (_asteroidHealth.FinalHealth < 1)
            {
                // Spawns a missile-pickup based on the asteroid's health amount
                if (_asteroidHealth.InitialHealth >= Metrics.PickupHealthRange)
                    _gameManager.SpawnPossiblePickupMissile(transform.position);

                SoundManager.Instance.PlaySfx(sfx, .5f);

                StartCoroutine(ScaleDownFx(transform));
            }
        }

        // Scale animation effect
        IEnumerator ScaleDownFx(Transform clone)
        {
            var initialScale = clone.transform.localScale;

            var finalScale = Vector3.zero;

            float elapsed = 0;

            while (elapsed < getToTime)
            {
                elapsed += Time.deltaTime;

                clone.transform.localScale = Vector3.Lerp(initialScale, finalScale, elapsed / getToTime);

                yield return 0;
            }

            clone.transform.localScale = finalScale;

            Despawn();
        }
    }

    // De-spawns if off-screen.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(Tags.Bound) && !collision.CompareTag(Tags.Ground)) return;

        var pos = transform.position;

        if (Mathf.Abs(pos.x) > Mathf.Abs(Utility.ScreenDimension.x) ||
            Mathf.Abs(pos.y) > Mathf.Abs(Utility.ScreenDimension.y))

            Despawn();
    }

    // Bounces on collision.
    private void Bounce(Vector2 collisionNormal)
    {
        var speed = _lastFrameVel.magnitude;

        var direction = Vector2.Reflect(_lastFrameVel.normalized, collisionNormal);

        _rb2D.AddTorque(Random.Range(-torqueAmount, torqueAmount));

        _rb2D.velocity = direction * Mathf.Max(speed, minVel);
    }

    // Similar to 'OnDisable'
    public override void Despawn()
    {
        base.Despawn();

        _lastFrameVel = Vector2.zero;
        _rb2D.velocity = Vector2.zero;

        Attacker.TotalPresentAttackers.Remove(gameObject);
    }

    // Assigns a random color to every asteroid generated.
    private void ChangeColor()
    {
        _spR.color = _gameManager.TagSet.ColorGenerator.GetColor();
    }

    private void OnDestroy()
    {
        GameManager.OnCurrentState -= GameManager_OnCurrentState;
    }
}
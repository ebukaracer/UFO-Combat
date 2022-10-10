using Racer.ObjectPooler;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Main Logic: Homing-like.
/// Class can be further refactored.
/// </summary>
internal class HomingObject : PoolObject
{
    // Caches
    private Transform _target;
    private Transform _spinner;
    private Rigidbody2D _rb;
    private SpriteRenderer _sprRend;

    private Vector2 _outOfBoundPos;
    private Vector2 _direction;
    private bool _isSpinner;
    private readonly float _spinSpeed = 1500;
    
    [Header("REFERENCES")]
    [SerializeField] private Sprite[] sprites;

    [Header("MOVEMENT")]
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float angularSpeed = 200;

    [Header("OUB"), Space(5)]
    [SerializeField] private Vector2[] outOfBoundPositions;

    private void Awake()
    {
        _sprRend = GetComponentInChildren<SpriteRenderer>();

        _rb = GetComponent<Rigidbody2D>();

        _target = GameManager.Instance.TagSet.Player.transform;

        _spinner = transform.GetChild(0);

        _outOfBoundPos = outOfBoundPositions[Random.Range(0, outOfBoundPositions.Length)];

        GameManager.OnCurrentState += GameManager_OnCurrentState;
    }

    private void GameManager_OnCurrentState(GameStates state)
    {
        if (!state.Equals(GameStates.GameOver)) return;

        _target = null;
    }

    public void Init()
    {
        Attacker.TotalPresentAttackers.Add(gameObject);

        _isSpinner = false;

        _sprRend.sprite = sprites[Random.Range(0, sprites.Length)];

        if (_sprRend.sprite.name.Equals("Spinner"))
            _isSpinner = true;
    }

    private void FixedUpdate()
    {
        if (_target)
            _direction = _rb.position - (Vector2)_target.position;
        else
            _direction = _rb.position - _outOfBoundPos;

        _direction.Normalize();

        var up = transform.up;
        var rotateAmount = Vector3.Cross(_direction, up).z;

        // Look rotation
        _rb.angularVelocity = rotateAmount * angularSpeed;
        _rb.velocity = up * moveSpeed;
    }

    private void Update()
    {
        if (!_isSpinner)
            return;

        _spinner.Rotate(_spinSpeed * Time.deltaTime * Vector3.forward, Space.World);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(Tags.Bound)) return;

        var currentPos = transform.position;

        if (Mathf.Abs(currentPos.x) > Mathf.Abs(collision.transform.position.x) &&
            Mathf.Abs(currentPos.y) > Mathf.Abs(collision.transform.position.y))
            Despawn();
    }

    public override void Despawn()
    {
        base.Despawn();

        _rb.velocity = Vector2.zero;

        _rb.angularVelocity = 0;

        if (_isSpinner)
            _spinner.localRotation = Quaternion.identity;

        Attacker.TotalPresentAttackers.Remove(gameObject);
    }

    private void OnDestroy()
    {
        GameManager.OnCurrentState -= GameManager_OnCurrentState;
    }
}

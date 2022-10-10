using Racer.SaveManager;
using UnityEngine;

internal class PlayerController : MonoBehaviour
{
    private Camera _mainCam;
    private Rigidbody2D _carRb;

    private Vector2 _movePos;
    private Vector2[] _initialPositions;

    private readonly float _offscreenX = 11.5f;
    private float _initialMoveSpeed;

    private int _randomPos;
    private bool _useMouse;
    private bool _isGameover;
    private bool _isGameInit;

    [Header("REFERENCES")]
    [SerializeField] private GameObject brokenPlayer;
    [SerializeField] private Rigidbody2D[] carRbTyres;

    [Space(5), Header("MOTION")]
    [SerializeField] private bool mouseInput;
    [SerializeField] private float moveSpeed = 2.5f;


    private void Awake()
    {
        _carRb = GetComponent<Rigidbody2D>();

        _initialPositions = new[] { Vector2.right, Vector2.left };

        _randomPos = Random.Range(0, 2);

        _isGameInit = true;

        // Move speed set from main menu.
        moveSpeed = SaveManager.GetFloat(Metrics.SpeedCount, moveSpeed);

        // Input type set from main menu.
        mouseInput = SaveManager.GetInt(Metrics.InputIndex) == 0;

        _initialMoveSpeed = moveSpeed / 2;

        _movePos = _carRb.position;

        _mainCam = Camera.main;
    }

    private void Start()
    {
        transform.position = new Vector2(_offscreenX * (_initialPositions[_randomPos]).x, transform.position.y);

        GameManager.OnCurrentState += GameManager_OnCurrentState;
    }

    private void GameManager_OnCurrentState(GameStates st)
    {
        switch (st)
        {
            case GameStates.Playing:
                _isGameInit = false;
                break;

            case GameStates.GameOver:
                {
                    _carRb.velocity = Vector3.zero;
                    _isGameover = true;
                    brokenPlayer.transform.position = new Vector2(transform.position.x, brokenPlayer.transform.position.y);
                    gameObject.SetActive(false);
                    brokenPlayer.SetActive(true);
                }
                break;

            case GameStates.Exit:
                _carRb.velocity = Vector3.zero;
                _isGameover = true;
                break;
        }
    }

    private void Update()
    {
        if (_isGameover || _isGameInit)
            return;

        if (mouseInput)
        {
            _useMouse = Input.GetMouseButton(0);

            _movePos.x = _mainCam.ScreenToWorldPoint(Input.mousePosition).x;
        }
        else
            _movePos.x = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        if (_isGameover)
            return;

        InitMovement();

        switch (_isGameInit)
        {
            case true:
                _carRb.MovePosition(Vector2.Lerp(_carRb.position, _movePos, moveSpeed * Time.fixedDeltaTime));
                break;
            default:
            {
                if (_useMouse)
                    _carRb.MovePosition(Vector2.Lerp(_carRb.position, _movePos, moveSpeed * Time.fixedDeltaTime));

                else if (!mouseInput)
                    _carRb.MovePosition(_carRb.position + (2 * moveSpeed * Time.fixedDeltaTime * _movePos));

                else
                    _carRb.velocity = Vector2.zero;
                break;
            }
        }
    }

    private void InitMovement()
    {
        if (!_isGameInit)
            return;

        if (Mathf.Abs(((Vector2)transform.position - Vector2.zero).x) > .1f)
        {
            moveSpeed = _initialMoveSpeed;

            if (mouseInput)
                _useMouse = true;

            _movePos.x = _initialPositions[_randomPos].x > 0 ? -1 : 1;
        }
        else
        {
            if (mouseInput)
                _useMouse = false;

            _movePos.x = 0f;

            _carRb.velocity = Vector2.zero;

            FreezeTyres();

            moveSpeed = _initialMoveSpeed * 2;

            GameManager.Instance.SetGameState(GameStates.Playing);
        }
    }

    private void FreezeTyres()
    {
        for (int i = 0; i < carRbTyres.Length; i++)
        {
            carRbTyres[i].velocity = Vector2.zero;
        }
    }

    private void OnDestroy() => GameManager.OnCurrentState -= GameManager_OnCurrentState;
}

using Racer.Utilities;
using System.Collections;
using UnityEngine;

internal class Ufo : MonoBehaviour
{
    // Spawn
    // Move: Hover-like
    // Delay
    // Drop Asteroid
    // Hide

    private BoxCollider2D _col2D;
    private Attacker _attacker;

    private bool _isBusy;
    private bool _wasHidden;

    private bool _isGamePlaying;

    private float _timer;
    private float _attackTimeCache;
    private readonly float _wallOffset = .95f;
    private readonly float _minDistance = .1f;

    private Vector2 _targetPosition;
    private Vector2 _hoverPosition;

    [SerializeField] private float moveSpeed = .7f;
    [SerializeField] private float hideDelay = 3f;
    [SerializeField] private float hideOffset = 2f;
    [SerializeField] private float attackTime = 5f;

    [Space(5), SerializeField] private Transform deployPos;



    private void Awake()
    {
        _col2D = GetComponent<BoxCollider2D>();

        _attacker = GameManager.Instance.TagSet.Attacker;

        _attackTimeCache = attackTime;

        // Y-Position to maintain ahead of player..
        _hoverPosition = new Vector2(transform.position.x, 4);

        GameManager.OnCurrentState += GameManager_OnCurrentState;
    }

    public void Init()
    {
        transform.position = HideYPos;

        _wasHidden = true;

        _targetPosition = RandomXPos;
    }


    private void GameManager_OnCurrentState(GameStates state)
    {
        switch (state)
        {
            case GameStates.Playing:
                _isGamePlaying = true;
                break;
            case GameStates.GameOver:
            case GameStates.Exit:
                {
                    StopCoroutine(nameof(Attack));
                    _isGamePlaying = false;
                    _wasHidden = false;
                    _isBusy = false;
                    _attacker.CanSpawnAsteroid = _isBusy;
                }
                break;
        }
    }


    private void Update()
    {
        _attacker.SpawnPosition = deployPos.position;

        var distance = _targetPosition - (Vector2)transform.position;

        if (_wasHidden)
        {
            if (Mathf.Abs(distance.x) > _minDistance)
                Move();
            else
            {
                // A frame is skipped here..
                _targetPosition = new Vector2(transform.position.x, _hoverPosition.y);

                if (Mathf.Abs(distance.y) > _minDistance)
                    Reveal();
                else if (distance.y != 0)
                    StartCoroutine(nameof(Attack));
            }
        }
        else
        {
            if (_isBusy) return;

            _targetPosition = new Vector2(transform.position.x, HideYPos.y);

            if (Mathf.Abs(distance.y) > _minDistance)
                Hide();
            else
            {
                _targetPosition = transform.position;

                // Remain hidden as long as game is over
                if (!_isGamePlaying)
                    return;

                // Delay can be replaced with another condition
                _timer += Time.deltaTime;

                if (!(_timer >= hideDelay) || Attacker.TotalPresentAttackers.Count >= Metrics.MaxAsteroidPresent) return;

                _timer = 0;

                _targetPosition = RandomXPos;

                _wasHidden = true;
            }
        }
    }

    private void Move()
    {
        transform.position = Vector2.Lerp(transform.position, _targetPosition, Time.deltaTime * moveSpeed);
    }

    private void Hide()
    {
        transform.Translate(moveSpeed * Time.deltaTime * new Vector2(0, HideYPos.y));
    }

    private void Reveal()
    {
        transform.Translate(moveSpeed * Time.deltaTime * new Vector2(0, -_hoverPosition.y));
    }

    private IEnumerator Attack()
    {
        _wasHidden = false;

        _isBusy = _isGamePlaying;

        if (IsFewDelay(out attackTime))
            _attacker.SpawnHomingObj();
        else
            _attacker.CanSpawnAsteroid = _isBusy;

        yield return Utility.GetWaitForSeconds(attackTime);

        _targetPosition = new Vector2(transform.position.x, HideYPos.y);

        _isBusy = false;

        _attacker.CanSpawnAsteroid = _isBusy;
    }

    private bool IsFewDelay(out float time)
    {
        var randomNumber = Random.Range(0, 5);

        if (randomNumber > 2 && UIControllerGame.Instance.CurrentWave >= Metrics.MidDifficultyAmount)
        {
            time = 1f;

            return true;
        }

        // Reset to it's original time.
        time = _attackTimeCache;

        return false;
    }

    // Hides only on the y-axis
    private Vector2 HideYPos => new(transform.position.x, ScreenPos.y + hideOffset);

    private static Vector2 ScreenPos => Utility.ScreenDimension;

    private Vector2 RandomXPos
    {
        get
        {
            var bounds = _col2D.bounds;
            var randX = Random.Range((-ScreenPos.x + bounds.extents.x) * _wallOffset, (ScreenPos.x - bounds.extents.x) * _wallOffset);

            return new Vector2(randX, transform.position.y);
        }
    }

    public void OnDeSpawn()
    {
        StopCoroutine(nameof(Attack));

        _wasHidden = false;

        _isBusy = false;

        _attacker.CanSpawnAsteroid = _isBusy;
    }

    private void OnDestroy()
    {
        GameManager.OnCurrentState -= GameManager_OnCurrentState;
    }
}

using Racer.Utilities;
using System.Collections;
using UnityEngine;

internal class LunchMissileSpawner : Spawner
{
    private Missile _missile;

    private bool _isGameover;

    public MissileProperty CurrentLunchMissile { get; set; }

    [Space(5), SerializeField] private Transform spawnOrigin;
    
    [SerializeField] private float nextSpawnDelay = .5f;


    private void Start()
    {
        GameManager.OnCurrentState += GameManager_OnCurrentState;

        CurrentLunchMissile = MissileSelector.Instance.GetMissile(MissileType.Default);
    }

    private void GameManager_OnCurrentState(GameStates state)
    {
        switch (state)
        {
            case GameStates.Playing:
                StartCoroutine(nameof(RandomSpawning));
                break;
            case GameStates.GameOver:
            case GameStates.Exit:
                _isGameover = true;
                StopCoroutine(nameof(RandomSpawning));
                break;
        }
    }

    private IEnumerator RandomSpawning()
    {
        while (!_isGameover)
        {
            yield return Utility.GetWaitForSeconds(nextSpawnDelay);

            var missileClone = Spawn(spawnOrigin.position, Quaternion.identity);

            if (!missileClone.TryGetComponent(out _missile)) continue;

            _missile.Init(mp: CurrentLunchMissile);
        }
    }

    private void OnDestroy()
    {
        GameManager.OnCurrentState -= GameManager_OnCurrentState;
    }
}

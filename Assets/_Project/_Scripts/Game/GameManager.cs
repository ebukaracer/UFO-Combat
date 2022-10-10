using Racer.Utilities;
using System;
using UnityEngine;

/// <summary>
/// Game states available for transitioning.
/// </summary>
internal enum GameStates
{
    Loading,
    Playing,
    GameOver,
    Exit
}

/// <summary>
/// This manages the various states of the game.
/// </summary>
[DefaultExecutionOrder(-10)]
internal class GameManager : SingletonPattern.Singleton<GameManager>
{
    private Camera _cam;

    public static event Action<GameStates> OnCurrentState;

    // For Visualization purpose only
    [field: SerializeField]
    public GameStates CurrentState { get; private set; }

    [field: SerializeField, Header("Other dependencies"), Space(5)]
    public TagSet TagSet { get; private set; }


    private void Start()
    {
        _cam = Utility.CameraMain;

        SetGameState(GameStates.Loading);
    }


    /// <summary>
    /// Sets the current state of the game.
    /// </summary>
    /// <param name="state">Actual state to transition to</param>
    public void SetGameState(GameStates state)
    {
        CurrentState = state;

        // Updates other scripts listening to the game's current state
        OnCurrentState?.Invoke(state);
    }

    public void ShakeCam(float duration, float magnitude)
    {
        StartCoroutine(_cam.transform.ShakePosition(duration, magnitude));
    }

    public void SpawnPossiblePickupMissile(Vector2 pos)
    {
        var ms = MissileSelector.Instance;

        if (CanSpawnPickupMissile)
            TagSet.PickupMissileSpawner.SpawnMissilePickup(pos, ms.GetMissile(UnityEngine.Random.Range(1, ms.Length)));

        CanSpawnPickupMissile = false;
    }

    public bool CanSpawnPickupMissile { get; set; } = true;
}
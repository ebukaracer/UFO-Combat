using UnityEngine;

internal class UfoSpawner : Spawner
{
    private void Start() => GameManager.OnCurrentState += GameManager_OnCurrentState;

    private void GameManager_OnCurrentState(GameStates st)
    {
        if (st.Equals(GameStates.Playing))
            SpawnUfo();
    }

    public void SpawnUfo()
    {
        var clone = Spawn(new Vector2(0, 7), Quaternion.identity);

        if (clone.TryGetComponent(out Ufo ufo))
            ufo.Init();
    }

    private void OnDestroy()
    {
        GameManager.OnCurrentState -= GameManager_OnCurrentState;
    }
}

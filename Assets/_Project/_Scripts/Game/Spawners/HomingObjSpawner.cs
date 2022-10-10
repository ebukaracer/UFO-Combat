using UnityEngine;

internal class HomingObjSpawner : Spawner
{
    public void SpawnObj(Vector2 spawnOrigin)
    {
        if (Spawn(spawnOrigin).TryGetComponent(out HomingObject homingObject))
            homingObject.Init();
    }
}

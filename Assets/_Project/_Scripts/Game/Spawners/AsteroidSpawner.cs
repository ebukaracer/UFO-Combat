using UnityEngine;

internal class AsteroidSpawner : Spawner
{
    private Asteroid _asteroid;

    public void SpawnAsteroid(Vector2 spawnOrigin = default)
    {
        var clone = Spawn(spawnOrigin, Quaternion.identity);

        if (clone.TryGetComponent(out _asteroid))
        {
            _asteroid.Init();
        }
    }
}

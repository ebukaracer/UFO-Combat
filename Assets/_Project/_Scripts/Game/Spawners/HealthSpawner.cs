using UnityEngine;

internal class HealthSpawner : Spawner
{
    public void SpawnHealth(Vector2 spawnPos)
    {
        Spawn(spawnPos, RandomRotation());
    }

    public static Quaternion RandomRotation() => Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
}

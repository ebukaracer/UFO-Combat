using UnityEngine;

internal class PickupMissileSpawner : Spawner
{
    private PickupMissile _pickupMissile;

    public void SpawnMissilePickup(Vector2 pos, MissileProperty mp)
    {
        var pickupClone = Spawn(pos, HealthSpawner.RandomRotation());

        if (pickupClone.TryGetComponent(out _pickupMissile))
        {
            _pickupMissile.Init(mp.sprite, mp.missileType);
        }
    }
}

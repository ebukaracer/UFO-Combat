using Racer.ObjectPooler;
using UnityEngine;

internal class PickupMissile : PoolObject
{
    [field: SerializeField]
    public MissileType MissileType { get; private set; }

    [SerializeField] private SpriteRenderer sprRend;


    public void Init(Sprite spr, MissileType missileType)
    {
        sprRend.sprite = spr;

        MissileType = missileType;
    }
}

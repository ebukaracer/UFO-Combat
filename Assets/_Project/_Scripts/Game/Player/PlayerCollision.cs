using Racer.SoundManager;
using System;
using UnityEngine;

internal class PlayerCollision : MonoBehaviour
{
    private PickupMissile _pickupMissile;
    private FillBar _pickupFill;

    public event Action<Collider2D> OnCollisionCallback;

    [SerializeField, Header("REFERENCES")]
    private LunchMissileSpawner lunchMissileSpawner;

    [Space(5), SerializeField]
    private AudioClip sfxPickup;


    private void Start()
    {
        _pickupFill = UIControllerGame.Instance.PickupFill;

        _pickupFill.OnDecreaseFinished += PickupFill_OnDecreaseFinished;
    }

    // Collectibles
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var col = collision.gameObject;

        OnCollisionCallback?.Invoke(collision);

        if (!col.CompareTag(Tags.PickupMissile)) return;
        if (!col.TryGetComponent(out _pickupMissile)) return;

        GameManager.Instance.CanSpawnPickupMissile = false;

        var missileProperty = MissileSelector.Instance.GetMissile(_pickupMissile.MissileType);

        _pickupFill.ChangeSprite(missileProperty.sprite);

        _pickupFill.DecreaseFill();

        lunchMissileSpawner.CurrentLunchMissile = missileProperty;

        _pickupMissile.Despawn();

        SoundManager.Instance.PlaySfx(sfxPickup);
    }


    private void PickupFill_OnDecreaseFinished()
    {
        lunchMissileSpawner.CurrentLunchMissile = MissileSelector.Instance.GetMissile(MissileType.Default);

        GameManager.Instance.CanSpawnPickupMissile = true;
    }

    private void OnDestroy()
    {
        _pickupFill.OnDecreaseFinished -= PickupFill_OnDecreaseFinished;
    }
}

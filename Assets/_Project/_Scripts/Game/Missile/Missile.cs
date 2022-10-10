using Racer.ObjectPooler;
using UnityEngine;

internal class Missile : PoolObject
{
    private Rigidbody2D _rb2D;
    private SpriteRenderer _spr;

    public MissileType MissileType { get; private set; }
    public int DmgPerHit { get; private set; }

    [SerializeField] private float upthrust = 5f;


    private void Awake()
    {
        _spr = GetComponent<SpriteRenderer>();

        _rb2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _rb2D.velocity = Vector2.up * upthrust;
    }

    public void Init(MissileProperty mp)
    {
        _spr.sprite = mp.sprite;

        MissileType = mp.missileType;

        DmgPerHit = mp.dmgPerHit;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Bound) || collision.CompareTag(Tags.Asteroid) || collision.CompareTag(Tags.Ufo))
            Despawn();
    }

    public override void Despawn()
    {
        base.Despawn();

        _rb2D.velocity = Vector2.zero;
    }
}

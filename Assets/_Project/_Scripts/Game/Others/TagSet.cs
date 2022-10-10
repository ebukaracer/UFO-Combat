using UnityEngine;

/// <summary>
/// Serves as a reference container.
/// </summary>
internal class TagSet : MonoBehaviour
{
    [field: SerializeField,
    Header("REFERENCED SCENE SCRIPTS")]
    public PickupMissileSpawner PickupMissileSpawner { get; private set; }
    [field: SerializeField] public UfoSpawner UfoSpawner { get; private set; }
    [field: SerializeField] public HealthSpawner HealthSpawner { get; private set; }
    [field: SerializeField] public ColorGenerator ColorGenerator { get; private set; }


    [field: SerializeField,
    Space(5)]
    public Attacker Attacker { get; private set; }
    [field: SerializeField] public PlayerController Player { get; private set; }

    [field: SerializeField,
    Space(5),
    Header("REFERENCED SCENE PARTICLE-SYSTEMS")]
    public ParticleSystem UfoHurtFx { get; private set; }

}

internal class Tags
{
    public const string Player = "Player";
    public const string Asteroid = "Asteroid";
    public const string Bound = "Bound";
    public const string Ground = "Ground";
    public const string Missile = "Missile";
    public const string PickupMissile = "Pickup_Missile";
    public const string Health = "Health";
    public const string Ufo = "UFO";
    public const string HomingObj = "Homing_Obj";
}

internal class Metrics
{
    // Keys
    public const string EncryptId = "MW"; // alias of 'Max_Wave'
    public const string SaveId = "Max_Wave";
    public const string SpeedCount = "Speed_Count";
    public const string InputIndex = "Input_Index";
    public const string SoundIndex= "Sound_Index";
    public const string ShakeIndex = "Shake_Index";
    public const string Username = "Username";
    public const string HasClaimed = "Claimed_Title";


    // Game parameters
    public const int Offset = 5; // for encrypt/decrypt
    public const int MidDifficultyAmount = 15;
    public const int MaxAsteroidPresent = 3;
    public const int PickupHealthRange = 15;
    public const int ProGamerCount = 100;

    // Higher Value => higher delay before next spawn, and vice versa.
    public const float MaxSpawnTime = 2f;
    public const float MinSpawnTime = 1.5f;
}

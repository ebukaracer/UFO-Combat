using Racer.SoundManager;
using System.Collections.Generic;
using UnityEngine;

internal class Attacker : MonoBehaviour
{
    private float _timer;
    private float _nextSpawnDelay = Metrics.MaxSpawnTime;

    public Vector2 SpawnPosition { get; set; }
    public bool CanSpawnAsteroid { get; set; }
    public static List<GameObject> TotalPresentAttackers { get; } = new();

    [SerializeField] private AsteroidSpawner asteroidSpawner;
    [SerializeField] private HomingObjSpawner homingObjSpawner;

    [Space(5), SerializeField] private AudioClip sfxDeploy;


    private void Update()
    {
        if (CanSpawnAsteroid)
        {
            _timer += Time.deltaTime;

            if (!(_timer > _nextSpawnDelay)) return;

            asteroidSpawner.SpawnAsteroid(SpawnPosition);

            SoundManager.Instance.PlaySfx(sfxDeploy);

            _timer = 0;
        }
        else
        {
            _timer = _nextSpawnDelay;
        }
    }

    public void SpawnHomingObj()
    {
        homingObjSpawner.SpawnObj(SpawnPosition);

        _nextSpawnDelay = Random.Range(0, 2) == 0 ? Metrics.MaxSpawnTime : Metrics.MinSpawnTime;

        SoundManager.Instance.PlaySfx(sfxDeploy);
    }

    private void OnDestroy() => TotalPresentAttackers.Clear();
}

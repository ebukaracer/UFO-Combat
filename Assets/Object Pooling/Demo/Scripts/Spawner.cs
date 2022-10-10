using UnityEngine;
using Racer.ObjectPooler;

[RequireComponent(typeof(Pool))]
public abstract class Spawner : MonoBehaviour
{
    protected Pool Pool;

    protected PoolObject SpawnedObj;

    protected virtual void Awake() => Pool = GetComponent<Pool>();

    public virtual PoolObject Spawn(Vector3 pos, Quaternion rot)
    {
        return SpawnedObj = Pool.SpawnObject(
           pos,
           rot
        );
    }

    public virtual PoolObject Spawn(Vector3 pos)
    {
        return SpawnedObj = Pool.SpawnObject(pos);
    }
}

[RequireComponent(typeof(Pool))]
abstract class Spawner<T> : MonoBehaviour where T : PoolObject
{
    protected Pool Pool;

    protected T SpawnedObj;

    protected virtual void Awake() => Pool = GetComponent<Pool>();

    public virtual T Spawn(Vector3 pos, Quaternion rot)
    {
        return SpawnedObj = (T)Pool.SpawnObject(
            pos,
            rot
        );
    }

    public virtual T Spawn(Vector3 pos)
    {
        return SpawnedObj = (T)Pool.SpawnObject(pos);
    }
}




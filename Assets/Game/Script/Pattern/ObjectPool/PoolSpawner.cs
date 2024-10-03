using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolSpawner : Singleton<PoolSpawner>
{
    public GameObject Spawn(GameObject obj, Transform holder = null, bool worldSpace = false)
    {
        return PoolObject.Instance.Spawn(obj, holder, worldSpace);
    }

    public GameObject Spawn(GameObject obj, Vector2 pos)
    {
        return PoolObject.Instance.Spawn(obj, pos);
    }

    public ParticleSystem Spawn(ParticleSystem obj, Transform holder = null, bool worldSpace = false)
    {
        return PoolObject.Instance.Spawn(obj, holder, worldSpace);
    }

    public ParticleSystem Spawn(ParticleSystem obj, Vector2 pos)
    {
        return PoolObject.Instance.Spawn(obj, pos);
    }

    public void Despawn(GameObject obj)
    {
        PoolObject.Instance.Despawn(obj);
    }

    public void DespawnTypeComponent(ParticleSystem obj)
    {
        PoolObject.Instance.Despawn(obj);
    }

    public void DestroyObjectOfType(GameObject obj)
    {
        PoolObject.Instance.DestroyAllGameObject(obj);
    }

    public void DestroyAllObjectOfTypeComponent(ParticleSystem obj)
    {
        PoolObject.Instance.DestroyAllParticle(obj);
    }

    public void ClearAllPoolGameObject()
    {
        PoolObject.Instance.ClearAllPoolGameObject();
    }

    public void ClearAllPoolParticle()
    {
        PoolObject.Instance.ClearAllPoolParticle();
    }

    public void ClearAllPool()
    {
        PoolObject.Instance.ClearAllPoolGameObject();
        PoolObject.Instance.ClearAllPoolParticle();
    }
}
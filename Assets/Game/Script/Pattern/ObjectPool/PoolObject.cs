using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;

public class PoolObject : Singleton<PoolObject>
{
    
    private Dictionary<string, Queue<GameObject>> _poolGameObject = new Dictionary<string, Queue<GameObject>>();

    private Dictionary<string, Queue<ParticleSystem>> _poolParticleSystem =
        new Dictionary<string, Queue<ParticleSystem>>();
    

    public GameObject Spawn(GameObject obj, Transform holder, bool worldSpace = false)
    {
        var key = obj.name.Replace("(Clone)", "");
        if (_poolGameObject.ContainsKey(key))
        {
            if (_poolGameObject[key].Count > 0)
            {
                obj = _poolGameObject[key].Dequeue();
                obj.transform.position = holder.position;
                obj.transform.rotation = Quaternion.AngleAxis(0, Vector3.zero);
                obj.Show();
                return obj;
            }
            else
            {
                obj = CreateNewObject();
                _poolGameObject.TryAdd(key, new Queue<GameObject>());
            }
        }
        else
        {
            obj = CreateNewObject();
            _poolGameObject.TryAdd(key, new Queue<GameObject>());
        }

        return obj;

        GameObject CreateNewObject()
        {
            GameObject oSpawned;
            oSpawned = worldSpace
                ? Object.Instantiate(obj, holder.position, quaternion.identity)
                : Object.Instantiate(obj, holder);
            return oSpawned;
        }
    }

    public GameObject Spawn(GameObject obj, Vector3 pos = default)
    {
        var key = obj.name.Replace("(Clone)", "");
        if (_poolGameObject.ContainsKey(key))
        {
            if (_poolGameObject[key].Count > 0)
            {
                obj = _poolGameObject[key].Dequeue();
                obj.transform.position = pos;
                obj.transform.rotation = Quaternion.AngleAxis(0, Vector3.zero);
                obj.Show();
                return obj;
            }
            else
            {
                obj = CreateNewObject();
                _poolGameObject.TryAdd(key, new Queue<GameObject>());
            }
        }
        else
        {
            obj = CreateNewObject();
            _poolGameObject.TryAdd(key, new Queue<GameObject>());
        }

        return obj;

        GameObject CreateNewObject()
        {
            return Object.Instantiate(obj, pos, quaternion.identity);
        }
    }

    public ParticleSystem Spawn(ParticleSystem obj, Transform holder, bool worldSpace = false)
    {
        var key = obj.gameObject.name.Replace("(Clone)", "");
        if (_poolParticleSystem.ContainsKey(key))
        {
            if (_poolParticleSystem[key].Count > 0)
            {
                obj = _poolParticleSystem[key].Dequeue();

                obj.transform.position = holder.position;
                obj.transform.rotation = Quaternion.AngleAxis(0, Vector3.zero);
                obj.Show();
                return obj;
            }
            else
            {
                obj = CreateNewObject();
                _poolParticleSystem.TryAdd(key, new Queue<ParticleSystem>());
            }
        }
        else
        {
            obj = CreateNewObject();
            _poolParticleSystem.TryAdd(key, new Queue<ParticleSystem>());
        }

        return obj;

        ParticleSystem CreateNewObject()
        {
            ParticleSystem oSpawned;
            oSpawned = worldSpace
                ? Object.Instantiate(obj, holder.position, quaternion.identity)
                : Object.Instantiate(obj, holder);
            return oSpawned;
        }
    }

    public ParticleSystem Spawn(ParticleSystem obj, Vector3 pos = default)
    {
        var key = obj.gameObject.name.Replace("(Clone)", "");
        if (_poolParticleSystem.ContainsKey(key))
        {
            if (_poolParticleSystem[key].Count > 0)
            {
                obj = _poolParticleSystem[key].Dequeue();
                obj.transform.position = pos;
                obj.transform.rotation = Quaternion.AngleAxis(0, Vector3.zero);
                obj.Show();
                return obj;
            }
            else
            {
                obj = CreateNewObject();
                _poolParticleSystem.TryAdd(key, new Queue<ParticleSystem>());
            }
        }
        else
        {
            obj = CreateNewObject();
            _poolParticleSystem.TryAdd(key, new Queue<ParticleSystem>());
        }

        return obj;

        ParticleSystem CreateNewObject()
        {
            return Object.Instantiate(obj, pos, quaternion.identity);
        }
    }
    

    public void Despawn(GameObject obj)
    {
        var key = obj.name.Replace("(Clone)", "");
        if (_poolGameObject.ContainsKey(key))
        {
            obj.Hide();
            _poolGameObject[key].Enqueue(obj);
        }
        else
        {
            var queue = new Queue<GameObject>();
            obj.Hide();
            queue.Enqueue(obj);
            _poolGameObject.TryAdd(key, queue);
        }
    }

    public void Despawn(ParticleSystem obj)
    {
        var key = obj.gameObject.name.Replace("(Clone)", "");
        if (_poolParticleSystem.ContainsKey(key))
        {
            obj.Hide();
            _poolParticleSystem[key].Enqueue(obj);
        }
        else
        {
            var queue = new Queue<ParticleSystem>();
            obj.Hide();
            queue.Enqueue(obj);
            _poolParticleSystem.TryAdd(key, queue);
        }
    }

    public void DestroyAllGameObject(GameObject obj)
    {
        var key = obj.gameObject.name.Replace("(Clone)", "");
        if (_poolGameObject.ContainsKey(key))
        {
            _poolGameObject[key].Clear();
            _poolGameObject.Remove(key);
            Object.Destroy(obj);
        }
        else
        {
            Object.Destroy(obj);
        }
    }

    public void DestroyAllParticle(ParticleSystem obj)
    {
        var key = obj.gameObject.name.Replace("(Clone)", "");
        if (_poolParticleSystem.ContainsKey(key))
        {
            _poolParticleSystem[key].Clear();
            _poolParticleSystem.Remove(key);
            Object.Destroy(obj);
        }
        else
        {
            Object.Destroy(obj);
        }
    }

    public void ClearAllPoolGameObject()
    {
        _poolGameObject.Clear();
    }

    public void ClearAllPoolParticle()
    {
        _poolParticleSystem.Clear();
    }
}
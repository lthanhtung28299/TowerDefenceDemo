using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFinder : MonoSingleton<ObjectFinder>
{
    [SerializeField] private List<ObjectType> objectTypes = new List<ObjectType>();
    [SerializeField] public DataHolder dataHolder;

    public static Transform GetObject(ObjectID type)
    {
        foreach (var o in Instance.objectTypes)
        {
            if (o.type == type)
            {
                return o.transform;
            }
        }

        return null;
    }
}

[Serializable]
public class ObjectType
{
    public ObjectID type;
    public Transform transform;
}


public enum ObjectID
{
    World,
    PopupHolder,
    WorldSpawnerHolder
}
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataHolder", menuName = "Data/DataHolder", order = 0)]
public class DataHolder : ScriptableObject
{
    [SerializeField] private List<ScriptableObject> dataHolders;
    private readonly Dictionary<Type, ScriptableObject> _cached = new ();
    [NonSerialized]
    private bool _isInitialized = false;

    //Lấy tham chiếu dataHolder trên hierarchy từ Objectfinder
    public static DataHolder GetInstance()
    {
        return ObjectFinder.Instance.dataHolder;
    }

    public T GetData<T>() where T : class
    {
        Initialize();
        return _cached[typeof(T)] as T;
    }

    private void Initialize()
    {
        if (!_isInitialized)
        {
            _isInitialized = true;
            foreach (var data in dataHolders)
            {
                _cached.Add(data.GetType(), data);
            }
        }
    }
}
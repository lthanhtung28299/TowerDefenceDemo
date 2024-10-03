using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T>: MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private bool isDontDestroyOnLoad;
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance) return instance;
            instance = FindObjectOfType<T>();
            if (instance) return instance;
            instance = new GameObject(typeof(T).Name).AddComponent<T>();
            return instance;
        }
    }

    public static bool IsInstance()
    {
        return instance != null;
    }
    
    protected virtual void Awake()
    {
        if (isDontDestroyOnLoad)
        {
            DontDestroyOnLoad(Instance.gameObject);    
        }
    }
}
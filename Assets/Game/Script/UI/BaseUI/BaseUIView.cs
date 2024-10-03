using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUIView : MonoBehaviour, IViewUI, IAnimation
{
    public UITransitionAnimation Anim { get; set; }

    protected virtual void Awake()
    {
        Anim = GetComponent<UITransitionAnimation>();
    }

    public void Open(Action onComplete = null)
    {
        gameObject.Show();
        onComplete?.Invoke();
        if (Anim != null)
        {
            Anim.OnStart();
        }
    }

    public void Close(Action onComplete = null)
    {
        if (Anim != null)
        {
            Anim.OnReverse(() =>
            {
                onComplete?.Invoke();
                PoolSpawner.Instance.Despawn(gameObject);
                // gameObject.Hide();
            });
        }
        else
        {
            onComplete?.Invoke();
            // gameObject.Hide();
            PoolSpawner.Instance.Despawn(gameObject);
        }
    }

    public virtual void OnOpen(Action onComplete = null)
    {
        Open(onComplete);
    }

    public virtual void OnClose(Action onComplete = null)
    {
        Close(onComplete);
    }
}
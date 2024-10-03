using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

public class BaseUIPopup : BaseUIView
{
    public int priority = PopupData.DEFAULT_PRIORITY;
    public bool IsOnOpenPopup { get; private set; }

    public override void OnOpen(Action onComplete = null)
    {
        base.OnOpen(onComplete);
        transform.SetAsLastSibling();
        IsOnOpenPopup = true;
    }

    public override void OnClose(Action onComplete = null)
    {
        IsOnOpenPopup = false;
        base.OnClose(onComplete);
    }
}

public class UIPopup<T> : BaseUIPopup where T : UIPopup<T>
{
    public static bool IsOpen { get; private set; }

    public override void OnClose(Action onComplete = null)
    {
        base.OnClose(() =>
        {
            onComplete?.Invoke();
            PoolSpawner.Instance.Despawn(gameObject);
            IsOpen = false;
        });
    }

    public static void OpenPopup(Action<T> onOpen = null)
    {
        PopupManager.OpenPopup(onOpen);
        IsOpen = true;
    }

    public static void ClosePopup(Action<T> onClose = null)
    {
        PopupManager.ClosePopup(onClose);
        IsOpen = false;
    }
}
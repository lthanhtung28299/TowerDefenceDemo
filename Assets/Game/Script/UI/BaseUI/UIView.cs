using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class UIView : BaseUIView
{
    public static readonly Dictionary<Type, UIView> screenViews = new Dictionary<Type, UIView>();
    public static UIView CurrentScreen { get; private set; }
    protected List<UIView> uiViews = new List<UIView>();

    protected override void Awake()
    {
        base.Awake();
        uiViews.Clear();
    }

    public override void OnOpen(Action onComplete = null)
    {
        base.OnOpen(onComplete);
        CurrentScreen = this;
    }

    public override void OnClose(Action onComplete = null)
    {
        CurrentScreen = null;
        base.OnClose(onComplete);
    }

    public virtual void BackScreen()
    {
    }
}

public abstract class UIView<T> : UIView where T : UIView
{
    public static T Instance
    {
        get
        {
            if (screenViews.ContainsKey(typeof(T)))
            {
                var ins = screenViews[typeof(T)];
                if (ins != null) return ins as T;
                screenViews.Remove(typeof(T));
                ins = ScreenManager.Instance.GetView<T>();
                screenViews.Add(typeof(T),ins);
                return ins as T;
            }
            else
            {
                var screen = ScreenManager.Instance.GetView<T>();
                screenViews.Add(typeof(T), screen);
                return screen as T;
            }
        }
    }

    public static void OpenScreen(Action<T> onComplete = null)
    {
        OpenView(onComplete);
    }

    private static void OpenView(Action<T> onComplete = null)
    {
        if (CurrentScreen is T)
        {
            return;
        }
        else
        {
            if (CurrentScreen)
            {
                CurrentScreen.OnClose(() => { Instance.OnOpen(() => onComplete?.Invoke(Instance)); });
            }
            else
            {
                Instance.OnOpen(() => { onComplete?.Invoke(Instance); });
            }
        }
    }

    public static bool IsOnScreen()
    {
        return CurrentScreen is T;
    }
}
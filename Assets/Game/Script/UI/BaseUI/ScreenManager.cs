using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoSingleton<ScreenManager>
{
    [SerializeField] private List<UIView> screens = new List<UIView>();
    [SerializeField] private GameObject bg;

    public void ShowHideBackGround(bool isShow)
    {
        bg.SetActive(isShow);
    }
    
    public UIView GetView<T>() where T : UIView
    {
        foreach (var view in screens)
        {
            if (view.GetType() == typeof(T))
            {
                return view;
            }
        }
        return null;
    }
}
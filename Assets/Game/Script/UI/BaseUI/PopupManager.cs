using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoSingleton<PopupManager>
{
    private static Dictionary<string, BaseUIPopup> Prefabs = new Dictionary<string, BaseUIPopup>();
    private static List<BaseUIPopup> _cachePopup = new List<BaseUIPopup>();

    public static BaseUIPopup GetPopupData(string popupName)
    {
        return _cachePopup.Find(s => s.name == popupName);
    }

    public static void OpenPopup<T>(Action<T> openComplete) where T : UIPopup<T>
    {
        var popupHolder = ObjectFinder.GetObject(ObjectID.PopupHolder);
        var popup = popupHolder.Find(typeof(T).Name)?.GetComponent<T>();
        if (popup == null)
        {
            popup = DataHolder.GetInstance().GetData<PopupData>().GetPopupInfo(typeof(T).Name).popupPrefab as T;
            OpenPopup(typeof(T).Name, () => OnPoolingPopup(popup, openComplete));
        }
        else
        {
            OpenPopup(typeof(T).Name, () => OnPoolingPopup(popup, openComplete));
        }
    }

    private static void OnPoolingPopup<T>(T popup, Action<T> openComplete) where T : UIPopup<T>
    {
        var popupHolder = ObjectFinder.GetObject(ObjectID.PopupHolder);
        var popupPool = PoolSpawner.Instance.Spawn(popup.gameObject,popupHolder);
        var p = popupPool.GetComponent<BaseUIView>();
        p.OnOpen();
        openComplete?.Invoke(p as T);
    }

    private static void OpenPopup(string popupName, Action onPooling)
    {
        var priority = DataHolder.GetInstance().GetData<PopupData>().GetPriority(popupName);
        if (!Prefabs.ContainsKey(popupName))
        {
            foreach (var popup in _cachePopup)
            {
                if (!popup.gameObject.activeInHierarchy) continue;
                if (popup.priority <= priority)
                {
                    popup.OnClose();
                }
            }

            var popupPrefab = DataHolder.GetInstance().GetData<PopupData>().GetPopupInfo(popupName).popupPrefab;
            Prefabs.Add(popupName, popupPrefab);
            onPooling?.Invoke();
        }
        else
        {
            foreach (var popup in _cachePopup)
            {
                if (!popup.gameObject.activeSelf) continue;
                if (popup.name == popupName) continue;
                if (popup.priority <= priority)
                {
                    popup.OnClose();
                }
            }

            onPooling?.Invoke();
        }
    }

    public static void ClosePopup<T>(Action<T> onClose) where T : UIPopup<T>
    {
        var popup = _cachePopup.Find(s => s.name == typeof(T).Name) as T;
        if (!popup) return;
        if (popup.gameObject.activeInHierarchy)
        {
            popup.OnClose(() => onClose?.Invoke(popup));
        }
    }

    public static void CloseAllPopup()
    {
        foreach (var popup in _cachePopup)
        {
            popup.OnClose();
        }
    }
}
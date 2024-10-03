using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "PopupPriorityData", menuName = "Data/Popup", order = 0)]
public class PopupData : ScriptableObject
{
    public const int DEFAULT_PRIORITY = 10;
    public List<PopupInfo> popupInfos = new List<PopupInfo>();

    public int GetPriority(string viewId)
    {
        foreach (var layer in popupInfos.Where(layer => layer.namePopup == viewId))
        {
            return layer.priority;
        }

        return DEFAULT_PRIORITY;
    }

    public PopupInfo GetPopupInfo(string popupType)
    {
        return popupInfos.Find(s => s.namePopup == popupType);
    }
    
}

[Serializable]
public class PopupInfo
{
    public string namePopup;
    public int priority = 10;
    public BaseUIPopup popupPrefab;

    public PopupInfo(BaseUIPopup popup)
    {
        namePopup = popup.name;
        priority = popup.priority;
        popupPrefab = popup;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMapModel : Singleton<GridMapModel>
{
    public List<GridMapInfo> MapInfos { get; private set; }= new List<GridMapInfo>();

    public void Initialize()
    {
        var gridMaps = GridMapSo.GetInstance().MapInfos;
        MapInfos.Clear();
        foreach (var info in gridMaps)
        {
            MapInfos.Add(new GridMapInfo(info));
        }
    }
}

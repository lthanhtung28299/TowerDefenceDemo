using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerModel : Singleton<TowerModel>
{
    public List<TowerDataInfo> TowerDataInfos { get; private set; } = new List<TowerDataInfo>();

    public void Initialize()
    {
        var towerDataInfos = TowerDataSo.GetInstance().TowerDataInfos;
        TowerDataInfos.Clear();
        foreach (var towerDataInfo in towerDataInfos)
        {
            TowerDataInfos.Add(new TowerDataInfo(towerDataInfo));
        }
    }
}
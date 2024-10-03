using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "TowerDataSo", menuName = "Data/TowerDataSo")]
public class TowerDataSo : ScriptableObject
{
    [field: SerializeField] public List<TowerDataInfo> TowerDataInfos { get; private set; } = new List<TowerDataInfo>();

    public static TowerDataSo GetInstance()
    {
        return DataHolder.GetInstance().GetData<TowerDataSo>();
    }
}

[Serializable]
public class TowerDataInfo
{
    public string towerName;

    [PreviewField(75f, ObjectFieldAlignment.Right)]
    public Sprite towerIcon;

    public ProjectileType projectileType;
    public int cost;
    public int damage;
    public float range;
    [ShowIf("IsProjectileExplosion")] public float radiusExplosion;
    public float fireRate;
    public float buildDelay;
    public BaseTower tower;

    private bool IsProjectileExplosion => projectileType == ProjectileType.Explosion;

    public TowerDataInfo(TowerDataInfo towerDataInfo)
    {
        towerName = towerDataInfo.towerName;
        towerIcon = towerDataInfo.towerIcon;
        cost = towerDataInfo.cost;
        damage = towerDataInfo.damage;
        range = towerDataInfo.range;
        radiusExplosion = towerDataInfo.radiusExplosion;
        fireRate = towerDataInfo.fireRate;
        buildDelay = towerDataInfo.buildDelay;
        tower = towerDataInfo.tower;
    }
}

public enum ProjectileType
{
    Normal,
    Explosion
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectBuildTowerViewItem : MonoBehaviour
{
    [SerializeField] private Button btnSellTower;
    [SerializeField] private SelectBuildTowerItemButton[] buttonTower;
    [SerializeField] private RectTransform selectRectTransform;
    private List<TowerDataInfo> towerDataList = new List<TowerDataInfo>();
    
    private TileNode _cacheNode;
    private Vector2Int _buildPos;
    private void Start()
    {
        btnSellTower.onClick.AddListener(OnSell);
    }
    
    public void OnShow(Vector2 pos,Vector2Int buildPos)
    {
        this.Hide();
        var tileNode = GridManager.Instance.GetTileNode(buildPos);
        if (tileNode == null) return;
        if(!tileNode.Node.canBuild) return;
        _cacheNode = tileNode;
        this.Show();
        SetDefaults();
        
        selectRectTransform.anchoredPosition = pos;
        towerDataList = TowerModel.Instance.TowerDataInfos;
        if (tileNode.Node.hasBeenBuilt)
        {
            btnSellTower.Show();
        }
        else
        {
            for (int i = 0; i < buttonTower.Length && i < towerDataList.Count; i++)
            {
                buttonTower[i].OnShow(towerDataList[i],buildPos,() => this.Hide());
            }    
        }
        
    }

    private void OnSell()
    {
        TowerSpawner.Instance.DespawnTower(_buildPos);
        if(_cacheNode != null)_cacheNode.SetBuiltNode(false);
        this.Hide();
    }

    private void SetDefaults()
    {
        foreach (var button in buttonTower)
        {
            button.Hide();
        }
        btnSellTower.Hide();
    }
}

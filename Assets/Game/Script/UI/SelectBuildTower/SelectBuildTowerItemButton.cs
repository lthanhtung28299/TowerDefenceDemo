using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectBuildTowerItemButton : MonoBehaviour
{
    [SerializeField] private Button btnChoose;
    [SerializeField] private Image icTower;
    [SerializeField] private TMP_Text txtGold;
    private Vector2Int _buildPos;
    private TowerDataInfo _info;
    private Action _onBuildComplete;
    private void Start()
    {
        btnChoose.onClick.AddListener(OnClick);
    }

    public void OnShow(TowerDataInfo info, Vector2Int buildPos,Action onBuildComplete)
    {
        this.Show();
        _info = info;
        _buildPos = buildPos;
        _onBuildComplete = onBuildComplete;
        icTower.sprite = info.towerIcon;
        icTower.SetNativeSize();
        txtGold.text = info.cost.ToString();
    }

    private void OnClick()
    {
        if(_info == null || GamePlayModel.Instance.GoldRemain < _info.cost) return;
        GamePlayModel.Instance.DecreaseGold(_info.cost);
        TowerSpawner.Instance.SpawnTower(_buildPos,_info);
        _onBuildComplete?.Invoke();
    }
}
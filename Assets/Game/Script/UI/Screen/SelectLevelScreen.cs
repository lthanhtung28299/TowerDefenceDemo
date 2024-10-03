using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelScreen : UIView<SelectLevelScreen>
{
    [SerializeField] private SelectLevelItem selectLevelItem;
    [SerializeField] private RectTransform selectLevelHolder;
    [SerializeField] private Button btnBack;
    
    private List<SelectLevelItem> selectLevelItems = new List<SelectLevelItem>();

    private void Start()
    {
        btnBack.onClick.AddListener(BackToHomeScreen);
    }

    public void OnShow()
    {
        var listLevel = GameLevelModel.Instance.levels;
        SetDefaults();
        SpawnListLevel(listLevel);
    }

    private void SpawnListLevel(List<GameLevelInfo> levels)
    {
        foreach (var levelInfo in levels)
        {
            var itemSelectGo = PoolSpawner.Instance.Spawn(selectLevelItem.gameObject, selectLevelHolder);
            var itemSelect = itemSelectGo.GetComponent<SelectLevelItem>();
            itemSelect.OnShow(levelInfo);
            selectLevelItems.Add(itemSelect);
        }
    }

    private void SetDefaults()
    {
        foreach (var item in selectLevelItems)
        {
            PoolSpawner.Instance.Despawn(item.gameObject);
        }
        selectLevelItems.Clear();
    }

    private void BackToHomeScreen()
    {
        HomeScreen.OpenScreen();
    }
}
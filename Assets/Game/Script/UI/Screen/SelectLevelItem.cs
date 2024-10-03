using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelItem : MonoBehaviour
{
    [SerializeField] private Button btnSelect;
    [SerializeField] private TMP_Text txtLevel;

    private GameLevelInfo _info;
    private void Start()
    {
        btnSelect.onClick.AddListener(OnSelect);
    }

    public void OnShow(GameLevelInfo levelInfo)
    {
        _info = levelInfo;
        txtLevel.text = _info.levelId;
    }

    private void OnSelect()
    {
        if(_info == null) return;
        GameSceneManager.Instance.LoadSceneGamePlay(_info, () =>
        {
            GamePlayScreen.OpenScreen(s => s.OnShow());
        });
    }
}
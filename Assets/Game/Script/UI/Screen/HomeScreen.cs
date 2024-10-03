using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : UIView<HomeScreen>
{
    [SerializeField] private Button btnStart;

    private void Start()
    {
        btnStart.onClick.AddListener(StartGame);
    }
    
    private void StartGame()
    {
        SelectLevelScreen.OpenScreen(s => s.OnShow());
    }
}

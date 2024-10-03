using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGamePopup : UIPopup<PauseGamePopup>
{
    [SerializeField] private Button btnHome, btnContinues;

    private void Start()
    {
        btnHome.onClick.AddListener(OnBackHome);
        btnContinues.onClick.AddListener(() => OnClose());
    }
    
    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    public void OnShow()
    {
        Time.timeScale = 0;
    }

    private void OnBackHome()
    {
        GameSceneManager.Instance.BackToMainMenu();
        OnClose();
    }
}

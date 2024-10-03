using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class GameInputHandle : MonoSingleton<GameInputHandle>
{
    [SerializeField] private ScreenTouchInput touchInput;
    [SerializeField] private SelectBuildTowerViewItem selectTowerViewItem;

    private void OnEnable()
    {
        touchInput.PointerDownPosition += HandlePointerDown;
        touchInput.OnEndDragAction += HandleEndDragAction;
    }

    private void OnDisable()
    {
        touchInput.PointerDownPosition -= HandlePointerDown;
        touchInput.OnEndDragAction -= HandleEndDragAction;
    }

    private void Update()
    {
        HandleDragMoveScreen();
    }

    private void HandleDragMoveScreen()
    {
        if (touchInput.IsDrag)
        {
            MovementCamera.Instance.vcam.transform.position -= (Vector3)touchInput.DragPosition  * Time.deltaTime;
            selectTowerViewItem.Hide();
        }
    }
    
    private void HandleEndDragAction()
    {
        MovementCamera.Instance.vcam.transform.position = MovementCamera.Instance.MainCamera.transform.position;
    }

    private void HandlePointerDown(Vector2 position)
    {
        if(touchInput.IsDrag) return;
        var worldPosition = MovementCamera.Instance.MainCamera.ScreenToWorldPoint(position);
        var screenSize = new Vector2(Screen.width/2, Screen.height/2);
        selectTowerViewItem.OnShow(position - screenSize,Vector2Int.RoundToInt(worldPosition));
    }
}

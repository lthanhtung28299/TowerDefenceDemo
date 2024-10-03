using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraScreenDrag : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [field: SerializeField] public float TouchSensitivity_x { get; private set; } = 10f;
    [field: SerializeField] public float TouchSensitivity_y { get; private set; } = 10f;
    private Vector2 _deltaPos;
    private bool _isPress;
    
    
    void Start()
    {
        CinemachineCore.GetInputAxis = HandleAxisInputDelegate;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        _deltaPos = eventData.delta;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _isPress = true;
    }
    
    
    public void OnPointerUp(PointerEventData eventData)
    {
        _deltaPos = Vector2.zero;
        _isPress = false;
    }

    public void DragScreen(Vector2 pos)
    {
        _isPress = true;
        _deltaPos = pos;
    }

    public void EndDragScreen()
    {
        _isPress = false;
        _deltaPos = Vector2.zero;
    }
    
    public float HandleAxisInputDelegate(string axisName)
    {
        
        if (_isPress)
        {
            switch (axisName)
            {
                case "Mouse X":
    
                    if (Input.touchCount > 0)
                    {
                        return (_deltaPos.x / TouchSensitivity_x);
                    }
                    else
                    {
                        return 0;
                    }
    
                case "Mouse Y":
                    if (Input.touchCount > 0)
                    {
                        return (_deltaPos.y / TouchSensitivity_y);
                    }
                    else
                    {
                        return 0;
                    }
    
                default:
                    Debug.LogError("Input <" + axisName + "> not recognyzed.", this);
                    break;
            }
    
            return 0f;
        }
        else
            return 0;
    }
}

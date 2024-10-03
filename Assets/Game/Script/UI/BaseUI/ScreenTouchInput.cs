using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenTouchInput : MonoSingleton<ScreenTouchInput>, IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float touchSensitivityX = 10f;
    [SerializeField] private float touchSensitivityY = 10f;
    public event Action<Vector2> PointerDownPosition;
    public event Action OnEndDragAction;
    public Vector2 _pointerDownPosition;
    private Vector2 _dragPointerDownPos;
    private Vector2 _dragPosition;

    public bool IsDrag { get; private set; }

    public Vector2 DragPosition
    {
        get => _dragPosition;
        private set => _dragPosition = value;
    }

    void Start()
    {
        CinemachineCore.GetInputAxis = HandleAxisInputDelegate;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!eventData.dragging) return;
        DragPosition = eventData.delta;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragPointerDownPos = eventData.position;
        IsDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        IsDrag = false;
        DragPosition = Vector2.zero;
        OnEndDragAction?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _pointerDownPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_pointerDownPosition == eventData.position) PointerDownPosition?.Invoke(_pointerDownPosition);
        _pointerDownPosition = Vector2.zero;
    }

    private float HandleAxisInputDelegate(string axisName)
    {
        if (IsDrag)
        {
            switch (axisName)
            {
                case "Mouse X":

                    if (_dragPosition.x != 0)
                    {
                        return _dragPosition.x / touchSensitivityX;
                    }
                    else
                    {
                        return 0;
                    }

                case "Mouse Y":
                    if (_dragPosition.y != 0)
                    {
                        return _dragPosition.y / touchSensitivityY;
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
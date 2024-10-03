using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UITransitionAnimation : MonoBehaviour
{
    [SerializeField] private TransitionType transitionType = TransitionType.FadeMove;
    [SerializeField] private TransitionScreenPosition startPos = TransitionScreenPosition.Bottom;
    [SerializeField] private Ease transitionEasing = Ease.OutQuart;
    [SerializeField] private RectTransform panelHolder;
    [SerializeField] private float duration = 0.35f;
    private Sequence _tw;
    private Vector2 _centerScreen;
    private Vector2 _leftBoundScreen;
    private Vector2 _rightBoundScreen;
    private Vector2 _topBoundScreen;
    private Vector2 _bottomBoundScreen;

    private void Awake()
    {
        CalculateScreenPos();
    }

    private void OnDisable()
    {
        _tw?.Kill();
    }

    private void CalculateScreenPos()
    {
        // _centerScreen = new Vector2(Screen.width / 2, Screen.height / 2);
        _centerScreen = Vector2.zero;
        // _leftBoundScreen = new Vector2(-Screen.width / 2, Screen.height / 2);
        _leftBoundScreen = Vector2.left * Screen.width;
        // _rightBoundScreen = new Vector2(Screen.width + Screen.width / 2, Screen.height / 2);
        _rightBoundScreen = Vector2.right * Screen.width;
        // _bottomBoundScreen = new Vector2(Screen.width / 2, -Screen.height / 2);
        _bottomBoundScreen = Vector2.down * Screen.height;
        // _topBoundScreen = new Vector2(Screen.width / 2, Screen.height + Screen.height / 2);
        _topBoundScreen = Vector2.up * Screen.height;
    }
    
    public void OnStart(Action onComplete = null)
    {
        OnPlayTransition(isReverse: false, onComplete);
    }

    public void OnReverse(Action onComplete = null)
    {
        OnPlayTransition(isReverse: true, onComplete);
    }

    private void OnPlayTransition(bool isReverse, Action onComplete)
    {
        switch (transitionType)
        {
            case TransitionType.Move:
                OnMove(isReverse, onComplete);
                break;
            case TransitionType.FadeMove:
                OnFadeMove(isReverse, onComplete);
                break;
            case TransitionType.Scale:
                OnScale(isReverse, onComplete);
                break;
        }
    }

    private void OnMove(bool isReverse, Action onComplete = null)
    {
        _tw?.Kill();
        _tw = DOTween.Sequence()
            .Prepend(panelHolder.DOAnchorPos(isReverse ? _centerScreen : GetStartPos(), 0f))
            .Append(panelHolder.DOAnchorPos(isReverse ? GetStartPos() : _centerScreen, duration)
                .SetEase(transitionEasing))
            .SetUpdate(true)
            .OnComplete(() => { onComplete?.Invoke(); });
    }

    private void OnFadeMove(bool isReverse, Action onComplete)
    {
        _tw?.Kill();
        _tw = DOTween.Sequence()
            .Prepend(panelHolder.DOAnchorPos(isReverse ? _centerScreen : GetStartPos(), 0.1f))
            .Append(panelHolder.DOAnchorPos(isReverse ? GetStartPos() : _centerScreen, duration)
                .SetEase(transitionEasing))
            .Join(panelHolder.GetComponent<CanvasGroup>()?.DOFade(isReverse ? 0f : 1f, duration)
                .SetEase(transitionEasing))
            .SetUpdate(true)
            .OnComplete(() => { onComplete?.Invoke(); });
    }

    private void OnScale(bool isReverse, Action onComplete)
    {
        _tw?.Kill();
        _tw = DOTween.Sequence()
            .Prepend(panelHolder.DOAnchorPos(_centerScreen, 0))
            .Join(panelHolder.DOScale(isReverse ? Vector3.one : Vector3.zero, 0))
            .Append(panelHolder.DOScale(isReverse ? Vector3.zero : Vector3.one, duration)
                .SetEase(isReverse ? Ease.OutQuart : transitionEasing))
            .SetUpdate(true)
            .OnComplete(() => { onComplete?.Invoke(); });
    }

    private Vector2 GetStartPos()
    {
        var pos = startPos switch
        {
            TransitionScreenPosition.Top => _topBoundScreen,
            TransitionScreenPosition.Right => _rightBoundScreen,
            TransitionScreenPosition.Bottom => _bottomBoundScreen,
            TransitionScreenPosition.Left => _leftBoundScreen,
            TransitionScreenPosition.Center => _centerScreen,
            _ => Vector2.zero
        };
        return pos;
    }
}

public enum TransitionType
{
    Move,
    FadeMove,
    Scale
}

public enum TransitionScreenPosition
{
    Top,
    Right,
    Bottom,
    Left,
    Center
}
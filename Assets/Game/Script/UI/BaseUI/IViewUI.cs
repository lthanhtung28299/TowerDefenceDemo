using System;

public interface IViewUI
{
    void Open(Action onComplete = null);
    void Close(Action onComplete = null);
}
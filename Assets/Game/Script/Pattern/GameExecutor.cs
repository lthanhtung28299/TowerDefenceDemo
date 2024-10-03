using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExecutor : MonoSingleton<GameExecutor>
{
    public void WaitNewFrame(Action onComplete)
    {
        StartCoroutine(IEWaitNewFrame(onComplete));
    }

    private IEnumerator IEWaitNewFrame(Action onComplete)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        onComplete.Invoke();
    }

    public void WaitUtil(Func<bool> predicate,Action onComplete)
    {
        StartCoroutine(IEWaitUtil(predicate, onComplete));
    }

    private IEnumerator IEWaitUtil(Func<bool> predicate, Action onComplete)
    {
        yield return new WaitUntil(predicate);
        onComplete.Invoke();
    }

    public void WaitForSecond(float time, Action onComplete)
    {
        StartCoroutine(IEWaitForSecond(time,onComplete));
    }
    
    private IEnumerator IEWaitForSecond(float time,Action onComplete)
    {
        yield return new WaitForSeconds(time);
        onComplete.Invoke();
    }
}
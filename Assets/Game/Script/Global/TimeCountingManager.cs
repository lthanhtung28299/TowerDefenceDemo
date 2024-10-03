using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TimeCountingManager : Singleton<TimeCountingManager>
{
    private List<Timer> _timers = new List<Timer>();
    private float TimeStep => 0.2f;

    public void AddTimer(Timer timer)
    {
        timer.timerCoroutine = GameExecutor.Instance.StartCoroutine(IECountingTimer(timer));
        _timers.Add(timer);
    }

    private IEnumerator IECountingTimer(Timer timer)
    {
        float second = 0;
        timer.onStart?.Invoke(timer);
        if (timer.isCountDown)
        {
            while (timer.time > 0 && timer.isRunning)
            {
                yield return new WaitForSecondsRealtime(TimeStep);
                if(Time.timeScale == 0) continue;
                timer.time -= TimeStep;
                second += TimeStep;
                timer.everyStepAction?.Invoke(timer);
                if (second - 1f <= 0) continue;
                timer.everySecondAction?.Invoke(timer);
                second = 0;
            }
        }
        else
        {
            while (timer.isRunning)
            {
                yield return new WaitForSecondsRealtime(TimeStep);
                if(Time.timeScale == 0) continue;
                timer.time += TimeStep;
                second += TimeStep;
                timer.everyStepAction?.Invoke(timer);
                if (second - 1f <= 0) continue;
                timer.everySecondAction?.Invoke(timer);
                second = 0;
            }
        }

        RemoveTimer(timer);
    }

    public void RemoveTimer(Timer timer, bool ignoreCompleteAction = false)
    {
        timer.isRunning = false;
        if (!ignoreCompleteAction)
        {
            timer.onComplete?.Invoke(timer);
        }

        if (timer.timerCoroutine != null)
        {
            GameExecutor.Instance.StopCoroutine(timer.timerCoroutine);
        }
        _timers.Remove(timer);
    }
}

public class Timer
{
    public float time;
    public bool isRunning;
    public bool isCountDown;
    public Action<Timer> onStart;
    public Action<Timer> onComplete;
    public Action<Timer> everyStepAction;
    public Action<Timer> everySecondAction;
    public Coroutine timerCoroutine;

    public Timer()
    {
    }

    public Timer(float time, bool isCountDown)
    {
        this.time = time;
        this.isCountDown = isCountDown;
    }

    public void StartTimer()
    {
        isRunning = true;
        TimeCountingManager.Instance.AddTimer(this);
    }

    public void StopTimer(bool ignoreCompleteAction = false)
    {
        TimeCountingManager.Instance.RemoveTimer(this, ignoreCompleteAction);
    }

    public Timer OnStart(Action<Timer> action)
    {
        onStart = action;
        return this;
    }

    public Timer OnComplete(Action<Timer> action)
    {
        onComplete = action;
        return this;
    }

    public Timer AddActionEveryStep(Action<Timer> action)
    {
        everyStepAction = action;
        return this;
    }

    public Timer AddActionEverySecond(Action<Timer> action)
    {
        everySecondAction = action;
        return this;
    }
}
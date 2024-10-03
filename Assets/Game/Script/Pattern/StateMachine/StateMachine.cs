using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State _currentState;
    
    
    public void SwitchState(State newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }

    protected virtual void Update()
    {
        _currentState?.Tick(Time.deltaTime);
    }

    protected virtual void FixedUpdate()
    {
        _currentState?.FixUpdateTick(Time.fixedDeltaTime);
    }
}
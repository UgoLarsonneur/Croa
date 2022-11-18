using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    protected T _owner;
    protected State<T> _currentState;

    public State<T> CurrentState {get => _currentState; set => setState(value);}
    public T Owner => _owner;

    public StateMachine(T owner)
    {
        _owner = owner;
    }

    public virtual void update() {
        if(_currentState == null)
            Debug.LogError("No state to execute");
        else
            _currentState?.update();
    }

    public virtual void fixedUpdate() {
        _currentState?.fixedUpdate();
    }

    public void setState(State<T> newState)
    {
        _currentState?.exit();

        _currentState = newState;
        newState?.enter();
    }
}

public class State<T>
{
    protected StateMachine<T> _stateMachine;
    protected T Owner {get => _stateMachine.Owner;}

    public State(StateMachine<T> stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public virtual void enter(){}
    public virtual void exit(){}
    public virtual void update(){}
    public virtual void fixedUpdate(){}

    public override string ToString()
    {
        return this.GetType().Name;
    }
}

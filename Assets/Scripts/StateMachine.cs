using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    protected T _owner;
    public T Owner {get => _owner;}

    protected State<T> currentState;
    public State<T> CurrentState {get => currentState; set => setState(value);}

    public StateMachine(T owner)
    {
        _owner = owner;
    }

    public virtual void update() {
        if(currentState == null)
            Debug.LogError("No state to execute");
        else
            currentState.update();
    }

    public virtual void fixedUpdate() {
        currentState.fixedUpdate();
    }

    public void setState(State<T> newState)
    {
        currentState?.exit();

        currentState = newState;
        newState.enter();
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

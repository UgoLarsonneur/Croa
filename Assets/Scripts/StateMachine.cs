using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StateMachine<T>
{
    protected State<T> _currentState;
    public State<T> CurrentState {
        get {
            return _currentState;
        }
        set {
            _currentState?.Exit();

            _currentState = value;
            _currentState?.Enter();
        }
    }

    public virtual T Owner {get;}

    public StateMachine(T owner)
    {
        Owner = owner;
    }

    public virtual void Update() {
    if(_currentState == null)
        Debug.LogError("No state to execute");
    else
        _currentState?.Update();
    }

    public virtual void FixedUpdate() {
        _currentState?.FixedUpdate();
    }
}



/// <summary>
/// 
/// </summary>
/// <typeparam name="T">Base object using the states</typeparam>
/// <typeparam name="B">Type of the state executing the states</typeparam>
public class SubStateMachine<T, B> : StateMachine<T> where B : State<T>
{
    protected B _baseStateMachine;
    public B BaseSubMachine => _baseStateMachine;

    public SubStateMachine(B baseStateMachine) : base(baseStateMachine.Owner)
    {
        _baseStateMachine = baseStateMachine;
    }
}



public class State<T>
{
    public StateMachine<T> StateMachine {get; protected set;}
    public virtual T Owner {get => StateMachine.Owner;}

    public State(StateMachine<T> stateMachine)
    {
        StateMachine = stateMachine;
    }

    public virtual void Enter(){}
    public virtual void Exit(){}
    public virtual void Update(){}
    public virtual void FixedUpdate(){}

    public override string ToString()
    {
        return this.GetType().Name;
    }
}



public class SuperState<T> : State<T>
{
    public StateMachine<T> SubStateMachine;
    public SuperState(StateMachine<T> stateMachine) : base(stateMachine)
    {
        SubStateMachine = new StateMachine<T>(Owner);
    }

    public override void Exit()
    {
        if(SubStateMachine.CurrentState != null)
        {
            SubStateMachine.CurrentState.Exit();
        }
    }

    public override void Update()
    {
        if(SubStateMachine.CurrentState != null)
        {
            SubStateMachine.CurrentState.Update();
        }
    }

    public override void FixedUpdate()
    {
        if(SubStateMachine.CurrentState != null)
        {
            SubStateMachine.CurrentState.FixedUpdate();
        }
    }

}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T">Type of the base object executing the states</typeparam>
/// <typeparam name="B">super state</typeparam>
public class SubState<T, S> : State<T> where S : SuperState<T>
{
    protected S SuperState {get;}

    public SubState(S superState) : base(superState.StateMachine)
    {
        SuperState = superState;
    }
}
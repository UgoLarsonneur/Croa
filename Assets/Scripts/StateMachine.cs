
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StateMachine<T>
{
    protected IState<T> _currentState;
    public IState<T> CurrentState {
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
        CurrentState?.Update();
    }

    public virtual void FixedUpdate() {
        CurrentState?.FixedUpdate();
    }
}

public interface IState
{
    public void Enter();
    public void Exit();
    public void Update();
    public void FixedUpdate();
}

public interface IState<T> : IState
{
    public T Owner{get;}
}

public abstract class State<T> : IState<T>
{
    protected StateMachine<T> StateMachine {get;}
    public T Owner {get => StateMachine.Owner;}

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

    public override string ToString()
    {
        return base.ToString()+"/"+SubStateMachine.CurrentState.ToString();
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

    public SubState(S superState) : base(superState.SubStateMachine)
    {
        SuperState = superState;
    }
}


public abstract class SubState2<T> : StateMachine<T>, IState<T>
{
    protected StateMachine<T> StateMachine {get;}

    public SubState2(T owner) : base(owner) {}

    public virtual void Enter(){}
    public virtual void Exit(){}
}
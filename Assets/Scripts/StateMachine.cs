
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IStateMachine<T>
{
    public IState<T> CurrentState{get; set;}
    public T Owner {get;}
}


public class StateMachine<T> : IStateMachine<T>
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


public interface IState<T>
{
    public T Owner{get;}
    public void Enter();
    public void Exit();
    public void Update();
    public void FixedUpdate();
}


public abstract class State<T> : IState<T>
{
    protected IStateMachine<T> StateMachine {get;}
    public T Owner {get => StateMachine.Owner;}

    public State(IStateMachine<T> stateMachine)
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


public abstract class SequencedState<T> : StateMachine<T>, IState<T>
{
    protected IStateMachine<T> StateMachine {get;}

    public SequencedState(T owner) : base(owner) {}

    public virtual void Enter(){}
    public virtual void Exit(){}
}

public abstract class SequencedState2<T> : State<T>, IStateMachine<T>
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

    public SequencedState2(IStateMachine<T> stateMachine) : base(stateMachine) {}

}



public class SuperState<T> : State<T>
{
    public StateMachine<T> SubStateMachine;
    public SuperState(IStateMachine<T> stateMachine) : base(stateMachine)
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



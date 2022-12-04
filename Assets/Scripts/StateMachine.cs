
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T>
{
    public T Owner{get;}
    public StateMachine<T> StateMachine{get;}
    public void Enter();
    public void Exit();
    public void Update();
}


public interface IStateMachine<T>
{
    public T Owner{get;}
    public IState<T> CurrentState{get; set;}
    public void Update();
}


public class StateMachine<T> : IStateMachine<T>
{
    public virtual T Owner{get;}
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

    public virtual void Update() {
        CurrentState?.Update();
    }

    public StateMachine(T owner)
    {
        Owner = owner;
    }

}


public abstract class State<T> : IState<T>
{
    public StateMachine<T> StateMachine {get;}
    public T Owner => StateMachine.Owner;

    public State(StateMachine<T> stateMachine)
    {
        StateMachine = stateMachine;
    }

    public virtual void Enter(){}
    public virtual void Exit(){}
    public virtual void Update(){}

}


public abstract class SuperState<T> : StateMachine<T>, IState<T>
{
    public StateMachine<T> StateMachine{get;}

    public SuperState(StateMachine<T> stateMachine) : base(stateMachine.Owner)
    {
        StateMachine = stateMachine;
    }

    public virtual void Enter(){}
    public virtual void Exit(){
        CurrentState?.Exit();
    }

    public override string ToString()
    {
        return this.GetType().Name+"/"+CurrentState.GetType().Name;
    }
}


/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="S"></typeparam>
public abstract class SubState<T, S> : State<T> where S : SuperState<T>
{
    protected S SuperState => (S)StateMachine;
    public SubState(S superState) : base(superState) {}
}

/*public abstract class SuperState2<T> : State<T>, IStateMachine<T>
{
    public T Owner{get;}
    public IStateMachine<T> StateMachine{get;}

    public SuperState2(IStateMachine<T> stateMachine, T owner)
    {
        Owner = owner;
        StateMachine = stateMachine;
    }

    public virtual void Enter(){}
    public virtual void Exit(){
        CurrentState?.Exit();
    }

    public override string ToString()
    {
        return this.GetType().Name+"/"+CurrentState.GetType().Name;
    }
}*/









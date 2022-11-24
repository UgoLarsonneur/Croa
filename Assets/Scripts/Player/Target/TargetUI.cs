using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* public abstract class TargetUI : MonoBehaviour
{

    protected Renderer _renderer;
    protected bool _show;

    public virtual void Awake() {
        _renderer = GetComponent<Renderer>();
        _renderer.enabled = false;
        _show = false;

        EventManager.StartListening("Charge", OnCharge);
        EventManager.StartListening("Jump", OnJump);
    }

    protected virtual void OnCharge()
    {
        _renderer.enabled = true;
        _show = true;
    }

    protected virtual void OnJump()
    {
        _show = false;
        _renderer.enabled = false;
    }

    public virtual void Update() {
        if(_show)
            OnShow();
    }

    protected virtual void OnShow() {}

} */

public class TargetUI : MonoBehaviour
{
    protected Renderer _renderer;
    public Renderer MyRenderer => _renderer;

    StateMachine<TargetUI> _stateMachine;
}

/* namespace TargetUIStates
{
    public abstract class Neutral<T> : State<T> where T : TargetUI
    {
        public Neutral(StateMachine<T> stateMachine) : base(stateMachine) {}

        public override void Enter()
        {
            base.Enter();
            EventManager.StartListening("Charge", OnCharge);
        }

        public abstract void OnCharge();

        public override void Exit()
        {
            base.Exit();
            EventManager.StopListening("Charge", OnCharge);
        }
    }

    public abstract class Charge<T> : State<T> where T : TargetUI
    {
        public Charge(StateMachine<T> stateMachine) : base(stateMachine) {}

        public override void Enter()
        {
            base.Enter();
            EventManager.StartListening("Jump", OnJump);
        }

        public abstract void OnJump();

        public override void Exit()
        {
            base.Exit();
            EventManager.StopListening("Jump", OnJump);
        }
    }
}
 */
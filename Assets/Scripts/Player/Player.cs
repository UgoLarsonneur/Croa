using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerStates;


public class Player : MonoBehaviour
{
    /*___Jumping___*/
    [SerializeField] float chargeDuration;
    [SerializeField] float minJumpDuration;
    [SerializeField] float maxJumpDuration;
    [SerializeField] float minJumpDist;
    [SerializeField] float maxJumpDist;
    [SerializeField] float maxJumpHeight;
    [SerializeField] AnimationCurve jumpShape;
    [SerializeField] AnimationCurve chargeMaxHeight;
    [SerializeField] float timeToCharge;

    public float ChargeDuration => chargeDuration;
    public float MinJumpDuration => minJumpDuration;
    public float MaxJumpDuration => maxJumpDuration;
    public float MinJumpDist => minJumpDist;
    public float MaxJumpDist => maxJumpDist;
    public float MaxJumpHeight => maxJumpHeight;
    public AnimationCurve JumpShape => jumpShape;
    public AnimationCurve ChargeMaxHeight => chargeMaxHeight;
    

    /*___Turning___*/
    [SerializeField] float turnSpeed;
    [SerializeField] float maxTurnAngle;

    public float TurnSpeed => turnSpeed;
    public float MaxTurnAngle => maxTurnAngle;

    StateMachine<Player> _stateMachine;
    public StateMachine<Player> SM => _stateMachine;

    private void Awake() {
        _stateMachine = new StateMachine<Player>(this);
        _stateMachine.setState(new PlayerStates.PlayerIdle(_stateMachine));
    }
    
    private void Update() {
        _stateMachine.update();

        if(Input.GetKeyDown(KeyCode.R))
        {
            transform.position = Vector3.zero;
        }
    }

    private void FixedUpdate() {
        _stateMachine.fixedUpdate();
    }

    public float getJumpDistance(float charge)
    {
        return MinJumpDist + (MaxJumpDist - MinJumpDist) * charge;
    }

    private void OnGUI() {
        if(GameManager.Instance.DebugEnabled)
            GUILayout.Box("Player State: " + _stateMachine.CurrentState.ToString());
    }
    
}

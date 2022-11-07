using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerStates;


public class Player : MonoBehaviour
{
    /*___Jumping___*/
    [SerializeField] float jumpChargeDuration;
    [SerializeField] float minJumpDuration;
    [SerializeField] float maxJumpDuration;
    [SerializeField] float minJumpDist;
    [SerializeField] float maxJumpDist;
    [SerializeField] float maxJumpHeight;
    [SerializeField] AnimationCurve jumpShape;
    [SerializeField] AnimationCurve chargeMaxHeight;
    [SerializeField] float timeToCharge;

    public float JumpChargeDuration {get => jumpChargeDuration;}
    public float MinJumpDuration {get => minJumpDuration;}
    public float MaxJumpDuration {get => maxJumpDuration;}
    public float MinJumpDist {get => minJumpDist;}
    public float MaxJumpDist {get => maxJumpDist;}
    public float MaxJumpHeight {get => maxJumpHeight;}
    public AnimationCurve JumpShape {get => jumpShape;}
    public AnimationCurve ChargeMaxHeight {get => chargeMaxHeight;}



    /*___Turning___*/
    //[SerializeField] float turnSpeed;

   PlayerStateMachine stateMachine;

    private void Awake() {
        stateMachine = new PlayerStateMachine(this);
    }
    
    private void Update() {
        stateMachine.update();

        if(Input.GetKeyDown(KeyCode.R))
        {
            transform.position = Vector3.zero;
        }
    }

    private void FixedUpdate() {
        stateMachine.fixedUpdate();
    }

    public class PlayerStateMachine : StateMachine<Player>
    {
        public PlayerStateMachine(Player owner) : base(owner)
        {
            setState(new PlayerIdle(this));
        }
    }

    private void OnGUI() {
        if(GameManager.Instance.DebugEnabled)
            GUILayout.Box("Player State: "+stateMachine.CurrentState.ToString());
    }
    
}

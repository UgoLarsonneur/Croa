using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    public StateMachine<Player> StateMachine {get; protected set;}

    private float _angle = 0f;
    public float Angle { get => _angle; set => _angle = value; }

    private void Awake() {
        StateMachine = new StateMachine<Player>(this);
        StateMachine.CurrentState = new PlayerStates.Idle(StateMachine);
    }
    
    private void Update() {
        StateMachine.Update();

        if(Input.GetKeyDown(KeyCode.R))
        {
            transform.position = Vector3.zero;
        }
    }

    private void FixedUpdate() {
        StateMachine.FixedUpdate();
    }

    public float getJumpDistance(float charge)
    {
        return MinJumpDist + (MaxJumpDist - MinJumpDist) * charge;
    }

    public float getJumpLength(float charge)
    {
        return MinJumpDuration + (MaxJumpDuration - MinJumpDuration) * charge;
    }

    private void OnGUI() {
        if(GameManager.Instance.DebugEnabled)
            GUILayout.Box("Player State: " + StateMachine.CurrentState.ToString());
    }
    
}

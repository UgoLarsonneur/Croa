using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public float Angle {get; set;} = 0f;
    
    public float TurnSpeed => turnSpeed;
    public float MaxTurnAngle => maxTurnAngle;



    /*___States___*/
    public StateMachine<Player> StateMachine {get; protected set;}


    private void Awake() {
        StateMachine = new StateMachine<Player>(this);
        StateMachine.CurrentState = new PlayerStates.Idle(StateMachine);
    }

    private void Start() {
        TryLand();
    }
    
    private void Update() {
        StateMachine.Update();

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public float getJumpDistance(float charge)
    {
        return MinJumpDist + (MaxJumpDist - MinJumpDist) * charge;
    }

    public float getJumpDuration(float charge)
    {
        return MinJumpDuration + (MaxJumpDuration - MinJumpDuration) * charge;
    }


    public bool TryLand()
    {
        Physics.Raycast(transform.position+Vector3.up*10f, Vector3.down, out RaycastHit hit, 10f, LayerMask.GetMask("Platform"));
        if(hit.collider != null)
        {
            Platform platform = hit.collider.gameObject.GetComponent<Platform>();
            platform.OnLandedOn();
            transform.parent = platform.transform;
            transform.localPosition = new Vector3(transform.localPosition.x, 0f, transform.localPosition.z);
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Obstacle") && StateMachine.CurrentState is not PlayerStates.Drowning)
        {
            StateMachine.CurrentState = new PlayerStates.Drowning(StateMachine);
        }
    }
}

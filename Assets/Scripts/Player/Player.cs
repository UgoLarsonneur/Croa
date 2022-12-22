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
            transform.position = Vector3.zero;
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

    public void MoveAngle()
    {
        float angleDelta = 0f;
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow))
        {
            angleDelta += 1f;
        }
        if(Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.RightArrow))
        {
            angleDelta -= 1f;
        }

        Angle = Mathf.Clamp(Angle + angleDelta * Time.deltaTime * TurnSpeed, -MaxTurnAngle, MaxTurnAngle);
    }


    public bool TryLand()
    {
        Physics.Raycast(transform.position+Vector3.up*10f, Vector3.down, out RaycastHit hit, 10f, LayerMask.GetMask("Platform"));
        if(hit.collider != null)
        {
            Platform platform = hit.collider.gameObject.GetComponent<Platform>();
            platform.OnLandedOn();
            transform.parent = platform.transform; //TODO: remplacer le parent par la mesh de la plateforme
            transform.localPosition = new Vector3(transform.localPosition.x, 0f, transform.localPosition.z);
            return true;
        }
        return false;
    }


    private void OnGUI() {
        if(GameManager.Instance.DebugEnabled)
        {
            GUILayout.Box("Player State: " + StateMachine.CurrentState.ToString());
            GUILayout.Space(10);
        }
    }
    
}

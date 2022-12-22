using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


/* public class JumpTarget : TargetUI
{
    [SerializeField] protected Player GameManager.Player;

    protected override void OnShow()
    {
        PlayerStates.PlayerCharge jumpPhase = (PlayerStates.PlayerCharge)GameManager.Player.SM.CurrentState;
        Vector3 pos = Quaternion.AngleAxis(jumpPhase.Angle, Vector3.up) * Vector3.forward * GameManager.Player.getJumpDistance(jumpPhase.Charge) + Vector3.up * 0.05f;
        currentChargeTarget.position = GameManager.Player.currentChargeTarget.position + pos;
    }
} */
//TODO: Refactor avec des States
public class JumpTarget : MonoBehaviour
{
    [SerializeField] Transform currentChargeTarget;
    [SerializeField] Transform maxTarget;

    private SpriteRenderer _currentTargetRenderer;
    private SpriteRenderer _maxTargetRenderer;

    private void Awake() {
        _currentTargetRenderer = currentChargeTarget.GetComponent<SpriteRenderer>();
        _maxTargetRenderer = maxTarget.GetComponent<SpriteRenderer>();
        _maxTargetRenderer.enabled = false;
    }

    private void Start() {
        EventManager.StartListening("Land", OnLand);
        EventManager.StartListening("Charge", OnCharge);
        EventManager.StartListening("Jump", OnJump);

        maxTarget.transform.localPosition = Vector3.forward * GameManager.Player.getJumpDistance(1f);
    }

    private void OnLand()
    {
        UpdateCurrentChargeTargetPos(0f);
        _currentTargetRenderer.enabled = true;
    }

    private void OnCharge()
    {
        _maxTargetRenderer.enabled = true;
    }

    private void OnJump()
    {
        _currentTargetRenderer.enabled = false;
        _maxTargetRenderer.enabled = false;
    }

    private void Update() {

        transform.rotation = Quaternion.AngleAxis(GameManager.Player.Angle, Vector3.up);
        
        IState<Player> playerState = GameManager.Player.StateMachine.CurrentState;
        if(!(playerState is PlayerStates.JumpPhase))
            return;

        PlayerStates.JumpPhase jumpPhase = (PlayerStates.JumpPhase)playerState;
        UpdateCurrentChargeTargetPos(jumpPhase.Charge);
    }

    private void UpdateCurrentChargeTargetPos(float charge)
    {
        currentChargeTarget.localPosition = Vector3.forward * GameManager.Player.getJumpDistance(charge);
    }

    private void OnDestroy() {
        EventManager.StopListening("Land", OnLand);
        EventManager.StopListening("Charge", OnCharge);
        EventManager.StopListening("Jump", OnJump);
    }
}

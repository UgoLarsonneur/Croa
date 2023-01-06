using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


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
        EventManager.StartListening("Jump", Hide);
        EventManager.StopListening("Drown", Hide);

        maxTarget.transform.localPosition = Vector3.forward * GameManager.Player.getJumpDistance(1f);
        UpdateCurrentChargeTargetPos(0f);
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

    private void Hide()
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
        EventManager.StopListening("Jump", Hide);
        EventManager.StopListening("Drown", Hide);
    }
}

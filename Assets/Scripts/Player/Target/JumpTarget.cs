using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* public class JumpTarget : TargetUI
{
    [SerializeField] protected Player player;

    protected override void OnShow()
    {
        PlayerStates.PlayerCharge jumpPhase = (PlayerStates.PlayerCharge)player.SM.CurrentState;
        Vector3 pos = Quaternion.AngleAxis(jumpPhase.Angle, Vector3.up) * Vector3.forward * player.getJumpDistance(jumpPhase.Charge) + Vector3.up * 0.05f;
        transform.position = player.transform.position + pos;
    }
} */

public class JumpTarget : MonoBehaviour
{
    [SerializeField] Player player;

    private SpriteRenderer _renderer;

    private void Awake() {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.enabled = false;
    }

    private void Start() {
        EventManager.StartListening("Charge", OnCharge);
        EventManager.StartListening("Jump", OnJump);
        enabled = false;
    }

    private void OnCharge()
    {
        UpdatePos();
        enabled = true;
        _renderer.enabled = true;
    }

    private void OnJump()
    {
        enabled = false;
        _renderer.enabled = false;
    }

    private void Update() {
        UpdatePos();
    }

    private void UpdatePos()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.up, player.transform.position - transform.position);
        PlayerStates.JumpPhase jumpPhase = (PlayerStates.JumpPhase)player.StateMachine.CurrentState;
        //Vector3 pos = Quaternion.AngleAxis(jumpPhase.Angle, Vector3.up) * Vector3.forward * player.getJumpDistance(jumpPhase.Charge) + Vector3.up * 0.05f;
        Vector3 pos = Quaternion.AngleAxis(player.Angle, Vector3.up) * Vector3.forward * player.getJumpDistance(jumpPhase.Charge) + Vector3.up * 0.05f;
        transform.position = player.transform.position + pos;
    }
}

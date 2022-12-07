using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


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
//TODO: Refactor avec des States
public class JumpTarget : MonoBehaviour
{
    [SerializeField] Player player;

    private float _distance;
    private SpriteRenderer _renderer;

    private void Awake() {
        _renderer = GetComponent<SpriteRenderer>();
        _distance  = player.getJumpDistance(0f);
    }

    private void Start() {
        EventManager.StartListening("Land", OnLand);
        EventManager.StartListening("Jump", OnJump);
    }

    private void OnLand()
    {
        _distance  = player.getJumpDistance(0f);
        UpdatePos();
        _renderer.enabled = true;
        //DOTween.To(() => _distance, x => _distance = x, player.getJumpDistance(0f), 0.1f);
    }

    private void OnJump()
    {
        _renderer.enabled = false;
    }

    private void Update() {
        UpdatePos();
    }

    private void UpdatePos()
    {

        IState<Player> playerState = player.StateMachine.CurrentState;
        if(playerState is PlayerStates.JumpPhase)
        {
            PlayerStates.JumpPhase jumpPhase= (PlayerStates.JumpPhase)playerState;
            if(jumpPhase.CurrentState is PlayerStates.Jumping)
                return;
            _distance = player.getJumpDistance(((PlayerStates.JumpPhase)playerState).Charge);
        }

        transform.rotation = Quaternion.LookRotation(Vector3.up, player.transform.position - transform.position);
        Vector3 pos = Quaternion.AngleAxis(player.Angle, Vector3.up) * Vector3.forward * _distance + Vector3.up * 0.05f;
        transform.position = player.transform.position + pos;


    }
}

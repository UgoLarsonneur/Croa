using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAnimHandler : MonoBehaviour
{
    [SerializeField] float turnLength = 0.1f;

    Animator _animator;

    int _landTriggerHash;
    int _chargeTriggerHash;
    int _jumpTriggerHash;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _landTriggerHash = Animator.StringToHash("Land");
        _chargeTriggerHash = Animator.StringToHash("Charge");
        _jumpTriggerHash = Animator.StringToHash("Jump");
    }

    void Start()
    {
        EventManager.StartListening("Charge", LaunchChargeAnim);
        EventManager.StartListening("Jump", LaunchJumpAnim);
        EventManager.StartListening("Land", LaunchIdleAnim);
    }

    void LaunchJumpAnim()
    {
        _animator.SetTrigger(_jumpTriggerHash);
        //PlayerStates.JumpPhase jumpPhase = (PlayerStates.JumpPhase)GameManager.Player.StateMachine.CurrentState;
        //transform.DORotate(new Vector3(0f, jumpPhase.Angle, 0f), turnLength).SetEase(Ease.OutQuint);
        transform.DORotate(new Vector3(0f, GameManager.Player.Angle, 0f), turnLength).SetEase(Ease.OutQuint);
        
    }

    void LaunchChargeAnim()
    {
        _animator.SetTrigger(_chargeTriggerHash);
    }

    void LaunchIdleAnim()
    {
        _animator.SetTrigger(_landTriggerHash);
        transform.DORotate(new Vector3(0f, 0f, 0f), turnLength).SetEase(Ease.InOutQuad);
    }
}

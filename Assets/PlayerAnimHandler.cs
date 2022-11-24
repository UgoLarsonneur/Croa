using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimHandler : MonoBehaviour
{
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
    }

    void LaunchChargeAnim()
    {
        _animator.SetTrigger(_chargeTriggerHash);
    }

    void LaunchIdleAnim()
    {
        _animator.SetTrigger(_landTriggerHash);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAnimHandler : MonoBehaviour
{
    [SerializeField] float turnLength = 0.1f;

    [SerializeField] Animator _animator;

    int _landTriggerHash;
    int _chargeTriggerHash;
    int _jumpTriggerHash;
    int _drownTriggerHash;

    private void Awake()
    {
        _landTriggerHash = Animator.StringToHash("Land");
        _chargeTriggerHash = Animator.StringToHash("Charge");
        _jumpTriggerHash = Animator.StringToHash("Jump");
        _drownTriggerHash = Animator.StringToHash("Drown");
    }

    void Start()
    {
        EventManager.StartListening("Charge", LaunchChargeAnim);
        EventManager.StartListening("Jump", LaunchJumpAnim);
        EventManager.StartListening("Land", LaunchIdleAnim);
        EventManager.StartListening("Drown", LaunchDrownAnim);
    }

    void LaunchJumpAnim()
    {
        _animator.SetTrigger(_jumpTriggerHash);
        Debug.Log(GameManager.Player.Angle);
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

    void LaunchDrownAnim()
    {
        _animator.SetTrigger(_drownTriggerHash);
    }
}

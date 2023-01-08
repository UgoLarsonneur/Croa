using UnityEngine;
using System;

public class Platform : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    public Animator Animator => animator;

    public Action OnLandDelegate;


    protected Collider _collider;

    public int Number{get; set;} //first platform is platform 0, next is 1, next is 2,...

    bool isHostingPlayer = false;

    private void Awake() {
        OnAwake();
        _collider = GetComponent<Collider>();
    }

    private void Start() {
        EventManager.StartListening("Jump", OnJump);
    }

    private void Update() {
        OnUpdate();
    }

    virtual protected void OnUpdate()
    {
        if(transform.position.z < Camera.main.transform.position.z - 1f)
        {
            Destroy(gameObject);
        }
    }


    virtual public void OnLandedOn()
    {
        animator.SetTrigger("Shake");
        isHostingPlayer = true;
        GameManager.UpdatePlatformCounts(Number);
        if(OnLandDelegate != null)
            OnLandDelegate();
    }


    virtual protected void OnAwake()
    {
    }

    protected void OnJump()
    {
        isHostingPlayer = false;
    }

    public void OnSink()
    {
        _collider.enabled = false;

        if(isHostingPlayer)
            GameManager.Player.StateMachine.CurrentState = new PlayerStates.Drowning(GameManager.Player.StateMachine);

        GameManager.Spawner.RemovePlatform(this);
        animator.SetTrigger("Sink");
    }


    protected virtual void OnDestroy() {
        if(GameManager.Quitting)
            return;
        GameManager.Spawner.RemovePlatform(this);
        EventManager.StopListening("Jump", OnJump);
    }


    // void CheckIfBehindCamera()
    // {
    //     if(transform.position.z < Camera.main.transform.position.z - 1f)
    //     {
    //         Destroy(gameObject);
    //     }
    // }
}

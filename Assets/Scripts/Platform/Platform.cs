using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    
    public int Number{get; set;} //first platform is platform 0, next is 1, next is 2,...


    private void Awake() {
        OnAwake();
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
        GameManager.LastPlatformReached = Mathf.Max(Number, GameManager.LastPlatformReached);
        GameManager.Spawner.CheckForRapidMode();
    }


    virtual protected void OnAwake()
    {
        //EventManager.StartListening("Land", CheckIfBehindCamera);
    }


    private void OnDestroy() {
        if(GameManager.Quitting)
            return;
        //EventManager.StopListening("Land", CheckIfBehindCamera);
        GameManager.Spawner.RemovePlatform(this);
    }


    // void CheckIfBehindCamera()
    // {
    //     if(transform.position.z < Camera.main.transform.position.z - 1f)
    //     {
    //         Destroy(gameObject);
    //     }
    // }
}

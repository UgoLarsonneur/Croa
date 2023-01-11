using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePlatform : Platform
{
    //TODO: faire marcher
    /*private void Start() {
        EventManager.StartListening("Land", OnSink);
    }

    protected override void OnDestroy() {
        EventManager.StopListening("Land", OnSink);
    }*/

    private void OnTriggerEnter(Collider other) {
        
        //if((other.gameObject.layer & LayerMask.GetMask("Obstacle", "Platform")) == other.gameObject.layer)
        if(other.gameObject.layer == LayerMask.NameToLayer("Obstacle") || other.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            OnSink();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingForwardPlatform : Platform
{


    protected override void OnAwake() {
        base.OnAwake();
        transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }


    protected override void OnUpdate()
    {
        base.OnUpdate();
        transform.position -= Vector3.forward * Time.deltaTime * GameManager.GlobalSpeed;
    }

    private void OnTriggerEnter(Collider other) {
        
        //if((other.gameObject.layer & LayerMask.GetMask("Obstacle", "Platform")) == other.gameObject.layer)
        if(other.gameObject.layer == LayerMask.NameToLayer("Obstacle") || other.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            OnSink();
        }
    }
}

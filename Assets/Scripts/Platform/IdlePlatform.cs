using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePlatform : Platform
{

    private void OnTriggerEnter(Collider other) {
        
        if(other.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            OnSink();
        }
    }
    
}

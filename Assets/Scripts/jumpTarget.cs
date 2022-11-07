using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpTarget : MonoBehaviour
{
    private SpriteRenderer _renderer;

    private void Start() {
        EventManager.StartListening("Charge", OnCharge);
        EventManager.StartListening("Jump", OnJump);
    }

    private void OnCharge()
    {
        Debug.Log("Charge!!!");
    }

    private void OnJump()
    {
        Debug.Log("Jump!!!");
    }
    
}

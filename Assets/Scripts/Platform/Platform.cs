using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public int Number {get; private set;} //first platform is platform 0, next is 1, next is 2,...


    private void Awake() {
        OnAwake();
    }


    virtual protected void OnAwake()
    {
        Number = PlatformSpawner.lastPlatformNumber++;
    }
}

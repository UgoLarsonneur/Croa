using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingFromLand : Crumbling
{
    void Start()
    {
        platform.OnLandDelegate += BeginCrumbling;
    }
}

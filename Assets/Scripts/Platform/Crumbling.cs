using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Platform))]
public class Crumbling : MonoBehaviour
{
    [SerializeField] float crumblingDuration = 3f;//time before the platform sinks;
    [SerializeField] float bigWarningDuration = 0.5f;

    protected Platform platform;


    private void Awake() {
        platform = GetComponent<Platform>();
    }

    protected void BeginCrumbling()
    {
        StartCoroutine(Crumble());
    }


    protected IEnumerator Crumble()
    {
        platform.Animator.SetBool("Crumbling", true);
        yield return new WaitForSeconds(crumblingDuration);
        platform.Animator.SetBool("Warning", true);
        yield return new WaitForSeconds(bigWarningDuration);
        platform.OnSink();
    }
}

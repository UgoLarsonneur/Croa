using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHUD : MonoBehaviour
{
    [SerializeField] GameObject deathMenu;
    [SerializeField] float deathMenuDelay = 1.5f;

    private void Start() {
        EventManager.StartListening("Drown",ShowDeathMenu);
    }

    private void ShowDeathMenu()
    {
        StartCoroutine(DelayShowDeathmenu());
    }

    private IEnumerator DelayShowDeathmenu()
    {
        yield return new WaitForSeconds(deathMenuDelay);
        deathMenu.SetActive(true);
    } 

    private void OnDestroy() {
        EventManager.StopListening("Drown", ShowDeathMenu);
    }

}

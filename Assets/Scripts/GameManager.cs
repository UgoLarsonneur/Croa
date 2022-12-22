using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    static public bool Quitting = false;

    private void OnApplicationQuit() {
        Quitting = true;
    } 

    [SerializeField] private Player player;
    public static Player Player => Instance.player;

    [SerializeField] private PlatformSpawner spawner;
    public static PlatformSpawner Spawner => Instance.spawner;

    [SerializeField] AnimationCurve globalSpeedByPlatformReached;
    public static float GlobalSpeed {
        get {
            return Instance.globalSpeedByPlatformReached.Evaluate(LastPlatformReached);
        }
    }

    public static int LastPlatformReached {get; set;} = 0;

    public bool DebugEnabled {get; private set;} = false;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.T))
        {
            DebugEnabled = !DebugEnabled;
        }
    }

    private void OnGUI() {
        if(!DebugEnabled)
            return;

        GUILayout.Box("Player State: " + Player.StateMachine.CurrentState.ToString());

        GUILayout.Box("Global speed:"+ GlobalSpeed);
        GUILayout.Box("Platform spawn count: " + Spawner.SpawnedCount);
        GUILayout.Box("Last platform reached: " + GameManager.LastPlatformReached);
        GUILayout.Box("Current non critical platform spawn chance: " + Spawner.NonCtriticalPlatformChance);
        GUILayout.Box("Current unsafe platform spawn chance: " + Spawner.UnsafePlatformChance);
    }
    
}

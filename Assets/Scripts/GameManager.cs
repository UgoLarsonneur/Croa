using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance{get; private set;}

    static public bool Quitting = false;

    private void Awake() {
        Instance = this;
    }

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

    int lastPlatformReached = 0;
    public static int LastPlatformReached => Instance.lastPlatformReached;

    private int numberOfPlatformReached = 0;
    public static int NumberOfPlatformReached => Instance.numberOfPlatformReached;

    public bool DebugMenuEnabled {get; private set;} = false;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.T))
        {
            DebugMenuEnabled = !DebugMenuEnabled;
        }
    }

    public static void UpdatePlatformCounts(int platformNumber)
    {
        Instance.lastPlatformReached = Mathf.Max(platformNumber, Instance.lastPlatformReached);
        ++Instance.numberOfPlatformReached;
    }

    private void OnGUI() {
        if(!DebugMenuEnabled)
            return;

        GUILayout.Box("Player State: " + Player.StateMachine.CurrentState.ToString());
        GUILayout.Box("Global speed:"+ GlobalSpeed);
        GUILayout.Box("Platform spawn count: " + Spawner.SpawnedCount);
        GUILayout.Box("Number of platform reached: " + NumberOfPlatformReached);
        GUILayout.Box("Last platform reached: " + LastPlatformReached);
        GUILayout.Box("Current non critical platform spawn chance: " + Spawner.NonCtriticalPlatformChance);
        GUILayout.Box("Current semo platform spawn chance: " + Spawner.SemiSafePlatformChance);
        GUILayout.Box("Current unsafe platform spawn chance: " + Spawner.UnsafePlatformChance);
    }
    
}

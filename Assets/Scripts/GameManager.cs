using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance {
        get {
            if(_instance == null)
            {
                GameObject gameObject = new GameObject();
                _instance = gameObject.AddComponent<GameManager>();
                gameObject.name = "GameManager";
            }

            return _instance;
        }
    
        private set {
            _instance = value;
        }
    }

    private bool _debugEnabled = false;
    public bool DebugEnabled => _debugEnabled;

    private void Awake() {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Update() {

        if(Input.GetKeyDown(KeyCode.T))
        {
            _debugEnabled = !_debugEnabled;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager _instance;

    private Dictionary <string, Action> eventDictionary;

    public static EventManager Instance {
        get {
            /*if(_instance == null)
            {
                GameObject gameObject = new GameObject();
                _instance = gameObject.AddComponent<GameManager>();
                gameObject.name = "GameManager";
            }*/

            return _instance;
        }
    
        private set {
            _instance = value;
        }
    }

    private void Awake() {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        _instance = this;

        eventDictionary = new Dictionary<string, Action>();
    }

    public static void TriggerEvent(string eventName)
    {
        if(_instance.eventDictionary.ContainsKey(eventName))
            _instance.eventDictionary[eventName]();
    }

    public static void StartListening(string eventName, Action listener)
    {
        Action thisEvent;
        if (_instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            _instance.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            _instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, Action listener)
    {
        if (_instance == null) return;

        Action thisEvent;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            _instance.eventDictionary[eventName] = thisEvent;
        }
    }

    

}

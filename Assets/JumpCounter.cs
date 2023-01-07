using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening; 

public class JumpCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float incrementPunchValue = 0.5f;
    [SerializeField] float incrementPunchDuration = 0.2f;

    void Start()
    {
        EventManager.StartListening("Land", UpdateText);
    }

    private void UpdateText()
    {
        text.text = "Jumps: "+GameManager.NumberOfPlatformReached;
        transform.DOPunchScale( new Vector3(incrementPunchValue, incrementPunchValue, incrementPunchValue), incrementPunchDuration);
    }

    private void OnDestroy() {
        EventManager.StopListening("Land", UpdateText);
    }
}

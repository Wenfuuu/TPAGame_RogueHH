using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTrigger : MonoBehaviour
{
    private Button button;
    private EventTrigger eventTrigger;

    void Awake()
    {
        button = GetComponent<Button>();
        eventTrigger = GetComponent<EventTrigger>();
    }

    void Update()
    {
        if (eventTrigger != null && button != null)
        {
            eventTrigger.enabled = button.interactable;
        }
    }
}

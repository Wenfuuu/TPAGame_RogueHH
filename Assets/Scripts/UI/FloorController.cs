using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    public TextMeshProUGUI FloorText;

    public IntEventChannel UpdateFloorText;

    private void OnEnable()
    {
        UpdateFloorText.OnEventRaised += UpdateFloor;
    }

    private void OnDisable()
    {
        UpdateFloorText.OnEventRaised -= UpdateFloor;
    }

    private void UpdateFloor(int floor)
    {
        string text = $"Floor {floor}";
        FloorText.text = text;
    }
}

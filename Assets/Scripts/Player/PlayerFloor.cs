using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloor : MonoBehaviour
{
    public PlayerStatsSO playerStats;
    public IntEventChannel UpdateFloorText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateFloorText.RaiseEvent(playerStats.CurrentFloor);
    }

    public void IncreaseFloor()
    {
        playerStats.IncreaseFloor();
        UpdateFloorText.RaiseEvent(playerStats.CurrentFloor);
    }
}

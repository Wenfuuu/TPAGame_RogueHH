using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    public PlayerStatsSO playerStats;
    public IntEventChannel UpdateLevelPlayer;

    // Start is called before the first frame update
    void Start()
    {
        UpdateLevelPlayer.RaiseEvent(playerStats.PlayerLevel);
    }

    public void IncreaseLevel()
    {
        playerStats.LevelUp();
        UpdateLevelPlayer.RaiseEvent(playerStats.PlayerLevel);
    }
}

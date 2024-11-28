using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZhen : MonoBehaviour
{
    public PlayerStatsSO playerStats;
    public IntEventChannel UpdateZhen;

    // Start is called before the first frame update
    void Start()
    {
        UpdateZhen.RaiseEvent(playerStats.Zhen);
    }

    public void IncreaseZhen(int zhen)
    {
        playerStats.IncreaseZhen(zhen);
        UpdateZhen.RaiseEvent(playerStats.Zhen);
    }
}

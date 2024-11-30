using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageable : MonoBehaviour
{
    public PlayerStatsSO playerStats;
    public IntEventChannel SetupHPPlayer;
    public IntEventChannel UpdateHPPlayer;
    public TwoIntEventChannel UpdateHPText;

    // Start is called before the first frame update
    void Start()
    {
        SetupHPPlayer.RaiseEvent(playerStats.MaxHP);
        UpdateHPPlayer.RaiseEvent(playerStats.CurrentHP);
        UpdateHPText.RaiseEvent(playerStats.CurrentHP, playerStats.MaxHP);
    }

    public void IncreaseHealth(int heal)
    {
        playerStats.IncreaseHealth(heal);
        UpdateHPPlayer.RaiseEvent(playerStats.CurrentHP);
        UpdateHPText.RaiseEvent(playerStats.CurrentHP, playerStats.MaxHP);
    }

    public void DecreaseHealth(int damage)
    {
        playerStats.DecreaseHealth(damage);
        UpdateHPPlayer.RaiseEvent(playerStats.CurrentHP);
        UpdateHPText.RaiseEvent(playerStats.CurrentHP, playerStats.MaxHP);
    }

    public void UpdateHealth()
    {
        UpdateHPPlayer.RaiseEvent(playerStats.CurrentHP);
        UpdateHPText.RaiseEvent(playerStats.CurrentHP, playerStats.MaxHP);
    }
}

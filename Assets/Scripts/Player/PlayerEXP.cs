using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEXP : MonoBehaviour
{
    public PlayerStatsSO playerStats;
    public IntEventChannel SetupEXPPlayer;
    public IntEventChannel UpdateEXPPlayer;
    public TwoIntEventChannel UpdateEXPText;

    // Start is called before the first frame update
    void Start()
    {
        SetupEXPPlayer.RaiseEvent(playerStats.MaxEXP);
        UpdateEXPPlayer.RaiseEvent(playerStats.CurrentEXP);
        UpdateEXPText.RaiseEvent(playerStats.CurrentEXP, playerStats.MaxEXP);
    }

    public void IncreaseEXP(int exp)
    {
        playerStats.IncreaseEXP(exp);
        // level up
        if(playerStats.CurrentEXP >= playerStats.MaxEXP)
        {
            LevelPopUpGenerator.Instance.CreatePopUp(transform.position);
            gameObject.GetComponent<PlayerLevel>().IncreaseLevel();
            gameObject.GetComponent<PlayerDamageable>().UpdateHealth();
        }
        UpdateEXPPlayer.RaiseEvent(playerStats.CurrentEXP);
        UpdateEXPText.RaiseEvent(playerStats.CurrentEXP, playerStats.MaxEXP);
    }
}

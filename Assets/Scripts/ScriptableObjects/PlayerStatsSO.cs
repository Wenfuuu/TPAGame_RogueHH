using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "Character/Player/Stats")]
public class PlayerStatsSO : ScriptableObject
{
    [Header("Stats")]
    public int PlayerLevel;
    public int MaxHP;
    public int CurrentHP;
    public int MaxEXP;
    public int CurrentEXP;
    public int Attack;
    public int Defense;
    public int CritRate;
    public int CritDamage;

    [Header("Upgradeable")]
    public int HPLevel;
    public int AtkLevel;
    public int DefLevel;
    public int CRLevel;
    public int CDLevel;
    public int TotalUpgrade;

    [Header("Upgrade Cost")]
    public int HPCost;
    public int AtkCost;
    public int DefCost;
    public int CRCost;
    public int CDCost;

    [Header("Resources")]
    public int Zhen;

    [Header("Multiplier")]
    public float MaxHPMultiplier;
    public float MaxEXPMultiplier;
    public float AtkMultiplier;
    public float DefMultiplier;

    [Header("Progress")]
    public int UnlockedFloor;
    public int CurrentFloor;
    public bool IsSaved;

    public void ResetStats()
    {
        PlayerLevel = 1;
        MaxHP = 20;
        CurrentHP = MaxHP;
        MaxEXP = 5;
        CurrentEXP = 0;
        Attack = 5;
        Defense = 5;
        CritRate = 5;
        CritDamage = 150;
        HPLevel = 0;
        AtkLevel = 0;
        DefLevel = 0;
        CRLevel = 0;
        CDLevel = 0;
        TotalUpgrade = 0;
        HPCost = 10;
        AtkCost = 10;
        DefCost = 10;
        CRCost = 10;
        CDCost = 10;
        Zhen = 0;
        UnlockedFloor = 1;
    }

    public void ResetHP(){
        CurrentHP = MaxHP;
    }

    public void IncreaseHealth(int heal)
    {
        CurrentHP += heal;
    }

    public void DecreaseHealth(int damage)
    {
        CurrentHP -= damage;
    }

    public void IncreaseEXP(int exp)
    {
        CurrentEXP += exp;
    }

    public void LevelUp()
    {
        while (CurrentEXP >= MaxEXP)
        {
            CurrentEXP -= MaxEXP;
            PlayerLevel++;
            // configure stats here
            MaxEXP = Mathf.RoundToInt(MaxEXP * MaxEXPMultiplier);
            MaxHP = Mathf.RoundToInt(MaxHP * MaxHPMultiplier);
            Attack = Mathf.RoundToInt(Attack * AtkMultiplier);
            Defense = Mathf.RoundToInt(Defense * DefMultiplier);
            CritRate += 5;
            CritDamage += 5;
            //CurrentHP = MaxHP;
        }
    }

    public void IncreaseZhen(int zhen)
    {
        Zhen += zhen;
    }

    public void DecreaseZhen(int zhen)
    {
        Zhen -= zhen;
    }

    public void IncreaseFloor()
    {
        CurrentFloor++;
        if(UnlockedFloor < 101 && (UnlockedFloor < CurrentFloor))
        UnlockedFloor++;
    }

    public void UnlockAllFloor()
    {
        UnlockedFloor = 101;
    }
}

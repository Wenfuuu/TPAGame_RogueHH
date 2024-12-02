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

    [Header("Resources")]
    public int Zhen;

    [Header("Multiplier")]
    public float MaxHPMultiplier = 1.764f;
    public float MaxEXPMultiplier = 1.662f;
    public float AtkMultiplier = 1.687f;
    public float DefMultiplier = 1.687f;

    [Header("Progress")]
    public int UnlockedFloor;
    public int CurrentFloor;
    public bool IsSaved;

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
        if(UnlockedFloor < 101 && (UnlockedFloor == CurrentFloor))
        UnlockedFloor++;
    }

    public void UnlockAllFloor()
    {
        UnlockedFloor = 101;
    }
}

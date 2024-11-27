using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats
{
    public int MaxHP;
    public int CurrentHP;
    public int Attack;
    public int Defense;
    public int ZhenDrop;
    public int ExpDrop;

    public EnemyStats()
    {
        MaxHP = 100;
        CurrentHP = 100;
        Attack = 10;
        Defense = 20;
        ZhenDrop = 50;
        ExpDrop = 50;
    }

    public void DecreaseHealth(int damage)
    {
        CurrentHP -= damage;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public static Sounds Instance;

    public AudioClip[] SwordSFX;
    public AudioClip[] PunchSFX;
    public AudioClip[] CriticalSFX;
    public AudioClip[] GruntSFX;
    public AudioClip[] DeathSFX;
    public AudioClip[] AlertSFX;
    public AudioClip[] StepSFX;
    public AudioClip[] PurchaseSFX;
    public AudioClip BashSFX;
    public AudioClip HealSFX;
    public AudioClip InventoryOpenSFX;
    public AudioClip InventoryCloseSFX;
    public AudioClip CheatCodeSFX;

    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillSO : ScriptableObject
{
    public string Name;
    public string Description;

    public int UnlockLevel;
    public bool IsLocked;

    public int Cooldown;
    public int CurrentCooldown;

    public bool IsReady;

    public BoolEventChannel LockSkill;
    public BoolEventChannel CooldownSkill;
    public IntEventChannel UpdateCooldownText;

    public abstract void Use(GameObject player);
    
    public void OnCooldown()
    {
        Debug.Log("skill is on cooldown");
        IsReady = false;
        CooldownSkill.RaiseEvent(true);
        UpdateCooldownText.RaiseEvent(CurrentCooldown);
    }

    public void OffCooldown()
    {
        IsReady = true;
        CooldownSkill.RaiseEvent(false);
    }

    public void Lock()
    {
        Debug.Log("locking skill");
        IsLocked = true;
        LockSkill.RaiseEvent(true);
    }

    public void Unlock()
    {
        Debug.Log("unlocking skill");
        IsLocked = false;
        LockSkill.RaiseEvent(false);
    }
}

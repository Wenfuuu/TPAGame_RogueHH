using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveSkillSO : SkillSO
{
    public bool IsSelected = false;// active skill

    public BoolEventChannel SelectedSkill;// only active

    public void OnSelected()
    {
        SelectedSkill.RaiseEvent(true);
    }

    public void OffSelected()
    {
        SelectedSkill.RaiseEvent(false);
    }

    public abstract void Attack(GameObject player);
    public abstract void FinishAttack(GameObject player);
}

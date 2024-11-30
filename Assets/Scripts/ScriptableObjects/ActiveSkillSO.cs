using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveSkillSO : SkillSO
{
    public bool IsSelected = false;// active skill

    public BoolEventChannel SelectedSkill;// only active
}

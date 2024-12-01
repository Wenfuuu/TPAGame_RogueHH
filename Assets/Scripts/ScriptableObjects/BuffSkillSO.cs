using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffSkillSO : SkillSO
{
    public int Duration;// buff skill
    public int CurrentDuration = 0;// buff skill
    public bool IsActive = false;

    public BoolEventChannel DurationSkill;// only buff
    public IntEventChannel UpdateDurationText;

    public void OnDuration()
    {
        DurationSkill.RaiseEvent(true);
        UpdateDurationText.RaiseEvent(CurrentDuration);
    }

    public void OffDuration()
    {
        DurationSkill.RaiseEvent(false);
    }

    public abstract void ApplyBuff(GameObject player);
}

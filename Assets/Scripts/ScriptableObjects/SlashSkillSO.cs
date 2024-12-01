using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlashSkillSO", menuName = "Skill/Active Skill/Slash")]
public class SlashSkillSO : ActiveSkillSO
{
    int atkBuff;//increase atk 20%

    public override void Attack(GameObject player)
    {
        // apply effect
        atkBuff = Mathf.RoundToInt(player.GetComponent<PlayerDamageable>().playerStats.Attack * 0.2f);
        player.GetComponent<PlayerDamageable>().playerStats.Attack += atkBuff;

        // add effect
        TrailRenderer t = player.GetComponent<PlayerStateMachine>().trail;
        t.startColor = Color.red;
        t.endColor = Color.yellow;

        JustUsed = true;
        IsSelected = false;
        OffSelected();
        //OnCooldown();
        //CurrentCooldown = Cooldown;
    }

    public override void FinishAttack(GameObject player)
    {
        player.GetComponent<PlayerDamageable>().playerStats.Attack -= atkBuff;

        //remove effect
        TrailRenderer t = player.GetComponent<PlayerStateMachine>().trail;
        t.startColor = Color.white;
        t.endColor = Color.gray;

        OnCooldown();
        CurrentCooldown = Cooldown;
    }

    public override void Use(GameObject player)
    {
        if (!IsSelected)
        {
            //set semua active skill lain (kalo ada) jadi unselected
            foreach(SkillSO skill in player.GetComponent<PlayerSkills>().skills)
            {
                if(skill is ActiveSkillSO temp) temp.IsSelected = false;
            }
            IsSelected = true;
            OnSelected();
        }
        else
        {
            IsSelected = false;
            OffSelected();
        }

        //ApplyBuff(player);
    }
}

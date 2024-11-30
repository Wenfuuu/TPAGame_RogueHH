using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealSkillSO", menuName = "Skill/Buff Skill/Heal")]
public class HealSkillSO : BuffSkillSO
{
    public GameObject healPrefab;

    int healAmount;//10% of player MaxHP

    public override void ApplyBuff(GameObject player)
    {
        int maxhp = player.GetComponent<PlayerDamageable>().playerStats.MaxHP;
        int currhp = player.GetComponent<PlayerDamageable>().playerStats.CurrentHP;
        healAmount = Mathf.RoundToInt(maxhp * 0.1f);
        healAmount = Mathf.Min(healAmount, maxhp - currhp);
        player.GetComponent<PlayerDamageable>().IncreaseHealth(healAmount);
    }

    public override void Use(GameObject player)
    {
        if (!IsActive)
        {
            player.GetComponent<PlayerSkills>().healEffect.SetActive(false);
            return;
        }

        if (CurrentCooldown == 0)
        {
            OnCooldown();
            CurrentCooldown = Cooldown;
            CurrentDuration = Duration;
        }

        ApplyBuff(player);
        if(CurrentDuration <= 0)
        {
            IsActive = false;
            Debug.Log("deactivating heal effect");
            player.GetComponent<PlayerSkills>().healEffect.SetActive(false);
        }
        else
        {
            //show effect
            player.GetComponent<PlayerSkills>().healEffect.SetActive(true);
        }

        //show heal text
        HealPopUpGenerator.Instance.CreatePopUp(healAmount, player.transform.position);
    }

}

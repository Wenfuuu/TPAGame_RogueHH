using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    public List<SkillSO> skills = new List<SkillSO>();
    public GameObject healEffect;

    int selectedIndex = -1;
    int level;

    private void Start()
    {
        level = gameObject.GetComponent<PlayerDamageable>().playerStats.PlayerLevel;
        //cek semua yang unlocked
        foreach (SkillSO skill in skills)
        {
            if (level >= skill.UnlockLevel)
            {
                //set active false overlay sama text
                skill.Unlock();
            }
            else skill.Lock();

            //bikin semua ready
            skill.CurrentCooldown = 0;
            skill.IsReady = true;
            if (skill is BuffSkillSO temp) temp.IsActive = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        selectedIndex = -1;
        level = gameObject.GetComponent<PlayerDamageable>().playerStats.PlayerLevel;
        foreach (SkillSO skill in skills)
        {
            if (level >= skill.UnlockLevel)
            {
                //set active false overlay sama text
                skill.Unlock();
            }
            else skill.Lock();

            if (!skill.IsReady)
            {
                skill.OnCooldown();
                if(skill is BuffSkillSO temp && temp.CurrentDuration > -1) temp.OnDuration();
            }
        }

        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString())) // Detect number key press
            {
                selectedIndex = i - 1; // Convert to zero-based index
            }
        }
        if(selectedIndex < 0 || selectedIndex >= skills.Count) return;

        if (skills[selectedIndex].IsReady && !skills[selectedIndex].IsLocked)// lagi cooldown ga dijalanin
        {
            //use skill
            if (skills[selectedIndex] is BuffSkillSO temp)
            {
                temp.IsActive = true;
                StartCoroutine(HandleBuff(selectedIndex));
            }
            else skills[selectedIndex].Use(gameObject);
        }
        //reset
        selectedIndex = -1;
    }

    IEnumerator HandleBuff(int selectedIndex)
    {
        gameObject.GetComponent<PlayerStateMachine>()._animator.SetBool("UsingSkill", true);
        skills[selectedIndex].Use(gameObject);
        yield return new WaitForSeconds(0.75f);
        gameObject.GetComponent<PlayerStateMachine>()._animator.SetBool("UsingSkill", false);
    }

    public void ReduceCooldown()
    {
        if (GameManager.Instance.executing) return;

        foreach (SkillSO skill in skills)
        {
            if (skill.CurrentCooldown == 0) continue;

            skill.CurrentCooldown--;
            if(skill.CurrentCooldown == 0)
            {
                skill.OffCooldown();
            }
        }
    }

    public void ApplyAllBuff()
    {
        if (GameManager.Instance.executing) return;

        foreach (SkillSO skill in skills)
        {
            if(skill is BuffSkillSO temp)
            {
                if (temp.IsActive)
                {
                    temp.Use(gameObject);
                    temp.CurrentDuration--;
                }
                if (temp.CurrentDuration <= 0)
                {
                    Debug.Log("removing duration");
                    temp.OffDuration();
                }
            }
        }
    }

    public void CheckActiveSkills()
    {
        foreach(SkillSO skill in skills)
        {
            if(skill is ActiveSkillSO temp)
            {
                if (temp.IsSelected)
                {
                    temp.Attack(gameObject);
                    GetComponent<PlayerStateMachine>().IsUsingSkill = true;
                }
                else if (temp.JustUsed)
                {
                    GetComponent<PlayerStateMachine>().IsUsingSkill = false;
                    temp.FinishAttack(gameObject);
                    temp.JustUsed = false;
                }
            }
        }
    }
}

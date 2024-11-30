using System.Collections;
using System.Collections.Generic;
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
        }
        //cek semua ready
        foreach (SkillSO skill in skills)
        {
            if(skill.CurrentCooldown == 0)
            {
                skill.IsReady = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        level = gameObject.GetComponent<PlayerDamageable>().playerStats.PlayerLevel;
        foreach (SkillSO skill in skills)
        {
            if (level >= skill.UnlockLevel)
            {
                //set active false overlay sama text
                skill.Unlock();
            }
            else skill.Lock();

            if (!skill.IsReady) skill.OnCooldown();
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
            }
            skills[selectedIndex].Use(gameObject);
        }
        //reset
        selectedIndex = -1;
    }

    public void ReduceCooldown()
    {
        if (GameManager.Instance.executing) return;

        foreach (SkillSO skill in skills)
        {
            if (skill.CurrentCooldown == 0) break;
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
            }
        }
    }
}

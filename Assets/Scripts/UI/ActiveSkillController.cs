using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveSkillController : MonoBehaviour
{
    public GameObject LockOverlay;
    public GameObject LockedText;

    public GameObject CooldownOverlay;
    public GameObject CooldownTextOverlay;
    public TextMeshProUGUI CooldownText;

    public GameObject SelectedOverlay;

    public TextMeshProUGUI DescriptionText;

    public BoolEventChannel LockSkill;
    public DescEventChannel LockDesc;
    public BoolEventChannel CooldownSkill;
    public IntEventChannel UpdateCooldownText;
    public BoolEventChannel SelectedSkill;// only active (yang oren)

    private void OnEnable()
    {
        LockSkill.OnEventRaised += LockSkillOverlay;
        LockDesc.OnEventRaised += LockSkillDesc;
        CooldownSkill.OnEventRaised += CooldownSkillOverlay;
        UpdateCooldownText.OnEventRaised += UpdateCooldown;
        SelectedSkill.OnEventRaised += UpdateSelected;
    }

    private void OnDisable()
    {
        LockSkill.OnEventRaised -= LockSkillOverlay;
        LockDesc.OnEventRaised -= LockSkillDesc;
        CooldownSkill.OnEventRaised -= CooldownSkillOverlay;
        UpdateCooldownText.OnEventRaised -= UpdateCooldown;
        SelectedSkill.OnEventRaised -= UpdateSelected;
    }

    private void LockSkillDesc(bool value, string desc, int unlocklvl)
    {
        if(!value)
        {
            DescriptionText.text = desc;
        }
        else
        {
            DescriptionText.text = $"Unlocked at level {unlocklvl}";
        }
    }

    private void LockSkillOverlay(bool value)
    {
        LockOverlay.SetActive(value);
        LockedText.SetActive(value);
    }

    private void CooldownSkillOverlay(bool value)
    {
        CooldownOverlay.SetActive(value);
        CooldownTextOverlay.SetActive(value);
    }

    private void UpdateCooldown(int cooldown)
    {
        CooldownText.text = cooldown.ToString();
    }

    private void UpdateSelected(bool value)
    {
        SelectedOverlay.SetActive(value);
    }

}

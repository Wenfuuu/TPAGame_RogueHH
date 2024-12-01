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

    public BoolEventChannel LockSkill;
    public BoolEventChannel CooldownSkill;
    public IntEventChannel UpdateCooldownText;
    public BoolEventChannel SelectedSkill;// only active (yang oren)

    private void OnEnable()
    {
        LockSkill.OnEventRaised += LockSkillOverlay;
        CooldownSkill.OnEventRaised += CooldownSkillOverlay;
        UpdateCooldownText.OnEventRaised += UpdateCooldown;
        SelectedSkill.OnEventRaised += UpdateSelected;
    }

    private void OnDisable()
    {
        LockSkill.OnEventRaised -= LockSkillOverlay;
        CooldownSkill.OnEventRaised -= CooldownSkillOverlay;
        UpdateCooldownText.OnEventRaised -= UpdateCooldown;
        SelectedSkill.OnEventRaised -= UpdateSelected;
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

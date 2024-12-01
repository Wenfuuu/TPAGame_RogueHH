using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuffSkillController : MonoBehaviour
{
    public GameObject LockOverlay;
    public GameObject LockedText;

    public GameObject CooldownOverlay;
    public GameObject CooldownTextOverlay;
    public TextMeshProUGUI CooldownText;

    public GameObject DurationOverlay;
    public GameObject DurationFrame;
    public GameObject DurationTextOverlay;
    public TextMeshProUGUI DurationText;

    public BoolEventChannel LockSkill;
    public BoolEventChannel CooldownSkill;
    public IntEventChannel UpdateCooldownText;
    public BoolEventChannel DurationSkill;// only buff (yang diatas)
    public IntEventChannel UpdateDurationText;

    private void OnEnable()
    {
        LockSkill.OnEventRaised += LockSkillOverlay;
        CooldownSkill.OnEventRaised += CooldownSkillOverlay;
        UpdateCooldownText.OnEventRaised += UpdateCooldown;
        DurationSkill.OnEventRaised += DurationSkillOverlay;
        UpdateDurationText.OnEventRaised += UpdateDuration;
    }

    private void OnDisable()
    {
        LockSkill.OnEventRaised -= LockSkillOverlay;
        CooldownSkill.OnEventRaised -= CooldownSkillOverlay;
        UpdateCooldownText.OnEventRaised -= UpdateCooldown;
        DurationSkill.OnEventRaised -= DurationSkillOverlay;
        UpdateDurationText.OnEventRaised -= UpdateDuration;
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

    private void DurationSkillOverlay(bool value)
    {
        DurationOverlay.SetActive(value);
        DurationFrame.SetActive(value);
        DurationTextOverlay.SetActive(value);
    }

    private void UpdateDuration(int duration)
    {
        Debug.Log("curr duration is: " + duration);
        DurationText.text = duration.ToString();
    }
}

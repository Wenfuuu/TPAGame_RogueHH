using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownFiller : MonoBehaviour
{
    public PlayerStatsSO player;
    public EventChannel UnlockFloor;

    private TMP_Dropdown dropdown;

    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        FillDropdown();
    }

    private void OnEnable()
    {
        UnlockFloor.OnEventRaised += FillDropdown;
    }

    private void OnDisable()
    {
        UnlockFloor.OnEventRaised -= FillDropdown;
    }

    public void GetValue()
    {
        player.CurrentFloor = dropdown.value;
    }

    private void FillDropdown()
    {
        dropdown.ClearOptions();
        List<string> options = new List<string>
        {
            "Boss"
        };

        for (int i = 1; i <= player.UnlockedFloor; i++)
        {
            options.Add($"Floor {i}");
        }

        dropdown.AddOptions(options);
        dropdown.value = player.CurrentFloor;
    }
}

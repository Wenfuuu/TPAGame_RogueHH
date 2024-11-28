using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EXPController : MonoBehaviour
{
    public Slider EXPSlider;
    public TextMeshProUGUI EXPText;

    public IntEventChannel SetupEXPPlayer;
    public IntEventChannel UpdateEXPPlayer;
    public TwoIntEventChannel UpdateEXPText;

    private void OnEnable()
    {
        SetupEXPPlayer.OnEventRaised += SetupEXP;
        UpdateEXPPlayer.OnEventRaised += UpdateEXP;
        UpdateEXPText.OnEventRaised += UpdateText;
    }

    private void OnDisable()
    {
        SetupEXPPlayer.OnEventRaised -= SetupEXP;
        UpdateEXPPlayer.OnEventRaised -= UpdateEXP;
        UpdateEXPText.OnEventRaised -= UpdateText;
    }

    private void SetupEXP(int value)
    {
        EXPSlider.maxValue = value;
    }

    private void UpdateEXP(int value)
    {
        EXPSlider.value = value;
    }

    private void UpdateText(int currHP, int maxHP)
    {
        string text = $"{currHP}/{maxHP}";
        EXPText.text = text;
    }
}

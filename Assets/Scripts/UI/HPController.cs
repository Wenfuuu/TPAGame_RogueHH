using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    public Slider HPSlider;
    public TextMeshProUGUI HPText;

    public IntEventChannel SetupHPPlayer;
    public IntEventChannel UpdateHPPlayer;
    public TwoIntEventChannel UpdateHPText;

    private void OnEnable()
    {
        SetupHPPlayer.OnEventRaised += SetupHP;
        UpdateHPPlayer.OnEventRaised += UpdateHP;
        UpdateHPText.OnEventRaised += UpdateText;
    }

    private void OnDisable()
    {
        SetupHPPlayer.OnEventRaised -= SetupHP;
        UpdateHPPlayer.OnEventRaised -= UpdateHP;
        UpdateHPText.OnEventRaised -= UpdateText;
    }

    private void SetupHP(int value)
    {
        HPSlider.maxValue = value;
    }

    private void UpdateHP(int value)
    {
        HPSlider.value = value;
    }

    private void UpdateText(int currHP, int maxHP)
    {
        string text = $"{currHP}/{maxHP}";
        HPText.text = text;
    }

}

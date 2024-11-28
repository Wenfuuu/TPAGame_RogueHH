using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public TextMeshProUGUI LevelText;

    public IntEventChannel UpdateLevelPlayer;

    private void OnEnable()
    {
        UpdateLevelPlayer.OnEventRaised += UpdateLevel;
    }

    private void OnDisable()
    {
        UpdateLevelPlayer.OnEventRaised -= UpdateLevel;
    }

    private void UpdateLevel(int level)
    {
        string text = $"Level {level}";
        LevelText.text = text;
    }
}

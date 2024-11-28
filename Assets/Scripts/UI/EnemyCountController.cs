using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyCountController : MonoBehaviour
{
    public TextMeshProUGUI EnemyCountText;

    public IntEventChannel UpdateEnemyCount;

    private void OnEnable()
    {
        UpdateEnemyCount.OnEventRaised += UpdateEnemy;
    }

    private void OnDisable()
    {
        UpdateEnemyCount.OnEventRaised -= UpdateEnemy;
    }

    private void UpdateEnemy(int enemyCount)
    {
        string text = $"Enemy left: {enemyCount}";
        EnemyCountText.text = text;
    }
}

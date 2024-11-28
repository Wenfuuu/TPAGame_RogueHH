using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamageable : MonoBehaviour
{
    public EnemyStats enemyStats;
    public Slider HPSlider;

    public string Type;

    // Start is called before the first frame update
    void Start()
    {
        enemyStats = EnemyFactory.CreateEnemy(Type);
        HPSlider.maxValue = enemyStats.MaxHP;
        HPSlider.value = enemyStats.CurrentHP;
    }

    public void DecreaseHealth(int damage)
    {
        enemyStats.DecreaseHealth(damage);
        HPSlider.value = enemyStats.CurrentHP;
    }
}

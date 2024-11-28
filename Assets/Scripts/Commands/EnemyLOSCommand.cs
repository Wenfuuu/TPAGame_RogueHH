using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLOSCommand : ICommand
{
    private EnemyStateMachine _enemy;

    public EnemyLOSCommand(EnemyStateMachine enemy)
    {
        _enemy = enemy;
    }

    public void Execute()
    {

    }

    public IEnumerator ExecuteCoroutine()
    {
        if (_enemy.gameObject.GetComponent<EnemyDamageable>().enemyStats.CurrentHP <= 0) yield break;

        Debug.Log("LOS woi");
        yield return _enemy.CheckLOS();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveCommand : ICommand
{
    private EnemyStateMachine _enemy;

    public EnemyMoveCommand(EnemyStateMachine enemy)
    {
        _enemy = enemy;
    }

    public void Execute()
    {

    }

    public IEnumerator ExecuteCoroutine()
    {
        if (_enemy == null) yield break;

        yield return _enemy.MoveToPlayer();
    }
}

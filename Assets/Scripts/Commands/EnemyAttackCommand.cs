using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCommand : ICommand
{
    private EnemyStateMachine _enemy;

    public EnemyAttackCommand(EnemyStateMachine enemy)
    {
        _enemy = enemy;
    }

    public void Execute()
    {

    }

    public IEnumerator ExecuteCoroutine()
    {
        _enemy._animator.SetBool("IsAttacking", true);
        yield return _enemy.AttackPlayer();
        _enemy._animator.SetBool("IsAttacking", false);
    }
}

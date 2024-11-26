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
        yield return new WaitForSeconds(0.3f);
        _enemy.PlayerRotate();
        _enemy.GetPlayer._animator.SetBool("IsHit", true);
        yield return _enemy.AttackPlayer();
        _enemy._animator.SetBool("IsAttacking", false);
        _enemy.GetPlayer._animator.SetBool("IsHit", false);
    }
}

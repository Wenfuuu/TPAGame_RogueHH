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

        int damage = CalculateDamage();
        _enemy.GetPlayer.gameObject.GetComponent<PlayerDamageable>().DecreaseHealth(damage);

        DamagePopUpGenerator.Instance.CreatePopUp(damage, false, _enemy.GetPlayer.transform.position);
        yield return _enemy.AttackPlayer();
        _enemy._animator.SetBool("IsAttacking", false);
        _enemy.GetPlayer._animator.SetBool("IsHit", false);
    }

    int CalculateDamage()
    {
        float defenseScalingFactor = Random.Range(50, 101);
        PlayerDamageable damageable = _enemy.GetPlayer.gameObject.GetComponent<PlayerDamageable>();
        float defense = damageable.playerStats.Defense;
        float defenseFactor = 1f - (defense / (defense + defenseScalingFactor));
        return Mathf.RoundToInt(80 * defenseFactor);
    }
}

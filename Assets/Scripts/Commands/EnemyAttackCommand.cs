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
        if(_enemy == null) yield break;

        _enemy._animator.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(0.3f);
        _enemy.PlayerRotate();
        _enemy.GetPlayer._animator.SetBool("IsHit", true);
        SFXManager.Instance.PlayRandomSFX(Sounds.Instance.GruntSFX, _enemy.GetPlayer.transform, 1f);

        int damage = CalculateDamage();

        bool crit = false;
        float critDamage = 100f;
        int randomVal = Random.Range(1, 101);
        if (randomVal <= _enemy.gameObject.GetComponent<EnemyDamageable>().enemyStats.CritRate) crit = true;
        if (crit)
        {
            critDamage = _enemy.gameObject.GetComponent<EnemyDamageable>().enemyStats.CritDamage;
            damage += Mathf.RoundToInt(damage * (critDamage / 100f));
            CameraShake.Instance.Shake();
            SFXManager.Instance.PlayRandomSFX(Sounds.Instance.CriticalSFX, _enemy.GetPlayer.transform, 1f);
        }
        _enemy.GetPlayer.gameObject.GetComponent<PlayerDamageable>().DecreaseHealth(damage);
        SFXManager.Instance.PlayRandomSFX(Sounds.Instance.PunchSFX, _enemy.GetPlayer.transform, 1f);
        DamagePopUpGenerator.Instance.CreatePopUp(damage, crit, _enemy.GetPlayer.transform.position);
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
        int atk = _enemy.gameObject.GetComponent<EnemyDamageable>().enemyStats.Attack;

        return Mathf.RoundToInt(atk * defenseFactor);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroState : EnemyBaseState
{
    public EnemyAggroState(EnemyStateMachine context, EnemyStateFactory states) : base(context, states)
    {
    }

    public override void EnterState()
    {
        //Debug.Log("enemy aggro");
        _context._animator.SetBool("IsAggro", true);
        _context.GetComponent<EnemyIndicator>().OnAggro();
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        if (_context.IsNearPlayer)
        {
            SwitchStates(_states.ReadyToAttack());
        }
        if (_context.IsMoving)
        {
            SwitchStates(_states.Run());
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyStateMachine context, EnemyStateFactory states) : base(context, states)
    {
    }

    public override void EnterState()
    {
        //Debug.Log("enemy idle");
        _context._animator.SetBool("IsMoving", false);
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        if (_context.IsAggro)
        {
            SwitchStates(_states.Aggro());
        }
    }
}

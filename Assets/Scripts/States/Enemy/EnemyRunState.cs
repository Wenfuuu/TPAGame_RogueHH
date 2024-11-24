using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRunState : EnemyBaseState
{
    public EnemyRunState(EnemyStateMachine context, EnemyStateFactory states) : base(context, states)
    {
    }

    public override void EnterState()
    {
        //Debug.Log("enemy running");
        _context._animator.SetBool("IsMoving", true);
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        if (!_context.IsMoving)
        {
            SwitchStates(_states.Aggro());
        }
    }
}

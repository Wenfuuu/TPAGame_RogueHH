using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReadyState : EnemyBaseState
{
    public EnemyReadyState(EnemyStateMachine context, EnemyStateFactory states) : base(context, states)
    {
    }

    public override void EnterState()
    {
        Debug.Log("enemy entering rta");
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        if (!_context.IsNearPlayer)
        {
            SwitchStates(_states.Aggro());
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class EnemyAlertState : EnemyBaseState
{
    public EnemyAlertState(EnemyStateMachine context, EnemyStateFactory states) : base(context, states)
    {
    }

    public override void EnterState()
    {
        Debug.Log("enemy entering alert");
        _context._animator.SetBool("IsAggro", true);
        _context.GetComponent<EnemyIndicator>().OnAlert();
    }

    public override void ExitState()
    {
        _context._animator.SetBool("IsAggro", false);
    }

    public override void UpdateState()
    {
        if (_context.IsAggro)
        {
            _context.GetComponent<EnemyIndicator>().OffAlert();
            SwitchStates(_states.Aggro());
        }
        if(!_context.IsAlert)
        {
            ExitState();
            SwitchStates(_states.Idle());
        }
    }
}

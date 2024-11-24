using Unity.VisualScripting;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentState, PlayerStateFactory states) : base(currentState, states)
    {
    }

    public override void EnterState()
    {
        //Debug.Log("idle");
        _context._animator.SetBool("IsMoving", false);
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        if(_context.IsMoving)
        {
            SwitchStates(_states.Run());
        }
    }

    void OnCollisionEnter(Collider other)
    {

    }
}
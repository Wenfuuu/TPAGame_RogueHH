using UnityEngine;
public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentState, PlayerStateFactory states) : base(currentState, states)
    {
    }

    public override void EnterState()
    {
        Debug.Log("running");
        _context._animator.SetBool("IsMoving", true);
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        if (!_context.IsMoving)
        {
            SwitchStates(_states.Idle());
        }
    }
}
using UnityEngine;

public abstract class PlayerBaseState
{
    public PlayerStateMachine _context;
    public PlayerStateFactory _states;

    public PlayerBaseState(PlayerStateMachine context, PlayerStateFactory states)
    {
        _context = context;
        _states = states;
    }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();

    public void UpdateStates()
    {
        UpdateState();
    }

    protected void SwitchStates(PlayerBaseState newState)
    {
        ExitState();
        newState.EnterState();

        _context.CurrentState = newState;
    }

}
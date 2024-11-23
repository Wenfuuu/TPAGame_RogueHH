public abstract class EnemyBaseState
{
    public EnemyStateMachine _context;
    public EnemyStateFactory _states;

    public EnemyBaseState(EnemyStateMachine context, EnemyStateFactory states)
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

    protected void SwitchStates(EnemyBaseState newState)
    {
        ExitState();
        newState.EnterState();

        _context.CurrentState = newState;
    }
}

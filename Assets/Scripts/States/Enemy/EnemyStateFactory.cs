public class EnemyStateFactory
{
    EnemyStateMachine _context;

    public EnemyStateFactory(EnemyStateMachine context)
    {
        _context = context;
    }

    public EnemyBaseState Idle()
    {
        return new EnemyIdleState(_context, this);
    }

    public EnemyBaseState Run()
    {
        return new EnemyRunState(_context, this);
    }

    public EnemyBaseState Aggro()
    {
        return new EnemyAggroState(_context, this);
    }
}

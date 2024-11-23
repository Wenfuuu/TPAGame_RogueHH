using System.Collections.Generic;

public class TurnInvoker
{
    private Queue<ICommand> turnsQueue = new Queue<ICommand>();

    public void AddTurn(ICommand command)
    {
        turnsQueue.Enqueue(command);
    }

    //public void ExecuteNextTurn()
    //{
    //    if (turnsQueue.Count > 0)
    //    {
    //        ICommand command = turnsQueue.Dequeue();
    //        command.Execute();
    //    }
    //}

    public void ExecuteAll()
    {
        while (turnsQueue.Count > 0)
        {
            ICommand command = turnsQueue.Dequeue();
            command.Execute();
        }
    }

    public bool IsTurnQueueEmpty()
    {
        return turnsQueue.Count == 0;
    }
}

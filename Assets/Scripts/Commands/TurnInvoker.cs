using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnInvoker
{
    private Queue<ICommand> turnsQueue = new Queue<ICommand>();

    public void AddTurn(ICommand command)
    {
        turnsQueue.Enqueue(command);
    }

    public IEnumerator ExecuteAllCoroutines()
    {
        while (turnsQueue.Count > 0)
        {
            Debug.Log("queue count: " +  turnsQueue.Count);
            ICommand command = turnsQueue.Dequeue();
            yield return command.ExecuteCoroutine();
            yield return new WaitForSeconds(0.1f);
        }

        GameManager.isEnemyTurn = false;
    }

    public int GetTurnCount()
    {
        return turnsQueue.Count;
    }

    public void ClearCommand()
    {
        turnsQueue.Clear();
    }
}

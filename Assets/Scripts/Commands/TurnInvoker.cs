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
            //if (command is EnemyMoveCommand enemyMoveCommand)
            //{
            //    // Execute the coroutine for enemy movement
            //    yield return enemyMoveCommand.ExecuteCoroutine();
            //    yield return new WaitForSeconds(0.1f);// delay buat prevent collision enemy
            //}else if(command is EnemyAttackCommand enemyAttackCommand)
            //{
            //    yield return enemyAttackCommand.ExecuteCoroutine();
            //    yield return new WaitForSeconds(0.1f);
            //}
        }

        GameManager.isEnemyTurn = false;
    }

    public int GetTurnCount()
    {
        return turnsQueue.Count;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private TurnInvoker invoker;

    //private bool isPlayerTurn = true;

    public List<EnemyStateMachine> enemies;
    private PlayerStateMachine player;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            invoker = new TurnInvoker();
            player = PlayerStateMachine.Instance;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        CheckTurn();

        invoker.ExecuteAll();
    }

    private void CheckTurn()// bakal ngecek skrng turn player / enemies (plural)
    {
        // cek player idle / alerted (dikejar) / in battle / ready to attack
        if (invoker.IsTurnQueueEmpty() && !player.IsMoving)// kalo empty brti turn enemy
        {
            StartEnemyTurn();// masukin semua command enemy ke invoker
        }
    }

    //private void StartPlayerTurn()
    //{
    //    Debug.Log("Player's Turn");
    //    isPlayerTurn = false;
    //}

    private void StartEnemyTurn()
    {
        foreach (EnemyStateMachine enemy in enemies)
        {
            //Debug.Log("aggro enemy di pos: " + enemy.transform.position + " " + enemy.IsAggro);
            if (enemy.IsAggro)
            {
                Debug.Log("Enemy's Turn");
                EnemyMoveCommand moveCommand = new EnemyMoveCommand(enemy);
                invoker.AddTurn(moveCommand);
                //invoker.ExecuteNextTurn();
            }
        }

        //isPlayerTurn = true;
    }

    public void QueueCommand(ICommand command)
    {
        if (command is PlayerMoveCommand moveCommand)
        {
            Debug.Log("langsung exec");
            moveCommand.Execute(); // Execute immediately
        }
        else invoker.AddTurn(command);
    }
}

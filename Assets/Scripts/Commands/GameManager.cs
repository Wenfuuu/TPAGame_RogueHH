using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private TurnInvoker invoker;

    public static bool isEnemyTurn = false;
    //public static bool isPlayerTurn = true;
    private bool executing = false;

    private List<EnemyStateMachine> enemies = new List<EnemyStateMachine>();
    private HashSet<EnemyStateMachine> aggroEnemies = new HashSet<EnemyStateMachine>();
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

        //Debug.Log("turn count is: " + invoker.GetTurnCount());
    }

    private void CheckTurn()// bakal ngecek skrng turn player / enemies
    {
        //if(isEnemyTurn)
        //{
        //    Debug.Log("turn enemy");
        //}
        //else
        //{
        //    Debug.Log("turn player");
        //}
        if (!isEnemyTurn) return;

        if (!player.IsMoving && isEnemyTurn)// kalo player ga gerak & lagi turn enemy
        {
            if (executing) return;
            //StartEnemyTurn();// masukin semua command enemy ke invoker
            AddEnemyCommand();
            StartCoroutine(StartEnemyTurn());
        }
    }

    private void AddEnemyCommand()
    {
        executing = true;
        foreach (EnemyStateMachine enemy in aggroEnemies)
        {
            if (!enemy.IsNearPlayer)
            {
                Debug.Log("giving move command to enemy");
                EnemyMoveCommand moveCommand = new EnemyMoveCommand(enemy);
                invoker.AddTurn(moveCommand);
            }
            else
            {
                Debug.Log("giving attack command to enemy");
                EnemyAttackCommand atkCommand = new EnemyAttackCommand(enemy);
                invoker.AddTurn(atkCommand);
            }
        }
    }

    private IEnumerator StartEnemyTurn()
    {
        //Debug.Log("turn count is: " + invoker.GetTurnCount());
        // Execute all enemy commands sequentially
        yield return invoker.ExecuteAllCoroutines();

        executing = false;
    }

    public void QueueCommand(ICommand command)
    {
        if (command is PlayerMoveCommand moveCommand)
        {
            //Debug.Log("langsung exec");
            moveCommand.Execute(); // Execute immediately
        }
        //else invoker.AddTurn(command);
    }

    public void AddEnemy(EnemyStateMachine enemy)
    {
        enemies.Add(enemy);
    }

    public void AddAggro(EnemyStateMachine enemy)
    {
        aggroEnemies.Add(enemy);
    }

    public bool CheckAggro()
    {
        return (aggroEnemies.Count > 0);
    }

    public HashSet<EnemyStateMachine> getAggroEnemies()
    {
        return aggroEnemies;
    }
}

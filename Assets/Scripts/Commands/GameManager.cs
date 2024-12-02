using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private TurnInvoker invoker;

    public static bool isEnemyTurn = false;
    //public static bool isPlayerTurn = true;
    [HideInInspector] public bool executing = false;

    public GameObject GameOver;
    public GameObject FloorClear;

    private List<EnemyStateMachine> enemies = new List<EnemyStateMachine>();
    private HashSet<EnemyStateMachine> aggroEnemies = new HashSet<EnemyStateMachine>();
    private PlayerStateMachine player;
    private bool dead = false;
    private bool clear = false;

    public IntEventChannel UpdateEnemyCount;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            invoker = new TurnInvoker();
            player = PlayerStateMachine.Instance;
            GameOver.SetActive(false);
            FloorClear.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //private void Start()
    //{
    //    UpdateFloorText.RaiseEvent(PlayerStateMachine.Instance.gameObject.GetComponent<PlayerFloor>().playerStats.CurrentFloor);
    //}

    private void Update()
    {
        if(player.GetComponent<PlayerDamageable>().playerStats.CurrentHP <= 0)
        {
            if (dead) return;
            dead = true;
            invoker.ClearCommand();
            LockPlayerInput(true);
            StartCoroutine(PlayerDeath());
            // show gameover
            //GameOver.SetActive(true);
        }

        if(GetEnemyCount() == 0)// win and go to next floor
        {
            if (clear) return;
            clear = true;
            NextFloor();
            LockPlayerInput(true);
            // show floor clear
            FloorClear.SetActive(true);
        }

        CheckTurn();
    }

    private IEnumerator PlayerDeath()
    {
        SFXManager.Instance.PlayRandomSFX(Sounds.Instance.DeathSFX, player.transform, 1f);
        player._animator.SetBool("IsDead", true);
        yield return new WaitForSeconds(2f);
        GameOver.SetActive(true);
    }

    private void LockPlayerInput(bool locked)
    {
        if (player != null)
        {
            var controller = player.GetComponent<PlayerStateMachine>();
            if (controller != null)
            {
                controller.enabled = !locked;
            }
        }
    }

    public void NextFloor()
    {
        player.gameObject.GetComponent<PlayerFloor>().IncreaseFloor();
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
            if(enemy.IsAggro)
            {
                Debug.Log("uda msk aggro");
            }

            if (!enemy.IsNearPlayer && enemy.IsAggro)
            {
                Debug.Log("giving move command to enemy");
                EnemyMoveCommand moveCommand = new EnemyMoveCommand(enemy);
                invoker.AddTurn(moveCommand);
            }
            else if (enemy.IsAlert && !enemy.IsAggro)
            {
                Debug.Log("giving LOS command to enemy");
                EnemyLOSCommand LOSCommand = new EnemyLOSCommand(enemy);
                invoker.AddTurn(LOSCommand);
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
            moveCommand.Execute();
        }
    }

    public void AddEnemy(EnemyStateMachine enemy)
    {
        enemies.Add(enemy);
    }

    public void AddAggro(EnemyStateMachine enemy)
    {
        aggroEnemies.Add(enemy);
    }

    public void EnemyDeath(EnemyStateMachine enemy)
    {
        enemies.Remove(enemy);
        aggroEnemies.Remove(enemy);

        Destroy(enemy.gameObject);
    }

    public void EnemyOutOfRange(EnemyStateMachine enemy)
    {
        aggroEnemies.Remove(enemy);
    }

    public int GetEnemyCount()
    {
        return enemies.Count;
    }

    public bool CheckAggro()
    {
        return (aggroEnemies.Count > 0);
    }

    public HashSet<EnemyStateMachine> getAggroEnemies()
    {
        return aggroEnemies;
    }

    public bool CheckBattle()
    {
        foreach (EnemyStateMachine enemy in aggroEnemies)
        {
            if (enemy.IsAggro)
            {
                return true;
            }
        }
        return false;
    }
}

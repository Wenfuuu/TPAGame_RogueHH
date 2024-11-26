using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStateMachine : MonoBehaviour
{
    private PlayerStateMachine player;
    public PathFinding pathfinding;

    private Vector3 lastPosition;
    public float moveSpeed = 1f;

    private List<Node> path;

    private int currentTargetIndex = 0;

    public Animator _animator;

    // States variables
    EnemyBaseState _currentState;
    EnemyStateFactory _states;

    private bool _isMoving = false;
    private bool _isAggro = false;
    private bool _isNearPlayer = false;

    public EnemyBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public bool IsMoving { get { return _isMoving; } }
    public bool IsAggro {  get { return _isAggro; } }
    public bool IsNearPlayer {  get { return _isNearPlayer; } }
    public PlayerStateMachine GetPlayer { get { return player; } }

    private void Awake()
    {
        player = PlayerStateMachine.Instance;
        pathfinding = PathFinding.Instance;

        // Get animator component
        _animator = GetComponent<Animator>();

        // Setup states
        _states = new EnemyStateFactory(this);
        _currentState = _states.Idle();
        _currentState.EnterState();

        GameManager.Instance.AddEnemy(this);
    }

    void Update()
    {
        Debug.Log("ready atk: " + _isNearPlayer);
        // jika dkt player jadi aggro & ngejar
        CheckPlayer();
        CheckReadyToAttack();

        _currentState.UpdateStates();
    }

    void CheckReadyToAttack()
    {
        if (_isMoving) return;

        _isNearPlayer = false;
        Vector3 currPos = transform.position;

        float distance = Vector3.Distance(currPos, player.transform.position);

        // Set _isNearPlayer to true if the enemy is within range to attack
        if (distance <= 2.2f) // Adjust the distance threshold as needed (e.g., 2 units)
        {
            _isNearPlayer = true;
            HandleRotation(player.transform.position);
        }
        //if(_isNearPlayer) Debug.Log("distance to player: " + distance);
    }

    void CheckPlayer()
    {
        //Debug.Log("cek player di: " + player.transform.position);
        if (_isAggro) return;
        Vector3 currPos = transform.position;
        for(int i = -4; i <= 4; i+=2)
        {
            for(int j = -4; j <= 4; j+=2)
            {
                if (i == 0 && j == 0) continue;

                Vector3 checkPos = new Vector3(currPos.x + i, 1, currPos.z + j);
                //Debug.Log("detecting player di: " + checkPos);
                if (checkPos == player.transform.position)
                {
                    _isAggro = true;
                    GameManager.Instance.AddAggro(this);
                    //Debug.Log("adding aggro");
                }
            }
            if (_isAggro) break;
        }
    }

    public IEnumerator AttackPlayer()
    {
        //Debug.Log("attacking player");
        yield return new WaitForSeconds(0.5f);
        //bikin animasi player gethit
        //player._animator.SetBool("IsHit", true);
    }

    public void PlayerRotate()
    {
        Vector3 direction = (transform.position - player.transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            player.transform.rotation = targetRotation;
        }
    }

    public IEnumerator MoveToPlayer()
    {
        if (!IsAggro) yield break;

        Vector3 targetPosition = player.transform.position;
        Node dest = pathfinding.grid.NodeFromWorldPoint(targetPosition);

        if (dest.isWalkable)
        {
            path = pathfinding.FindPath(transform.position, targetPosition);

            if (path != null && path.Count > 1)// klo 1 brti uda dkt player jadi skip
            {
                yield return StartCoroutine(FollowPath());
            }
        }
        HandleRotation(player.transform.position);
    }

    IEnumerator FollowPath()
    {
        //Debug.Log("following player");
        if (_isMoving) yield break;

        _isMoving = true;
        currentTargetIndex = 0;

        if (currentTargetIndex < path.Count)// berhenti 1 tile sblm player
        {
            //Debug.Log("curr idx: " + currentTargetIndex);
            Node targetNode = path[currentTargetIndex];
            Vector3 targetPosition = targetNode.worldPosition;

            //cek next walkable (ada enemy jadi unwalkable)
            Node temp = Grid.Instance.NodeFromWorldPoint(targetPosition);
            if (!temp.isWalkable)
            {
                _isMoving = false;
                yield break;
            }

            targetPosition.y = 1;
            Vector3 currentPosition = transform.position;
            HandleRotation(targetPosition);

            float travelPercent = 0f;
            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(currentPosition, targetPosition, travelPercent);
                yield return null;
            }

            currentTargetIndex++;
        }

        path.Clear(); // Clear the path once the target is reached
        _isMoving = false;
    }

    void HandleRotation(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = targetRotation;
        }
    }

}

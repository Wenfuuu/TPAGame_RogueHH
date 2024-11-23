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

    public float attackTime = 1f;

    // States variables
    EnemyBaseState _currentState;
    EnemyStateFactory _states;

    private bool _isMoving = false;
    private bool _isAggro = false;

    public EnemyBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public bool IsMoving { get { return _isMoving; } }
    public bool IsAggro {  get { return _isAggro; } }

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

        GameManager.Instance.enemies.Add(this);
    }

    void Update()
    {
        _currentState.UpdateStates();
        // jika dkt player jadi aggro & ngejar
        CheckPlayer();
    }

    void CheckPlayer()
    {
        //Debug.Log("cek player di: " + player.transform.position);
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
                }
            }
            if (_isAggro) break;
        }
    }

    public void MoveToPlayer()
    {
        Vector3 targetPosition = player.transform.position;
        Node dest = pathfinding.grid.NodeFromWorldPoint(targetPosition);
        Debug.Log("moving to player");
        //if (dest.isWalkable)
        //{
            pathfinding.FindPath(transform.position, targetPosition);
            path = pathfinding.grid.path;

            if (path != null && path.Count > 0)
            {
                StartCoroutine(FollowPath());
            }
        //}
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

    IEnumerator FollowPath()
    {
        Debug.Log("following player");
        _isMoving = true;
        currentTargetIndex = 0;

        while (currentTargetIndex < path.Count - 1)// berhenti 1 tile sblm player
        {
            //Debug.Log("curr idx: " + currentTargetIndex);
            Node targetNode = path[currentTargetIndex];
            Vector3 targetPosition = targetNode.worldPosition;
            targetPosition.y = 1;

            // Calculate the direction needed to move on X and Z
            Vector3 currentPosition = transform.position;

            // handle rotation
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
}

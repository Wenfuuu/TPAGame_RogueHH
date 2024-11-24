using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateMachine : MonoBehaviour
{
    public static PlayerStateMachine Instance;

    public Camera cam;
    public PathFinding pathfinding;

    private Vector3 lastPosition;
    public LayerMask mask;
    public float moveSpeed = 5f;

    private List<Node> path;
    private List<Node> highlightPath;
    private int currentTargetIndex = 0;
    private bool isClicked = false;
    private GameManager manager;

    public Animator _animator;

    public float attackTime = 1f;

    // States variables
    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    private bool _isMoving = false;

    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public bool IsMoving { get { return _isMoving; } }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Get animator component
        _animator = GetComponent<Animator>();

        // Setup states
        _states = new PlayerStateFactory(this);
        _currentState = _states.Idle();
        _currentState.EnterState();
    }

    void Update()
    {
        _currentState.UpdateStates();
        manager = GameManager.Instance;

        if (Input.GetKeyDown(KeyCode.Space))// skip turn
        {
            Debug.Log("skipping turn");
            GameManager.isEnemyTurn = true;
        }

        if (Input.mousePosition != null && !_isMoving)// show highlight path
        {
            Vector3 startPos = transform.position;
            Vector3 targetPos = GetMapPosition();

            Node dest = pathfinding.grid.NodeFromWorldPoint(targetPos);
            //Debug.Log(dest.isWalkable);
            if (dest.isWalkable == true)
            {
                highlightPath = pathfinding.FindPath(startPos, targetPos);

                if (highlightPath != null && !isClicked)
                {
                    ChangeColor();
                }
            }
        }
        else if(Input.mousePosition != null && _isMoving)// highlight 1 tile
        {
            ChangeOneColor();
        }

        if (Input.GetMouseButtonDown(0) && _isMoving)
        {
            currentTargetIndex = path.Count;
        }

        if (GameManager.isEnemyTurn) return;
        if (Input.GetMouseButtonDown(0) && !_isMoving)// implement command
        {
            //Vector3 startPos = transform.position;
            Vector3 targetPos = GetMapPosition();

            Node dest = pathfinding.grid.NodeFromWorldPoint(targetPos);
            targetPos.y = 1;
            Vector3 currPos = new Vector3();
            currPos.x = Mathf.RoundToInt(transform.position.x);
            currPos.y = Mathf.RoundToInt(transform.position.y);
            currPos.z = Mathf.RoundToInt(transform.position.z);
            targetPos.x = Mathf.RoundToInt(targetPos.x);
            targetPos.y = Mathf.RoundToInt(targetPos.y);
            targetPos.z = Mathf.RoundToInt(targetPos.z);

            if (dest.isWalkable && (currPos != targetPos))
            {
                PlayerMoveCommand moveCommand = new PlayerMoveCommand(this, targetPos);
                manager.QueueCommand(moveCommand);

                // kalo ada enemy aggro & player brsn gerak
                if (manager.CheckAggro())
                {
                    GameManager.isEnemyTurn = true;
                    Debug.Log("ada enemy aggro");
                }
            }
        }
    }

    public void MoveTo(Vector3 targetPosition)
    {
        Node dest = pathfinding.grid.NodeFromWorldPoint(targetPosition);
        if (dest.isWalkable)
        {
            path = pathfinding.FindPath(transform.position, targetPosition);

            if (path != null && path.Count > 0)
            {
                StartCoroutine(FollowPath());
            }
        }
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
        _isMoving = true;
        currentTargetIndex = 0;

        while (currentTargetIndex < path.Count)
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

        isClicked = false;
    }

    void ChangeOneColor()
    {
        //Debug.Log("Changing one tile");
        Vector3 tilePos = GetMapPosition();
        Node tileTarget = pathfinding.grid.NodeFromWorldPoint(tilePos);

        // Reset all materials
        foreach (Node node in pathfinding.grid.grid)
        {
            if (node == null) continue;

            Collider[] colliders = Physics.OverlapSphere(node.worldPosition, 0.1f);
            if (colliders.Length == 0) continue; // Skip if no colliders are found.

            GameObject tile = colliders[0].gameObject;
            tile.GetComponentInChildren<Renderer>().material.color = Color.gray;
        }

        // Change one tile
        Collider[] collide = Physics.OverlapSphere(tileTarget.worldPosition, 0.1f);
        foreach (Collider col in collide)
        {
            GameObject target = col.gameObject;
            target.GetComponentInChildren<Renderer>().material.color = Color.white;
        }
    }

    void ChangeColor()
    {
        // Reset all materials
        foreach (Node node in pathfinding.grid.grid)
        {
            if (node == null) continue;

            Collider[] colliders = Physics.OverlapSphere(node.worldPosition, 0.1f);
            if (colliders.Length == 0) continue; // Skip if no colliders are found.

            GameObject tile = colliders[0].gameObject;
            tile.GetComponentInChildren<Renderer>().material.color = Color.gray;
        }

        // Change material for the path
        foreach (Node node in highlightPath)
        {
            Collider[] colliders = Physics.OverlapSphere(node.worldPosition, 0.1f);
            if (colliders.Length > 0)
            {
                GameObject tile = colliders[0].gameObject;
                if (tile != null)
                {
                    tile.GetComponentInChildren<Renderer>().material.color = Color.white;
                }
            }
        }
    }

    public Vector3 GetMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, mask))
        {
            lastPosition = hit.point;
            lastPosition.y = 0;
        }

        return lastPosition;
    }
}

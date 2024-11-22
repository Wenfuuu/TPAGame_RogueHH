using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    public Camera cam;
    public PathFinding pathfinding;

    public Transform player;
    private Vector3 lastPosition;
    public LayerMask mask;
    public float moveSpeed = 5f;

    private List<Node> path;
    private bool isClicked;
    private int currentTargetIndex = 0;

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
        foreach (Node node in path)
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
    void Update()
    {
        //GetMapPosition();
        if (Input.mousePosition != null)
        {
            Vector3 startPos = player.position;
            Vector3 targetPos = GetMapPosition();

            pathfinding.FindPath(startPos, targetPos);
            path = pathfinding.grid.path;

            if (path != null)
            {
                ChangeColor();
            }
        }

        HandleClick();

        //nanti tambahin state
        if(path.Count > 0 && isClicked)
        {
            FollowPath();
        }
    }

    void FollowPath()
    {
        if (currentTargetIndex < path.Count)
        {
            Node targetNode = path[currentTargetIndex];
            Vector3 targetPosition = targetNode.worldPosition; // Ensure this is the tile's center.

            // Calculate the direction needed to move on X and Z
            Vector3 currentPosition = player.position;
            Vector3 movementDirection = Vector3.zero;

            // Prioritize X-axis movement
            if (Mathf.Abs(targetPosition.x - currentPosition.x) > 0.01f) // Use a small threshold for precision
            {
                movementDirection = new Vector3(targetPosition.x - currentPosition.x, 0, 0).normalized;
            }
            // If X-axis movement is complete, move along Z-axis
            else if (Mathf.Abs(targetPosition.z - currentPosition.z) > 0.01f)
            {
                movementDirection = new Vector3(0, 0, targetPosition.z - currentPosition.z).normalized;
            }

            // Move the player only in the calculated direction
            player.position += movementDirection * moveSpeed * Time.deltaTime;

            // Check if the player has reached the target position (center of the tile)
            if (Vector3.Distance(currentPosition, targetPosition) < 0.05f) // Ensure precise stopping at the center
            {
                player.position = targetPosition; // Snap to the exact center
                currentTargetIndex++;
            }
        }
        else
        {
            isClicked = false;
            path.Clear(); // Clear the path once the target is reached
        }
    }

    void HandleClick()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, mask))
            {
                Node targetNode = pathfinding.grid.NodeFromWorldPoint(hit.point);
                if (targetNode.isWalkable)
                {
                    isClicked = true;
                    currentTargetIndex = 0; // Reset the path index
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

        //Debug.Log(lastPosition);
        return lastPosition;
    }
}

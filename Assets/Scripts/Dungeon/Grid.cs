using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public static Grid Instance;

    public LayerMask walkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public Node[,] grid;

    public List<Node> path;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
    }

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = (Physics.CheckSphere(worldPoint, nodeRadius * 0.9f, walkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (Mathf.Abs(x) == Mathf.Abs(y)) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = worldPosition.x / gridWorldSize.x;
        float percentY = worldPosition.z / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX) * percentX);
        int y = Mathf.RoundToInt((gridSizeY) * percentY);
        //Debug.Log("x: " + x + ", " + y);
        return grid[x, y];
    }

    //void OnDrawGizmos()
    //{
    //    //Debug.Log("drawing gizmos");
    //    Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

    //    if (grid != null)
    //    {
    //        foreach (Node n in grid)
    //        {
    //            Gizmos.color = (n.isWalkable) ? Color.red : Color.white;
    //            if (path != null)
    //            {
    //                if (path.Contains(n)) Gizmos.color = Color.black;
    //            }
    //            //else
    //            //{
    //            //    Debug.Log("path null juga");
    //            //}
    //            Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
    //        }
    //    }
    //}
}
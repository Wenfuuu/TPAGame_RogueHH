using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    //public DungeonCreator dungeonCreator;

    //public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    //{
    //    Node startNode = dungeonCreator.GetNodeFromWorldPosition(startPos);
    //    Node targetNode = dungeonCreator.GetNodeFromWorldPosition(targetPos);

    //    List<Node> openSet = new List<Node>();
    //    HashSet<Node> closedSet = new HashSet<Node>();
    //    openSet.Add(startNode);

    //    while (openSet.Count > 0)
    //    {
    //        Node currentNode = openSet[0];
    //        for (int i = 1; i < openSet.Count; i++)
    //        {
    //            if (openSet[i].FCost < currentNode.FCost ||
    //                (openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost))
    //            {
    //                currentNode = openSet[i];
    //            }
    //        }

    //        openSet.Remove(currentNode);
    //        closedSet.Add(currentNode);

    //        if (currentNode == targetNode)
    //        {
    //            return RetracePath(startNode, targetNode);
    //        }

    //        foreach (Node neighbor in GetNeighbors(currentNode))
    //        {
    //            if (neighbor == null) continue;
    //            if (!neighbor.isWalkable || closedSet.Contains(neighbor))
    //                continue;

    //            int newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
    //            if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
    //            {
    //                neighbor.gCost = newCostToNeighbor;
    //                neighbor.hCost = GetDistance(neighbor, targetNode);
    //                neighbor.parent = currentNode;

    //                if (!openSet.Contains(neighbor))
    //                    openSet.Add(neighbor);
    //            }
    //        }
    //    }

    //    Debug.Log("gg sih");
    //    return null; // Path not found
    //}

    //List<Node> RetracePath(Node startNode, Node endNode)
    //{
    //    List<Node> path = new List<Node>();
    //    Node currentNode = endNode;

    //    while (currentNode != startNode)
    //    {
    //        path.Add(currentNode);
    //        currentNode = currentNode.parent;
    //    }

    //    path.Reverse();
    //    return path;
    //}

    //int GetDistance(Node a, Node b)
    //{
    //    int distX = Mathf.Abs(Mathf.RoundToInt(a.worldPosition.x - b.worldPosition.x));
    //    int distZ = Mathf.Abs(Mathf.RoundToInt(a.worldPosition.z - b.worldPosition.z));
    //    return distX + distZ;
    //}

    //List<Node> GetNeighbors(Node node)
    //{
    //    List<Node> neighbors = new List<Node>();
    //    int x = Mathf.RoundToInt(node.worldPosition.x / dungeonCreator.tileSize);
    //    int z = Mathf.RoundToInt(node.worldPosition.z / dungeonCreator.tileSize);

    //    // Check adjacent positions
    //    for (int dx = -1; dx <= 1; dx++)
    //    {
    //        for (int dz = -1; dz <= 1; dz++)
    //        {
    //            if (dx == 0 && dz == 0) continue;

    //            int checkX = x + dx;
    //            int checkZ = z + dz;

    //            if (checkX >= 0 && checkX < dungeonCreator.gridHeight && checkZ >= 0 && checkZ < dungeonCreator.gridWidth)
    //            {
    //                neighbors.Add(dungeonCreator.dungeonGrid[checkX, checkZ]);
    //            }
    //        }
    //    }

    //    return neighbors;
    //}
}

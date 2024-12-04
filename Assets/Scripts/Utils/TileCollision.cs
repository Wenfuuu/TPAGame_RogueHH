using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollision : MonoBehaviour
{
    public Grid gridRef;

    void Awake()
    {
        gridRef = Grid.Instance;    
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            //Debug.Log("ada enemy di " + transform.position);

            Node temp = gridRef.NodeFromWorldPoint(transform.position);
            gridRef.grid[temp.gridX, temp.gridY].isWalkable = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            if (collision.collider.CompareTag("Enemy")) Debug.Log("ada enemy cabut di " + transform.position);

            Node temp = gridRef.NodeFromWorldPoint(transform.position);
            gridRef.grid[temp.gridX, temp.gridY].isWalkable = true;
        }
    }

}

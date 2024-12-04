using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    private List<Vector3> availableTiles = new List<Vector3>();
    private bool spawned = false;
    private DungeonCreator dungeonCreator;

    void Start()
    {
        // Find the DungeonCreator and get available tiles
        dungeonCreator = GetComponent<DungeonCreator>();
        if (dungeonCreator != null)
        {
            availableTiles = dungeonCreator.GetAllTileWorldPositions();
            SpawnPlayer();
        }
    }

    void SpawnPlayer()
    {
        while (!spawned)
        {
            int randomIndex = Random.Range(0, availableTiles.Count);
            Vector3 spawnPosition = availableTiles[randomIndex];
            //Debug.Log(spawnPosition);

            Node spawnTile = dungeonCreator.grid.NodeFromWorldPoint(spawnPosition);
            if (!spawnTile.isWalkable) continue;

            spawned = true;
            spawnPosition.y = 2;
            playerPrefab.transform.position = spawnPosition;
        };
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    private List<Vector3> availableTiles = new List<Vector3>();

    void Start()
    {
        // Find the DungeonCreator and get available tiles
        DungeonCreator dungeonCreator = FindObjectOfType<DungeonCreator>();
        if (dungeonCreator != null)
        {
            availableTiles = dungeonCreator.GetAllTileWorldPositions();
            SpawnPlayerAtRandomTile();
        }
    }

    void SpawnPlayerAtRandomTile()
    {
        if (availableTiles.Count > 0)
        {
            // Choose a random tile
            int randomIndex = Random.Range(0, availableTiles.Count);
            Vector3 spawnPosition = availableTiles[randomIndex];
            spawnPosition.y = 1;
            //Debug.Log(spawnPosition);

            // Spawn the player at the chosen position
            playerPrefab.transform.position = spawnPosition;
        }
    }
}

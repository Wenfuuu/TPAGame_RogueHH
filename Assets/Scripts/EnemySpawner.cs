using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab1;
    public int EnemyCount = 5;

    private List<Vector3> availableTiles = new List<Vector3>();
    private bool spawned = false;
    private DungeonCreator dungeonCreator;

    // Start is called before the first frame update
    void Start()
    {
        dungeonCreator = GetComponent<DungeonCreator>();
        if (dungeonCreator != null)
        {
            availableTiles = dungeonCreator.GetAllTileWorldPositions();
            Debug.Log("count: " + availableTiles.Count);

            int randomIndex = Random.Range(0, availableTiles.Count);
            Vector3 spawnPosition = availableTiles[randomIndex];
            Debug.Log(spawnPosition);
            Node spawnTile = dungeonCreator.grid.NodeFromWorldPoint(spawnPosition);

            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        int count = 0;
        while (count < EnemyCount)
        {
            spawned = false;
            while (!spawned)
            {
                int randomIndex = Random.Range(0, availableTiles.Count);
                Vector3 spawnPosition = availableTiles[randomIndex];
                Debug.Log(spawnPosition);

                Node spawnTile = dungeonCreator.grid.NodeFromWorldPoint(spawnPosition);
                if (!spawnTile.isWalkable) continue;

                spawned = true;
                spawnPosition.y = 1;
                Instantiate(EnemyPrefab1, spawnPosition, Quaternion.identity);
            };
            count++;
        }
    }
}

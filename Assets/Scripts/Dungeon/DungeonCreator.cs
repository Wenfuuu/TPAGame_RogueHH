using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static BSPTree;

public class DungeonCreator : MonoBehaviour
{
    [Header("Dungeon Settings")]
    //public GameObject roomPrefab; // Prefab for a single room
    //public GameObject corridorPrefab; // Prefab for a corridor
    public GameObject[] tilesPrefab;

    private List<GameObject> decorPrefabs = new List<GameObject>();
    public GameObject decorPrefab1;
    public GameObject decorPrefab2;
    public GameObject decorPrefab3;
    public GameObject decorPrefab4;
    public GameObject emptyPrefab;

    public GameObject EnemyPrefab1;
    public GameObject EnemyPrefab2;
    public GameObject EnemyPrefab3;
    public GameObject BossPrefab;

    private int EnemyCount;
    private int gridWidth; // Width of the dungeon grid
    private int gridHeight; // Height of the dungeon grid
    private int numberOfRooms; // Number of rooms to generate

    public int buffer = 2; // Minimum buffer spacing between rooms
    public float tileSize = 2.0f; // Tile size for world coordinates
    public Grid grid;

    public Node[,] dungeonGrid; // 2D grid representing the dungeon layout
    private List<Vector2Int> roomCenters = new List<Vector2Int>(); // Center positions of rooms
    private List<Room> rooms = new List<Room>();

    private PlayerStateMachine player;

    private List<Node> corridors = new List<Node>();
    private List<Node> atEntrance = new List<Node>();
    private List<Node> decoratedTiles = new List<Node>();
    private List<Node> enemyTiles = new List<Node>();

    //private int totalTiles = 0;
    private int floor;

    public List<Vector3> GetAllTileWorldPositions()// untuk spawn player & enemy
    {
        List<Vector3> tileWorldPositions = new List<Vector3>();

        for (int x = 0; x < gridHeight; x++)
        {
            for (int z = 0; z < gridWidth; z++)
            {
                bool nearDecor = false;
                Node currentNode = dungeonGrid[x, z];
                for (int i = -1; i <= 1; i++)// cek kalo ada neighbor decorations (skip diagonal)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (Mathf.Abs(i) == Mathf.Abs(j)) continue;

                        int index1 = x + i;
                        int index2 = z + j;
                        if (index1 < 0 || index2 < 0 || index1 >= gridWidth || index2 >= gridHeight) continue;
                        Node temp = dungeonGrid[index1, index2];
                        if(decoratedTiles.Contains(temp)) nearDecor = true;
                    }
                    if (nearDecor) break;
                }

                if (currentNode != null && !corridors.Contains(currentNode) && !decoratedTiles.Contains(currentNode) && !enemyTiles.Contains(currentNode) && !nearDecor)
                {
                    tileWorldPositions.Add(currentNode.worldPosition);
                }
            }
        }

        return tileWorldPositions;
    }

    void Awake()
    {
        player = PlayerStateMachine.Instance;
        floor = player.GetComponent<PlayerFloor>().playerStats.CurrentFloor;
        if(floor == 0)// boss
        {
            EnemyCount = 1;
            gridWidth = 40;
            gridHeight = 40;
            numberOfRooms = 1;
        }
        else
        {
            EnemyCount = 5 + (floor / 4);
            gridWidth = 40 + (floor / 13 * 5);
            gridHeight = 40 + (floor / 13 * 5);
            numberOfRooms = 5 + (floor / 8);
        }
        dungeonGrid = new Node[gridWidth, gridHeight];

        GenerateRooms();
        ConnectRooms();
        decorPrefabs.Add(decorPrefab1);
        decorPrefabs.Add(decorPrefab2);
        decorPrefabs.Add(decorPrefab3);
        decorPrefabs.Add(decorPrefab4);
    }

    void Start()
    {
        //player = PlayerStateMachine.Instance;
        GenerateDecorations();
        if(floor == 0)
        {
            //spawn boss
            SpawnBoss();
        }
        else SpawnEnemy();
        GenerateEmptyTiles();
    }

    public class Room
    {
        public Vector2Int roomCenter;
        public int width;
        public int height;

        public Room(Vector2Int roomCenter, int width, int height)
        {
            this.roomCenter = roomCenter;
            this.width = width;
            this.height = height;
        }
    }

    void GenerateEmptyTiles()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                Node node = dungeonGrid[x, z];
                if (node == null)
                {
                    Vector3 position = new Vector3(x * tileSize, 1, z * tileSize);
                    Instantiate(emptyPrefab, position, Quaternion.identity);
                }
            }
        }
    }

    void SpawnBoss()
    {
        GameManager.Instance.UpdateEnemyCount.RaiseEvent(EnemyCount);

        foreach (Room room in rooms)
        {
            // Get room dimensions (5x7)
            int roomWidth = room.width;
            int roomHeight = room.height;

            // Calculate room bounds
            int startX = room.roomCenter.x - roomWidth / 2;
            int startY = room.roomCenter.y - roomHeight / 2;

            // Collect available tiles in the room
            List<Node> roomTiles = new List<Node>();
            Node playerTile = grid.NodeFromWorldPoint(player.gameObject.transform.position);
            for (int x = startX; x < startX + roomWidth; x++)
            {
                for (int y = startY; y < startY + roomHeight; y++)
                {
                    Node node = dungeonGrid[x, y];
                    if (node != null && !enemyTiles.Contains(node) && !decoratedTiles.Contains(node) && node.isWalkable && (node != playerTile))
                    {
                        roomTiles.Add(node);
                    }
                }
            }

            // Determine how many enemies to spawn in this room
            int enemiesToSpawn = 1;
            // Spawn enemies
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                if (roomTiles.Count == 0) break;

                int randomIndex = Random.Range(0, roomTiles.Count);
                Node selectedNode = roomTiles[randomIndex];

                // Set as occupied
                enemyTiles.Add(selectedNode);
                roomTiles.RemoveAt(randomIndex);

                // Set as unwalkable
                Node unwalk = grid.NodeFromWorldPoint(selectedNode.worldPosition);
                if (unwalk != null) unwalk.isWalkable = false;

                Vector3 temp = selectedNode.worldPosition;
                temp.y = 2;
                Instantiate(BossPrefab, temp, Quaternion.identity);
            }
        }
    }

    void SpawnEnemy()
    {
        GameManager.Instance.UpdateEnemyCount.RaiseEvent(EnemyCount);

        int enemiesPerRoom = EnemyCount / rooms.Count;
        int remainingEnemies = EnemyCount % rooms.Count;

        foreach (Room room in rooms)
        {
            // Get room dimensions (5x7)
            int roomWidth = room.width;
            int roomHeight = room.height;

            // Calculate room bounds
            int startX = room.roomCenter.x - roomWidth / 2;
            int startY = room.roomCenter.y - roomHeight / 2;

            // Collect available tiles in the room
            List<Node> roomTiles = new List<Node>();
            Node playerTile = grid.NodeFromWorldPoint(player.gameObject.transform.position);
            for (int x = startX; x < startX + roomWidth; x++)
            {
                for (int y = startY; y < startY + roomHeight; y++)
                {
                    Node node = dungeonGrid[x, y];
                    if (node != null && !enemyTiles.Contains(node) && !decoratedTiles.Contains(node) && node.isWalkable && (node != playerTile))
                    {
                        roomTiles.Add(node);
                    }
                }
            }

            // Determine how many enemies to spawn in this room
            int enemiesToSpawn = enemiesPerRoom + (remainingEnemies > 0 ? 1 : 0);
            if (remainingEnemies > 0) remainingEnemies--;

            // Spawn enemies
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                if (roomTiles.Count == 0) break;

                int randomIndex = Random.Range(0, roomTiles.Count);
                Node selectedNode = roomTiles[randomIndex];

                // Set as occupied
                enemyTiles.Add(selectedNode);
                roomTiles.RemoveAt(randomIndex);

                // Set as unwalkable
                Node unwalk = grid.NodeFromWorldPoint(selectedNode.worldPosition);
                if (unwalk != null) unwalk.isWalkable = false;

                // Spawn enemy
                selectedNode.worldPosition.y = 1;
                float commonWeight = Mathf.Max(100 - floor, 10); // Common decreases as floor increases
                float mediumWeight = Mathf.Max(floor * 1.5f, 10); // Medium increases moderately
                float eliteWeight = Mathf.Max(floor * 2f - 50, 10); // Elite increases more sharply on higher floors

                // Normalize weights
                float totalWeight = commonWeight + mediumWeight + eliteWeight;
                float commonChance = (commonWeight / totalWeight) * 100f;
                float mediumChance = (mediumWeight / totalWeight) * 100f;
                float eliteChance = (eliteWeight / totalWeight) * 100f;

                // Determine enemy type
                float randomValue = Random.Range(0f, 100f);
                GameObject selectedPrefab;
                if (randomValue < commonChance)
                {
                    selectedPrefab = EnemyPrefab1; // Common enemy
                }
                else if (randomValue < commonChance + mediumChance)
                {
                    selectedPrefab = EnemyPrefab2; // Medium enemy
                }
                else
                {
                    selectedPrefab = EnemyPrefab3; // Elite enemy
                }

                Instantiate(selectedPrefab, selectedNode.worldPosition, Quaternion.identity);
                //float randomValue = Random.Range(0f, 100f);
                //GameObject selectedPrefab = (randomValue < 50) ? EnemyPrefab1 : (randomValue < 80 ? EnemyPrefab2 : EnemyPrefab3);
                //Instantiate(selectedPrefab, selectedNode.worldPosition, Quaternion.identity);
            }
        }
    }

    Vector2Int GetGridPos(Node node)
    {
        int a = -1;
        int b = -1;
        for (int x = 0; x < gridHeight; x++)
        {
            for (int z = 0; z < gridWidth; z++)
            {
                if(dungeonGrid[x, z] == node)
                {
                    a = x;
                    b = z;
                }
            }
        }
        return new Vector2Int(a, b);
    }

    bool IsBufferClear(Vector2Int tilePos)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                //if (Mathf.Abs(x) == Mathf.Abs(z)) continue;

                int neighborX = tilePos.x + x;
                int neighborZ = tilePos.y + z;

                if (neighborX < 0 || neighborZ < 0 || neighborX >= gridWidth || neighborZ >= gridHeight)
                    continue;

                Node neighborNode = dungeonGrid[neighborX, neighborZ];
                if (decoratedTiles.Contains(neighborNode))
                {
                    return false;
                }
            }
        }
        return true;
    }

    void GenerateDecorations()
    {
        //Debug.Log("test");
        foreach (Room room in rooms)
        {
            // Get room dimensions (5x7)
            int roomWidth = room.width;
            int roomHeight = room.height;

            // Calculate room bounds
            int startX = room.roomCenter.x - roomWidth / 2;
            int startY = room.roomCenter.y - roomHeight / 2;

            // Calculate the number of decorations (20% of tiles in the room)
            int totalRoomTiles = roomWidth * roomHeight;
            int totalDecorations = Mathf.RoundToInt(totalRoomTiles * 0.2f);
            Debug.Log("total deco is: " + totalDecorations);

            // Collect available tiles in the room
            List<Node> roomTiles = new List<Node>();
            for (int x = startX; x < startX + roomWidth; x++)
            {
                for (int y = startY; y < startY + roomHeight; y++)
                {
                    Node node = dungeonGrid[x, y];
                    if (node != null && !decoratedTiles.Contains(node) && !corridors.Contains(node) && !atEntrance.Contains(node))
                    {
                        roomTiles.Add(node);
                    }
                }
            }

            // Place decorations randomly in the room
            for (int i = 0; i < totalDecorations; i++)
            {
                if (roomTiles.Count == 0) break;

                int randomIndex = Random.Range(0, roomTiles.Count);
                Node selectedNode = roomTiles[randomIndex];

                //cek buffer
                //bool checkbuffer = true;
                Vector2Int currGrid = GetGridPos(selectedNode);
                bool checkbuffer = IsBufferClear(currGrid);
                if (!checkbuffer) continue;

                // Set node as decorated
                decoratedTiles.Add(selectedNode);
                roomTiles.RemoveAt(randomIndex);

                // Set as unwalkable
                Node unwalk = grid.NodeFromWorldPoint(selectedNode.worldPosition);
                if (unwalk != null) unwalk.isWalkable = false;

                // Spawn decoration
                selectedNode.worldPosition.y = 1;
                float randomRotation = 90f * Random.Range(1, 4);
                int randomDecorIndex = Random.Range(0, decorPrefabs.Count);
                Instantiate(decorPrefabs[randomDecorIndex], selectedNode.worldPosition, Quaternion.Euler(0, randomRotation, 0));
            }
        }
    }

    void GenerateRooms()
    {
        //Vector2Int initialRoomCenter = new Vector2Int(2, 3); //start at 0, 0, 0
        //PlaceRoom(initialRoomCenter, 5, 7);
        //roomCenters.Add(initialRoomCenter);
        int []size = { 5, 7, 9 };
        for (int i = 0; i < numberOfRooms; i++)
        {
            int width = size[Random.Range(0, size.Length)];
            int height = size[Random.Range(0, size.Length)];

            if(floor == 0)
            {
                width = 9;
                height = 9;
            }

            bool roomPlaced = false;

            while (!roomPlaced)
            {
                Vector2Int roomCenter = new Vector2Int(
                    Random.Range(0 + buffer, gridWidth - buffer),
                    Random.Range(0 + buffer, gridHeight - buffer)
                );

                if (CanPlaceRoom(roomCenter, width, height))
                {
                    PlaceRoom(roomCenter, width, height);
                    roomCenters.Add(roomCenter);
                    rooms.Add(new Room(roomCenter, width, height));
                    roomPlaced = true;
                }
            }
        }
    }

    bool CanPlaceRoom(Vector2Int center, int width, int height)
    {
        int startX = center.x - width / 2 - buffer;
        int startY = center.y - height / 2 - buffer;
        int endX = center.x + width / 2 + buffer;
        int endY = center.y + height / 2 + buffer;

        if (startX < 0 || startY < 0 || endX >= gridWidth || endY >= gridHeight)
            return false;

        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                if (dungeonGrid[x, y] != null) return false; // Overlaps with existing room
            }
        }

        return true;
    }

    void PlaceRoom(Vector2Int center, int width, int height)
    {
        int startX = center.x - width / 2;
        int startY = center.y - height / 2;

        for (int x = startX; x < startX + width; x++)
        {
            for (int y = startY; y < startY + height; y++)
            {
                // Instantiate room tiles
                Vector3 position = new Vector3(x * tileSize, 0, y * tileSize);

                int gridX = Mathf.RoundToInt(position.x / tileSize);
                int gridY = Mathf.RoundToInt(position.z / tileSize);

                dungeonGrid[x, y] = new Node(true, position, gridX, gridY); // Mark as occupied
                int idx = Random.Range(0, tilesPrefab.Length);
                GameObject tile = tilesPrefab[idx];
                Instantiate(tile, position, Quaternion.identity);

                //totalTiles++;
            }
        }
    }

    void ConnectRooms()
    {
        // Use Prim's algorithm to connect rooms with corridors
        HashSet<Vector2Int> connected = new HashSet<Vector2Int> { roomCenters[0] };
        HashSet<Vector2Int> unconnected = new HashSet<Vector2Int>(roomCenters);
        unconnected.Remove(roomCenters[0]);

        while (unconnected.Count > 0)
        {
            Vector2Int nearestConnected = Vector2Int.zero;
            Vector2Int nearestUnconnected = Vector2Int.zero;
            float minDistance = float.MaxValue;

            foreach (Vector2Int connectedRoom in connected)
            {
                foreach (Vector2Int unconnectedRoom in unconnected)
                {
                    float distance = Vector2Int.Distance(connectedRoom, unconnectedRoom);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestConnected = connectedRoom;
                        nearestUnconnected = unconnectedRoom;
                    }
                }
            }

            // Connect the nearest connected and unconnected rooms
            CreateCorridor(nearestConnected, nearestUnconnected);
            connected.Add(nearestUnconnected);
            unconnected.Remove(nearestUnconnected);
        }
    }

    void CreateCorridor(Vector2Int start, Vector2Int end)
    {
        Vector2Int current = start;

        // Horizontal corridor
        while (current.x != end.x)
        {
            current.x += (end.x > current.x) ? 1 : -1;
            PlaceCorridorTile(current);
        }

        // Vertical corridor
        while (current.y != end.y)
        {
            current.y += (end.y > current.y) ? 1 : -1;
            PlaceCorridorTile(current);
        }
    }

    void PlaceCorridorTile(Vector2Int position)
    {
        if (dungeonGrid[position.x, position.y] == null)
        {
            // Instantiate corridor tile
            Vector3 worldPosition = new Vector3(position.x * tileSize, 0, position.y * tileSize);

            int gridX = Mathf.RoundToInt(worldPosition.x / tileSize);
            int gridY = Mathf.RoundToInt(worldPosition.z / tileSize);

            dungeonGrid[position.x, position.y] = new Node(true, worldPosition, gridX, gridY);
            //Instantiate(corridorPrefab, worldPosition, Quaternion.identity);
            int idx = Random.Range(0, tilesPrefab.Length);
            GameObject tile = tilesPrefab[idx];
            Instantiate(tile, worldPosition, Quaternion.identity);

            // add to corridors list
            corridors.Add(dungeonGrid[position.x, position.y]);

            CheckAtEntrance(position);
        }
    }

    void CheckAtEntrance(Vector2Int position)// ini posisi collidor yang baru dimasukin
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (Mathf.Abs(x) == Mathf.Abs(y)) continue;

                Node neighbor = dungeonGrid[position.x + x, position.y + y];
                if(neighbor != null && !corridors.Contains(neighbor))// kalo neighbor ada dan bukan collider
                {
                    atEntrance.Add(neighbor);
                }
            }
        }
    }
}
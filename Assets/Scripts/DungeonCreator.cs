using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    [Header("Dungeon Settings")]
    public GameObject roomPrefab; // Prefab for a single room
    public GameObject corridorPrefab; // Prefab for a corridor
    public GameObject decorPrefab1;
    public GameObject decorPrefab2;
    public GameObject decorPrefab3;
    public GameObject decorPrefab4;

    public int gridWidth = 30; // Width of the dungeon grid
    public int gridHeight = 30; // Height of the dungeon grid
    public int numberOfRooms = 5; // Number of rooms to generate
    public int buffer = 2; // Minimum buffer spacing between rooms
    public float tileSize = 2.0f; // Tile size for world coordinates
    public Grid grid;

    public Node[,] dungeonGrid; // 2D grid representing the dungeon layout
    private List<Vector2Int> roomCenters = new List<Vector2Int>(); // Center positions of rooms

    private List<Node> corridors = new List<Node>();
    private List<Node> atEntrance = new List<Node>();
    private List<Node> decoratedTiles = new List<Node>();

    private int totalTiles = 0;

    public List<Vector3> GetAllTileWorldPositions()
    {
        List<Vector3> tileWorldPositions = new List<Vector3>();

        for (int x = 0; x < gridHeight; x++)
        {
            for (int z = 0; z < gridWidth; z++)
            {
                Node currentNode = dungeonGrid[x, z];

                // Check if this node is part of the dungeon (occupied)
                if (currentNode != null)
                {
                    tileWorldPositions.Add(currentNode.worldPosition);
                }
            }
        }

        return tileWorldPositions;
    }

    void Awake()
    {
        dungeonGrid = new Node[gridWidth, gridHeight];
        GenerateRooms();
        ConnectRooms();
    }

    void Start()
    {
        GenerateDecorations();
    }

    void GenerateDecorations()
    {
        //Debug.Log("test");
        int totalDecorations = Mathf.RoundToInt(totalTiles * 0.2f);
        int decor = 0;
        while(decor < totalDecorations)
        {
            bool checkBuffer = true;

            int a = Random.Range(0, gridHeight);
            int b = Random.Range(0, gridWidth);

            Node node = dungeonGrid[a, b];
            if(node != null && !corridors.Contains(node) && !atEntrance.Contains(node) && !decoratedTiles.Contains(node))
            {
                //cek buffer
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (Mathf.Abs(x) == Mathf.Abs(y)) continue;

                        int index1 = a + x;
                        int index2 = b + y;
                        if (index1 < 0 || index2 < 0 || index1 >= gridWidth || index2 >= gridHeight) continue;
                        Node temp = dungeonGrid[index1, index2];
                        //if(temp != null) Debug.Log("temp: " + temp.worldPosition);

                        if(temp != null && decoratedTiles.Contains(temp))
                        {
                            Debug.Log("ngecek di " + node.worldPosition + " neighbor sudah decorated di pos: " + temp.worldPosition);
                            checkBuffer = false;
                        }
                    }
                    if (!checkBuffer) break;
                }

                if(checkBuffer)
                {
                    // add to list
                    decoratedTiles.Add(node);

                    //set to unwalkable
                    Node unwalk = grid.NodeFromWorldPoint(node.worldPosition);
                    if (unwalk != null) unwalk.isWalkable = false;

                    // spawn decor 
                    node.worldPosition.y = 1;
                    float randomRotation = 90f * Random.Range(1, 4);
                    Instantiate(decorPrefab1, node.worldPosition, Quaternion.Euler(0, randomRotation, 0));
                    decor++;
                }
            } 
        }
    }

    void GenerateRooms()
    {
        Vector2Int initialRoomCenter = new Vector2Int(2, 3); //start at 0, 0, 0
        PlaceRoom(initialRoomCenter, 5, 7);
        roomCenters.Add(initialRoomCenter);

        for (int i = 1; i < numberOfRooms; i++)
        {
            bool roomPlaced = false;

            while (!roomPlaced)
            {
                Vector2Int roomCenter = new Vector2Int(
                    Random.Range(0 + buffer, gridWidth - buffer),
                    Random.Range(0 + buffer, gridHeight - buffer)
                );

                if (CanPlaceRoom(roomCenter, 5, 7))
                {
                    PlaceRoom(roomCenter, 5, 7);
                    roomCenters.Add(roomCenter);
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
                Instantiate(roomPrefab, position, Quaternion.identity);

                totalTiles++;
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
            Instantiate(corridorPrefab, worldPosition, Quaternion.identity);

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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private Vector3Int startPos = Vector3Int.zero;
    [SerializeField] private int walkLength = 10;
    [SerializeField] private int numRooms = 10;
    [SerializeField] private int roomSize = 10;
    //[SerializeField] private int roomBoarder = 10;

    //Think of DungeonSize as like a Radius: 25 units up, down, left, right from center
    //[SerializeField] private int DungeonSize = 25;

    [SerializeField] private GameObject floorTile;
    [SerializeField] private GameObject starterTile;
    [SerializeField] private GameObject wallTile;
    [SerializeField] private GameObject doorTile;

    private HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
    private HashSet<GameObject> tilesTotal = new HashSet<GameObject>();
    private HashSet<Vector3Int> wallsTotal = new HashSet<Vector3Int>();

    public static List<Vector3Int> cardinalDirectionsList = new List<Vector3Int>()
    {
        new Vector3Int(0,0,1), //UP
        new Vector3Int(0,0,-1), //DOWN
        new Vector3Int(1,0,0), //RIGHT
        new Vector3Int(-1,0,0) //LEFT
    };

    void Start()
    {
        generate();
    }

    public void generate() {
        clear();
        wallsTotal.Add(new Vector3Int(4,0,0));
        starterGen(starterTile);
        roomGen(numRooms, Vector3Int.zero);
    }

    private void clear() {
        foreach (var tile in tilesTotal) {
            Destroy(tile); // Destroy old tiles from the scene
        }
        tilesTotal.Clear(); // Clear the list
        visited.Clear();
        wallsTotal.Clear();
    }

    private void tileGen(GameObject tile, HashSet<Vector3Int> floorPos) {
        foreach (var floor in floorPos) {
            GameObject spawnedTile = Instantiate(tile, new Vector3(floor.x, floor.y, floor.z), Quaternion.identity);
            tilesTotal.Add(spawnedTile); // Track the instantiated tiles
        }
    }

    private void roomGen(int numRooms, Vector3Int center) {
        HashSet<Vector3Int> floorPos = new HashSet<Vector3Int>();
        for (int i = 0; i < numRooms; i++) {
            // doesn't change center for the 3 starter rooms.
            if (center == Vector3Int.zero) {
                center = wallsTotal.ElementAt(Random.Range(0, wallsTotal.Count));
            }
            floorPos = RandomWalk.randomWalk(center, walkLength, roomSize, visited);

            if (floorPos.Count == 0) {
                Debug.Log("couldn't find a good start pos");
                continue;
            }
            if (numRooms > 1) {
                var doorPos = RandomWalk.startReal + (RandomWalk.prevMove * -1);
                GameObject door = Instantiate(doorTile, doorPos + Vector3Int.up, Quaternion.identity);
                tilesTotal.Add(door);
            }
            visited.UnionWith(floorPos);
            tileGen(floorTile, floorPos);
            wallGen(wallTile, floorPos);
        }
    }

    private void starterGen(GameObject tile) {
        HashSet<Vector3Int> floorPos = new HashSet<Vector3Int>();
        for (int i = -2; i <= 2; i++) {
            for (int j = -2; j <= 2; j++) {
                floorPos.Add(new Vector3Int(i, 0, j));
            }
        }
        roomGen(1, new Vector3Int(4, 0, 0));
        roomGen(1, new Vector3Int(0, 0, 4));
        roomGen(1, new Vector3Int(0, 0, -4));
        visited.UnionWith(floorPos);
        tileGen(tile, floorPos);
    }

    private void wallGen(GameObject tile, HashSet<Vector3Int> floorPos) {
        HashSet<Vector3Int> wallPos = new HashSet<Vector3Int>();
        foreach (var floor in floorPos) {
            var tileUp = floor + cardinalDirectionsList[0];
            var tileDown = floor + cardinalDirectionsList[1];
            var tileRight = floor + cardinalDirectionsList[2];
            var tileLeft = floor + cardinalDirectionsList[3];

            if(!floorPos.Contains(tileUp)) {
                wallPos.Add(tileUp);
            }
            if (!floorPos.Contains(tileDown)) {
                wallPos.Add(tileDown);
            }
            if (!floorPos.Contains(tileRight)) {
                wallPos.Add(tileRight);
            }
            if (!floorPos.Contains(tileLeft)) {
                wallPos.Add(tileLeft);
            }
        }
        visited.UnionWith(wallPos);
        wallsTotal.UnionWith(wallPos);
        tileGen(tile, wallPos);
    }
}

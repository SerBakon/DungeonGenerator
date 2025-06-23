using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private Vector3Int startPos = Vector3Int.zero;
    [SerializeField] private int walkLength = 10;
    [SerializeField] private int numRooms = 10;
    //[SerializeField] private int roomBoarder = 10;

    //Think of DungeonSize as like a Radius: 25 units up, down, left, right from center
    [SerializeField] private int DungeonSize = 25;

    [SerializeField] private GameObject floorTile;
    [SerializeField] private GameObject starterTile;

    private HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
    void Start()
    {
        generate();
    }

    private void generate() {
        roomGen(numRooms);
        starterGen(starterTile);
    }
    private void floorGen(GameObject tile, HashSet<Vector3Int> floorPos) {
        foreach (var floor in floorPos) {
            Instantiate(tile, new Vector3(floor.x,floor.y,floor.z), Quaternion.identity);
        }
    }

    private void roomGen(int numRooms) {
        HashSet<Vector3Int> floorPos = new HashSet<Vector3Int>();
        for (int i = 0; i < numRooms; i++) {
            Vector3Int roomCenter = new Vector3Int(Random.Range(-1 * DungeonSize, DungeonSize), 0, Random.Range(-1 * DungeonSize, DungeonSize));
            floorPos.UnionWith(RandomWalk.randomWalk(roomCenter, walkLength, visited));
        }
        visited.UnionWith(floorPos);
        floorGen(floorTile, floorPos);
    }

    private void starterGen(GameObject tile) {
        HashSet<Vector3Int> floorPos = new HashSet<Vector3Int>();
        for (int i = -2; i <= 2; i++) {
            for (int j = -2; j <= 2; j++) {
                floorPos.Add(new Vector3Int(i, 0, j));
            }
        }
        visited.UnionWith(floorPos);
        floorGen(tile, floorPos);
    }
}

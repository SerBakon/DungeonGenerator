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
    
    private HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
    void Start()
    {
        floorGen();
    }

    private void floorGen() {
        roomGen(numRooms);
        foreach (var floor in visited) {
            Instantiate(floorTile, new Vector3(floor.x,floor.y,floor.z), Quaternion.identity);
        }
    }

    private void roomGen(int numRooms) {
        for (int i = 0; i < numRooms; i++) {
            Vector3Int roomCenter = new Vector3Int(Random.Range(-1 * DungeonSize, DungeonSize), 0, Random.Range(-1 * DungeonSize, DungeonSize));
            HashSet<Vector3Int> floorPos = RandomWalk.randomWalk(roomCenter, walkLength, visited);
            visited.UnionWith(floorPos);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RandomWalk {

    public static List<Vector3Int> cardinalDirectionsList = new List<Vector3Int>()
    {
        new Vector3Int(0,0,1), //UP
        new Vector3Int(0,0,-1), //DOWN
        new Vector3Int(1,0,0), //RIGHT
        new Vector3Int(-1,0,0) //LEFT
    };
    public static HashSet<Vector3Int> randomWalk(Vector3Int startPos, int walkLength, int maxLen, HashSet<Vector3Int> walkedSet) {
        HashSet<Vector3Int> floorPos = new HashSet<Vector3Int>();
        int max = Mathf.Abs(startPos.x) + maxLen;

        int attempts1 = 0;
        while (walkedSet.Contains(startPos) && attempts1 < 100) {
            Debug.Log("Started in a taken space");
            startPos += cardinalDirectionsList[Random.Range(0, 4)];
            attempts1++;
        }
        floorPos.Add(startPos);
        Debug.Log("start pos: " + startPos);

        Vector3Int prevPos = startPos;

        for (int i = 0; i < walkLength; i++) {
            int attempts = 0;
            Vector3Int newPos;
            do {
                Vector3Int direction = cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
                newPos = prevPos + direction;
                attempts++;
            } while ((walkedSet.Contains(newPos) || Mathf.Abs(newPos.x - startPos.x) > maxLen || Mathf.Abs(newPos.z - startPos.z) > maxLen) && attempts < 100);

            if (attempts < 100) {
                floorPos.Add(newPos);
                prevPos = newPos;
                //Debug.Log("Added newPos at: " + newPos);
            }
            else {
                // Dead-end, choose a random previous floor tile to resume from
                prevPos = floorPos.ElementAt(Random.Range(0, floorPos.Count));
                Debug.Log("Too many attempts. Jumping to random floorPos.");
            }
        }

        return floorPos;
    }
}
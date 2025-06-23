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
    public static HashSet<Vector3Int> randomWalk(Vector3Int startPos, int walkLength, HashSet<Vector3Int> walkedSet) {
        HashSet<Vector3Int> floorPos = new HashSet<Vector3Int>();

        floorPos.Add(startPos);

        Vector3Int prevPos = startPos;
        //for loop makes sure to generate a new coordinate for however long walkLength is
        for (int i = 0; i < walkLength; i++) {
            // generate a random direction for the next step
            int randomDirectionElementNum = Random.Range(0, 4);
            var newPos = prevPos + cardinalDirectionsList[randomDirectionElementNum];

            //if the walker has already walked there, find a new place to walk to.
            int attempts = 0;
            while(floorPos.Contains(newPos) && attempts < 20) {
                randomDirectionElementNum = Random.Range(0, 4);
                newPos = prevPos + cardinalDirectionsList[randomDirectionElementNum];
                attempts++;
                if(attempts == 20) {
                    Debug.Log("too many attempts, restarting walker" + attempts);
                }
            }
            if(!(attempts == 20)) {
                floorPos.Add(newPos);
                Debug.Log("Added newPos at: " + newPos);
            } else {
                prevPos = floorPos.ElementAt(Random.Range(0,floorPos.Count));
            }
        }
        return floorPos;
    }
}
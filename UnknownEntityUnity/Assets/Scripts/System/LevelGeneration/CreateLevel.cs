using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLevel : MonoBehaviour
{
    public SO_LevelBase level;

    int roomAmnt;
    int roomSize;
    public GameObject[] roomHolder;

    void Update()
    {
        if (Input.GetKeyDown("c")) {
            // Create level.
            SetupLevelParameters();
            SpawnRooms();
        }
    }

    void SetupLevelParameters() {
        roomAmnt = Random.Range(level.minRoomAmnt, level.maxRoomAmnt+1);
        roomSize = Random.Range(level.minRoomSize, level.maxRoomSize+1);
    }

    void SpawnRooms(){
        for (int i = 0; i < level.rooms.Count; i++) {
            if (i == 0) {
                int connection = Random.Range(0, level.rooms[i].roomConnections.Length);
                roomHolder[i].transform.position = level.rooms[i].roomConnections[connection];
            }
        }
    }
}

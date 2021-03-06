using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Diagnostics;

public class PremadeRoomLevelGeneration : MonoBehaviour
{
    public SO_RoomBase[] roomBases;
    public int amntOfRooms = 1;
    public Transform levelRoomHolder;
    Tilemap thisFloorTilemap;
    Tilemap thisWallTileMap;
    Vector3Int lastExitDoorCellPos;
    Vector3 lastExitDoorWorldPos;
    int lastDoorDir;
    int lastExitDoorSide = 999;
    int lastEntranceDoorSide = 999;
    int lastRoomTileGridMinX, lastRoomTileGridMaxX;
    int lastRoomTileGridMinY, lastRoomTileGridMaxY;
    float tileGridCellSize = 1f;

    public void SetupCreateLevel() {
        Stopwatch premadeSW = new Stopwatch();
        premadeSW.Start();

        FirstRoom();
        for(int i = 1; i < amntOfRooms; i++) {
            NextRoom(i);
        }

        premadeSW.Stop();
        print("Premade level created in: " + premadeSW.ElapsedMilliseconds + "ms");
    }

    void FirstRoom() {
        int chosenRoom = Random.Range(0, roomBases.Length);
        GameObject newRoom = Instantiate(roomBases[chosenRoom].roomPrefab, Vector3.zero, Quaternion.identity, levelRoomHolder);
        PremadeRoom newRoomScript = newRoom.GetComponent<PremadeRoom>();
        tileGridCellSize = newRoomScript.grid.cellSize.x;
        thisFloorTilemap = newRoomScript.floorTileMap;
        thisWallTileMap = newRoomScript.wallTileMap;
        if (amntOfRooms > 1) {
            AssignExitDoor(newRoomScript);
        }
    }
    // Chose the next room, connect it to the last one based on door positions, adjust its entrance tile. If its not the last room, create an exit door.
    void NextRoom(int roomNumber) {
        // Randomly pick a room for the array of available rooms.
        int chosenRoom = Random.Range(0, roomBases.Length);
        // Grab the prefab room's posittion (should be: 0, 0, 0).
        Vector3 roomPos = roomBases[chosenRoom].roomPrefab.transform.position;
        // Instantiate the new room and grab its script component.
        GameObject newRoom = Instantiate(roomBases[chosenRoom].roomPrefab, Vector3.zero, Quaternion.identity, levelRoomHolder);
        PremadeRoom newRoomScript = newRoom.GetComponent<PremadeRoom>();
        // Chose a connecting entrance door that is on the opposite side of the last one.
        int entranceDoorSide = OppositeDoorSide(lastExitDoorSide);
        Transform[] doorsOnSide = GetDoorsOnSide(newRoomScript, entranceDoorSide);
        int entranceDoor = Random.Range(0, doorsOnSide.Length);
        // This is gonna be useless as the door objects will already be turned off.
        doorsOnSide[entranceDoor].gameObject.SetActive(false);
        // Move the room to the correct world position based on the diffence between the entrance door's position while its pivot is at 0, 0, 0 and where the entrance door should be.
        Vector3 entranceDoorWorldPos = newRoomScript.potentialDoors[entranceDoorSide].position;
        Vector3 entranceDoorConnectedWorldPos = lastExitDoorWorldPos + NewDoorOffset(lastExitDoorSide);
        newRoom.transform.position += (entranceDoorConnectedWorldPos - entranceDoorWorldPos);
        // Get the new entrance door's cell position, remove the wall tile and add a floor tile.
        Vector3Int entranceDoorCellPos = newRoomScript.wallTileMap.WorldToCell(entranceDoorConnectedWorldPos);
        thisFloorTilemap = newRoomScript.floorTileMap;
        thisWallTileMap = newRoomScript.wallTileMap;
        thisFloorTilemap.SetTile(entranceDoorCellPos, newRoomScript.floorTile);
        thisWallTileMap.SetTile(entranceDoorCellPos, null);
        // Check to see if I need to create and exit door or if this is the last room.
        if (roomNumber < amntOfRooms-1) {
            lastEntranceDoorSide = entranceDoorSide;
            AssignExitDoor(newRoomScript);
        }
    }
    // Assign an exit door that is not the same as the entrance door.
    void AssignExitDoor(PremadeRoom roomScript) {
        // Pick one of the potential doors at random and turn it into a door.
        // The randomness could be influenced if for example the dungeon needs to grow in a certain direction.
        int doorSide = 0;
        if (lastEntranceDoorSide < 999) {
            // int numberOfLoops = 0;
            do {
                // numberOfLoops++;
                doorSide = Random.Range(0, roomScript.potentialDoors.Length);
            } while (doorSide == lastEntranceDoorSide);
            //print(numberOfLoops);
        }
        else {
            doorSide = Random.Range(0, roomScript.potentialDoors.Length);
        }
        roomScript.potentialDoors[doorSide].gameObject.SetActive(false);
        //print("Door number: " + doorNumber);
        // Randomly select a door thats on the correct side. (0=up, 1=right, ...)
        Transform[] doorsOnSide = GetDoorsOnSide(roomScript, doorSide);
        int openDoor = Random.Range(0, doorsOnSide.Length);
        // Get the world position and the cell position (on the tile grid) for the open door.
        lastExitDoorWorldPos = doorsOnSide[openDoor].position;
        lastExitDoorCellPos = roomScript.wallTileMap.WorldToCell(lastExitDoorWorldPos);
        //Store the last door side for the next room.
        lastExitDoorSide = doorSide;
        // Take off the wall tile and add a floor tile.
        thisWallTileMap.SetTile(lastExitDoorCellPos, null);
        thisFloorTilemap.SetTile(lastExitDoorCellPos, roomScript.floorTile);
        // Possibly skip compress bounds since Unity says it is slow and just make sure its done in the prefab. (Tilemap -> Cog -> Compress)
        thisWallTileMap.CompressBounds();
        lastRoomTileGridMinX = Mathf.RoundToInt(thisWallTileMap.cellBounds.min.x);
        lastRoomTileGridMaxX = Mathf.RoundToInt(thisWallTileMap.cellBounds.max.x);
        lastRoomTileGridMinY = Mathf.RoundToInt(thisWallTileMap.cellBounds.min.y);
        lastRoomTileGridMaxY = Mathf.RoundToInt(thisWallTileMap.cellBounds.max.y);
    }
    // Get the opposite side.
    int OppositeDoorSide(int doorSide) {
        if (doorSide == 0) {
            return 2;
        }
        else if (doorSide == 1) {
            return 3;
        }
        else if (doorSide == 2) {
            return 0;
        }
        else {
            return 1;
        }
    }
    // Add an offset based on a door side and the tilemap's grid cell size.
    Vector3 NewDoorOffset(int lastExitDoorSide) {
        if (lastExitDoorSide == 0) {
            return Vector3.up * tileGridCellSize;
        }
        else if (lastExitDoorSide == 1) {
            return Vector3.right * tileGridCellSize;
        }
        else if (lastExitDoorSide == 2) {
            return Vector3.down * tileGridCellSize;
        }
        else {
            return Vector3.left * tileGridCellSize;
        }
    }
    // Get the doors from a room script based on a side.
    Transform[] GetDoorsOnSide(PremadeRoom roomScript, int side) {
        if (side == 0) {
            return roomScript.potentialDoorsUp;
        }
        else if (side == 1) {
            return roomScript.potentialDoorsRight;
        }
        else if (side == 2) {
            return roomScript.potentialDoorsDown;
        }
        else {
            return roomScript.potentialDoorsLeft;
        }
    }
}

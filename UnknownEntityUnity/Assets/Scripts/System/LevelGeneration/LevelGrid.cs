using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Diagnostics;

public class LevelGrid : MonoBehaviour
{
    public float tileSize;
    // The room size = amount of cells.
    public int minRoomTilesX, maxRoomTilesX;
    public int minRoomTilesY, maxRoomTilesY;
    public SO_RoomLayouts roomLayouts;
    // These tiles's size should match the tileSize set above.
    public Tile floorTile, wallTile;
    public Tilemap floorTilemap, wallTilemap;
    public Grid grid;
    int roomTilesX, roomTilesY;
    // Multiple of the room size as well as matching the tilegrid tile size?
    public int minAmntRoomsX, maxAmntRoomsX;
    public int minAmntRoomsY, maxAmntRoomsY;

    public int amntOfRooms;
    int roomsX, roomsY;
    Vector2 centeredLvlBotLeft;
    Room[,] rooms;
    Room_Connections roomConnect;
    List<Room> chosenRooms = new List<Room>();
    List<int> availableDirs = new List<int>();
    bool up, right, down, left;
    
    public void CreateLevel() {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        // To be able to re-create the rooms without getting out of playmode everytime.
        if (chosenRooms.Count > 0) {
            ClearAllRoomTiles(chosenRooms);
            chosenRooms.Clear();
        }
        roomsX = Random.Range(minAmntRoomsX, maxAmntRoomsX+1);
        roomsY = Random.Range(minAmntRoomsY, maxAmntRoomsY+1);
        roomTilesX = Random.Range(minRoomTilesX, maxRoomTilesX+1);
        roomTilesY = Random.Range(minRoomTilesY, maxRoomTilesY+1);
        float roomWidth = roomTilesX * tileSize;
        float roomHeight = roomTilesY * tileSize;
        rooms = new Room[roomsX, roomsY];
        centeredLvlBotLeft = (Vector2)this.transform.position - Vector2.right*roomsX*roomWidth/2 - Vector2.up*roomsY*roomHeight/2;
        floorTilemap.gameObject.transform.position = centeredLvlBotLeft;
        wallTilemap.gameObject.transform.position = centeredLvlBotLeft;
        grid.cellSize = new Vector2(tileSize, tileSize);

        // for (int x = 0; x < roomsX; x++) {
        //     for (int y = 0; y < roomsY; y++) {
        //         Vector2 botLeftPos = new Vector2(centeredLvlBotLeft.x + x*roomWidth, centeredLvlBotLeft.y + y*roomHeight);
        //         rooms[x, y] = new Room(roomTilesX, roomTilesY, tileSize, botLeftPos, RandomRoomLayout(), x, y);
        //         rooms[x, y].CreateRoom();
        //     }
        // }

        int firstRoomX = Random.Range(0,roomsX);
        // If from bot to top -> one at random on the bottom row to start.
        Vector2 botLeftPos = new Vector2(centeredLvlBotLeft.x + firstRoomX*roomWidth, centeredLvlBotLeft.y + 0*roomHeight);
        rooms[firstRoomX, 0] = new Room(roomTilesX, roomTilesY, tileSize, botLeftPos, RandomRoomLayout(), firstRoomX, 0, 2);
        chosenRooms.Add(rooms[firstRoomX, 0]);
        Room curRoom = chosenRooms[0];
        curRoom.exit = 0;
        for (int i = 1; i < amntOfRooms; i++) {
            // Reset possible directions.
            up = right = down = left = true;

            if ((roomTilesY-curRoom.indexY) == (amntOfRooms-(i-1))) {
                up = true;
                right = down = left = false;
            }
            else {
                // Remove direction if next to an edge ex: CurRoom is all the way right, cannot move right.
                if (curRoom.indexX == roomsX-1) {right = false;}
                if (curRoom.indexX == 0) {left = false;}
                if (curRoom.indexY == roomsY-1) {up = false;}
                if (curRoom.indexY == 0) {down = false;}
                // Remove opposite direction after moving, ex: move left(3) next cannot move right(1).
                RemoveOpposite(curRoom.exit);
                // Remove a certain direction everytime. down = false;
                RemoveOpposite(0);
            }
            availableDirs = AvailableDirections(up, right, down, left);
            if (availableDirs.Count == 0) {
                print("Oups, I cant seem to go anywhere. "+"I have "+(amntOfRooms-i)+" room(s) left to create. What should I do?");
                i = amntOfRooms;
            }
            else {
                // Get next direction.
                int dir = Random.Range(0,availableDirs.Count);
                // Add the next direction vector to the current room index to get the next room's index.
                Vector2 curRoomIndex = new Vector2(curRoom.indexX, curRoom.indexY);
                Vector2 newRoomIndex = curRoomIndex + DirectionVector(availableDirs[dir]);
                int newRoomIndexX = (int)newRoomIndex.x;
                int newRoomIndexY = (int)newRoomIndex.y;
                botLeftPos = new Vector2(centeredLvlBotLeft.x + newRoomIndexX*roomWidth, centeredLvlBotLeft.y + newRoomIndexY*roomHeight);
                rooms[newRoomIndexX, newRoomIndexY] = new Room(roomTilesX, roomTilesY, tileSize, botLeftPos,RandomRoomLayout(), newRoomIndexX, newRoomIndexY, OppositeDoorSide(availableDirs[dir]));
                chosenRooms.Add(rooms[newRoomIndexX, newRoomIndexY]);
                curRoom = chosenRooms[i];
                curRoom.exit = availableDirs[dir];
            }
        }
        // Create all the chosen rooms.
        foreach(Room room in chosenRooms) {
            room.CreateRoom();
        }
        // Assign tiles to the chosen rooms.
        AssignRoomTiles(chosenRooms);
        
        sw.Stop();
        print("Level rooms & tiles created in: "+sw.ElapsedMilliseconds+"ms");
    }
    // Add all true direction to a list.
    List<int> AvailableDirections(bool _up, bool _right, bool _down, bool _left) {
        availableDirs.Clear();
        if (_up) {availableDirs.Add(0);}
        if (_right) {availableDirs.Add(1);}
        if (_down) {availableDirs.Add(2);}
        if (_left) {availableDirs.Add(3);}
        return availableDirs;
    }
    // Get the opposite int side of an int.
    int OppositeDoorSide(int doorSide) {
        if (doorSide == 0) {return 2;}
        else if (doorSide == 1) {return 3;}
        else if (doorSide == 2) {return 0;}
        else {return 1;}
    }
    // Set the opposite direction to an int to false.
    void RemoveOpposite(int doorSide) {
        if (doorSide == 0) {down = false;}
        else if (doorSide == 1) {left = false;}
        else if (doorSide == 2) {up = false;}
        else {right = false;}
    }
    // Get a Vector2 direction from an int direction.
    public Vector2 DirectionVector(int dir) {
        if (dir == 0) {return Vector2.up;}
        else if (dir == 1) {return Vector2.right;}
        else if (dir == 2) {return Vector2.down;}
        else {return Vector2.left;}
    }

    // Assign the correct tile visual based on their value.
    void AssignAllRoomsTiles() {
        Stopwatch tileSw = new Stopwatch();
        tileSw.Start();
        // Rooms.
        for (int roomX = 0; roomX < roomsX; roomX++) {
            for (int roomY = 0; roomY < roomsY; roomY++) {
                // Tiles in rooms.
                for (int tileX = 0; tileX < roomTilesX; tileX++) {
                    for (int tileY = 0; tileY < roomTilesY; tileY++) {
                        int tileXLvlValue = roomX*roomTilesX + tileX;
                        int tileYLvlValue = roomY*roomTilesY + tileY;
                        Vector2Int tileCenter = new Vector2Int(tileXLvlValue, tileYLvlValue);
                        switch (rooms[roomX, roomY].roomTiles[tileX, tileY])
                        {
                            case Room.Tile.empty:
                                break;
                            case Room.Tile.floor:
                                floorTilemap.SetTile((Vector3Int)tileCenter, floorTile);
                                break;
                            case Room.Tile.wall:
                                wallTilemap.SetTile((Vector3Int)tileCenter, wallTile);
                                break;
                            default:
                                print("This shouldnt happen, send help! (Or check the room tile values of the scriptable object)");
                                break;
                        }
                    }
                }
            }
        }
        tileSw.Stop();
        print("Tiles assigned in: "+tileSw.ElapsedMilliseconds+"ms");
    }

    void AssignRoomTiles(List<Room> roomsToTile) {
        foreach(Room room in roomsToTile) {
            // Tiles in rooms.
            for (int tileX = 0; tileX < roomTilesX; tileX++) {
                for (int tileY = 0; tileY < roomTilesY; tileY++) {
                    int tileXLvlValue = room.indexX*roomTilesX + tileX;
                    int tileYLvlValue = room.indexY*roomTilesY + tileY;
                    Vector2Int tileCenter = new Vector2Int(tileXLvlValue, tileYLvlValue);
                    switch (rooms[room.indexX, room.indexY].roomTiles[tileX, tileY])
                    {
                        case Room.Tile.empty:
                            break;
                        case Room.Tile.floor:
                            floorTilemap.SetTile((Vector3Int)tileCenter, floorTile);
                            break;
                        case Room.Tile.wall:
                            wallTilemap.SetTile((Vector3Int)tileCenter, wallTile);
                            break;
                        default:
                            print("This shouldnt happen, send help! (Or check the room tile values of the scriptable object. ^_^)");
                            break;
                    }
                }
            }
        }
    }
    // Set tiles from a list of rooms to null.
    void ClearAllRoomTiles(List<Room> roomsToTile) {
        foreach(Room room in roomsToTile) {
            // Tiles in rooms.
            for (int tileX = 0; tileX < roomTilesX; tileX++) {
                for (int tileY = 0; tileY < roomTilesY; tileY++) {
                    int tileXLvlValue = room.indexX*roomTilesX + tileX;
                    int tileYLvlValue = room.indexY*roomTilesY + tileY;
                    Vector2Int tileCenter = new Vector2Int(tileXLvlValue, tileYLvlValue);
                    switch (rooms[room.indexX, room.indexY].roomTiles[tileX, tileY])
                    {
                        case Room.Tile.empty:
                            break;
                        case Room.Tile.floor:
                            floorTilemap.SetTile((Vector3Int)tileCenter, null);
                            break;
                        case Room.Tile.wall:
                            wallTilemap.SetTile((Vector3Int)tileCenter, null);
                            break;
                        default:
                            print("This shouldnt happen, send help! (Or check the room tile values of the scriptable object. ^_^)");
                            break;
                    }
                }
            }
        }
    }

    SO_TileRoomBase RandomRoomLayout() {
        return roomLayouts.TilesXByTilesY[Random.Range(0, roomLayouts.TilesXByTilesY.Length)];
    }

    // void OnDrawGizmos() {
    //     // Draw the rooms.
    //     Vector2 roomMidOffset = new Vector2(tileSize * roomTilesX / 2, tileSize * roomTilesY / 2);
    //     Vector2 roomSize = new Vector2(tileSize * roomTilesX, tileSize * roomTilesY);
    //     for (int x = 0; x < roomsX; x++) {
    //         for (int y = 0; y < roomsY; y++) {
    //             Vector3 center = new Vector3(centeredLvlBotLeft.x + x*roomSize.x + roomMidOffset.x, centeredLvlBotLeft.y + y*roomSize.y + roomMidOffset.y, 0);
    //             Gizmos.color = Color.red;
    //             Gizmos.DrawCube(center, roomSize * 0.9f);
    //             // Draw the tiles.
    //             Vector2 roomBotLeft = new Vector2(center.x-roomMidOffset.x, center.y-roomMidOffset.y);
    //             float midOffset = tileSize / 2;
    //             Vector3 tileSizeVector = new Vector3(tileSize, tileSize, tileSize);
    //             for (int tileX = 0; tileX < roomTilesX; tileX++) {
    //                 for (int tileY = 0; tileY < roomTilesY; tileY++) {
    //                     Vector3 tileCenter = new Vector3(roomBotLeft.x+(tileX*tileSize)+midOffset, roomBotLeft.y+(tileY*tileSize)+midOffset, -10);
    //                     //PickGizmosColor(tileX, tileY);
    //                     Gizmos.color = Color.blue;
    //                     Gizmos.DrawCube(tileCenter, tileSizeVector * 0.8f);
    //                 }
    //             }
    //         }
    //     }
    // }
}

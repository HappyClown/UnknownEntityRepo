using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    enum Tile { empty, floor, wall }
    Tile[,] roomTiles;
    // Premade room would be a 2d array of ints that gives information as to what is on a tile.
    // Legend: 0 = Empty, 1 = floor, 2 = wall, 3 = corridor, etc.
    // 5x5 room.
    // 0  2  2  2  0 
    // 3  1  1  1  2
    // 0  2  1  1  3
    // 2  1  1  1  2
    // 0  2  2  2  0

    int roomTilesX;
    int roomTilesY;
    float tileSize;
    Vector2 roomBotLeftPos;

    public Room(int _roomTilesX, int _roomTilesY, float _tileSize, Vector2 _roomBotLeftPos) {
        roomTilesX = _roomTilesX;
        roomTilesY = _roomTilesY;
        tileSize = _tileSize;
        roomBotLeftPos = _roomBotLeftPos;
    }

    //"Create" every tile by setting them to empty.
    public void CreateRoom() {
        roomTiles = new Tile[roomTilesX, roomTilesY];
        for (int x = 0; x < roomTilesX; x++) {
            for (int y = 0; y < roomTilesY; y++) {
                roomTiles[x,y] = Tile.empty;
            }
        }
    }
}

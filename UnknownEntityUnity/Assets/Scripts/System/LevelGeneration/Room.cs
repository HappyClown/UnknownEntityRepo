using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public enum Tile { empty, floor, wall }
    public Tile[,] roomTiles;
    // Premade room would be a 2d array of ints that gives information as to what is on a tile.
    // Legend: 0 = Empty, 1 = floor, 2 = wall, 3 = corridor, etc.
    // 5x5 room.
    // 0  2  2  2  0 
    // 3  1  1  1  2
    // 0  2  1  1  3
    // 2  1  1  1  2
    // 0  2  2  2  0
    public int roomTilesX;
    public int roomTilesY;
    float tileSize;
    Vector2 roomBotLeftPos;
    SO_TileRoomBase roomLayout;
    public int indexX, indexY;
    public int entrance, exit;

    public Room(int _roomTilesX, int _roomTilesY, float _tileSize, Vector2 _roomBotLeftPos, SO_TileRoomBase _roomLayout, int _indexX, int _indexY, int _entrance) {
        roomTilesX = _roomTilesX;
        roomTilesY = _roomTilesY;
        tileSize = _tileSize;
        roomBotLeftPos = _roomBotLeftPos;
        roomLayout = _roomLayout;
        indexX = _indexX;
        indexY = _indexY;
        entrance = _entrance;
    }

    //"Create" every tile by setting them to empty.
    public void CreateRoom() {
        roomTilesX = roomLayout.columns.Length;
        // columns[0] assuming every column has the same amount of rows. Otherwise this should be fed into the for(y) loop and checked everytime based on the for(x).
        roomTilesY = roomLayout.columns[0].amntOfRows.Length;
        roomTiles = new Tile[roomTilesX, roomTilesY];

        for (int x = 0; x < roomTilesX; x++) {
            for (int y = 0; y < roomTilesY; y++) {
                roomTiles[x,y] = (Tile)roomLayout.columns[x].amntOfRows[y];
                //print("Room #["+x+","+y+"] is a: "+roomTiles[x,y].ToString());
            }
        }
    }
}

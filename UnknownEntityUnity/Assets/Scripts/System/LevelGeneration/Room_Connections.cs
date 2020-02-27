using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Connections : MonoBehaviour
{
    // 0 || 2 = vertical = y
    // 1 || 3 = horizontal = x
    // Find the column/row of tiles closest to the exit direction
    // Pick 1 of those tiles, store the row/column,
    // Add from picked tile + 1 in the direction, to the edge of the room,
    // Do the same to its target room in the opposite direction.
    // prevRoom.maxTileY
    public LevelGrid lvlGrid;
    List<int> startRoomCorTiles, endRoomCorTiles = new List<int>();
    int corY, corX;
    public void MakeCorridor(Room startRoom, Room endRoom, int exit) {
        int firstTileX = 0;
        int firstTileY = 0;
        startRoomCorTiles = new List<int>();
        print("Exit: "+exit);
        // Horizontal corridor, exit 1. (right to left, bottom to top)
        if (exit == 1) {
            for (int x = startRoom.roomTilesX-1; x > 0; x--) {
                for (int y = 0; y < startRoom.roomTilesY; y++) {
                    if (startRoom.roomTiles[x, y] == Room.Tile.floor) {
                        firstTileX = x;
                        firstTileY = y;
                        y = startRoom.roomTilesY;
                        x =0;
                    }
                }
            }
        }
        else if (exit == 3) {
            // Horizontal corridor, exit 3. (left to right, bottom to top)
            for (int x = 0; x < startRoom.roomTilesX; x++) {
                for (int y = 0; y < startRoom.roomTilesY; y++) {
                    if (startRoom.roomTiles[x, y] == Room.Tile.floor) {
                        firstTileX = x;
                        firstTileY = y;
                        y = startRoom.roomTilesY;
                        x = startRoom.roomTilesX;
                    }
                }
            }
        }
        else if (exit == 0) {
            // Vertical corridor, exit 0. (top to bot, left to right)
            for (int y = startRoom.roomTilesY-1; y > 0; y--) {
                for (int x = 0; x < startRoom.roomTilesX; x++) {
                    //print("Index X: "+x+"Index Y: "+y);
                    if (startRoom.roomTiles[x, y] == Room.Tile.floor) {
                        firstTileX = x;
                        firstTileY = y;
                        y = 0;
                        x = startRoom.roomTilesX;
                    }
                }
            }
        }
        else {
            print("Wrong exit, shouldn't go down yet.");
        }

        Vector2 exitDir = lvlGrid.DirectionVector(exit);
        print("My exit direction: "+exitDir);
        int repeat = startRoom.roomTilesX - firstTileX;
        print("Amount of tile repeat: "+repeat);
        AssignTiles(startRoom, firstTileX, firstTileY, exitDir, repeat);
    }

    void AssignTiles(Room room, int tileX, int tileY, Vector2 _dir, int repeat) {
        // ROom number * tilesize + tile number * tile size;
        int tileXLvlValue = room.indexX*10 + tileX;
        int tileYLvlValue = room.indexY*10 + tileY;
        Vector2Int tilePos = new Vector2Int(tileXLvlValue, tileYLvlValue);
        Vector2Int dir = new Vector2Int((int)_dir.x, (int)_dir.y);
        for (int i = 0; i < repeat; i++) {
            tilePos += dir;
            lvlGrid.floorTilemap.SetTile((Vector3Int)tilePos, lvlGrid.floorTile);
            lvlGrid.wallTilemap.SetTile((Vector3Int)tilePos, null);
        }
    }
}

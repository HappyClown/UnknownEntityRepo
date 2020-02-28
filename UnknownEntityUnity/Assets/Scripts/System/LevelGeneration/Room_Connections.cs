using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Connections
{
    public LevelGrid lvlGrid;
    bool foundFloor = false;
    public void MakeCorridor(Room startRoom, Room endRoom, int exit) {
        int firstTileX = 0;
        int firstTileY = 0;
        int repeat = 0;
        // Horizontal corridor, exit 0. (right to left, bottom to top)
        if (exit == 0) {
            for (int y = startRoom.roomTilesY-1; y > 0; y--) {
                for (int x = 0; x < startRoom.roomTilesX; x++) {
                    if (startRoom.roomTiles[x, y] == Room.Tile.floor) {
                        firstTileX = x;
                        firstTileY = y;
                        y = 0;
                        x = startRoom.roomTilesX;
                        repeat = startRoom.roomTilesY - firstTileY-1;
                    }
                }
            }
        }
        // Horizontal corridor, exit 1. (right to left, top to bot)
        else if (exit == 1) {
            for (int x = startRoom.roomTilesX-1; x > 0; x--) {
                for (int y = 0; y < startRoom.roomTilesY; y++) {
                    if (startRoom.roomTiles[x, y] == Room.Tile.floor) {
                        firstTileX = x;
                        firstTileY = y;
                        y = startRoom.roomTilesY;
                        x =0;
                        repeat = startRoom.roomTilesX - firstTileX-1;
                    }
                }
            }
        }
        // Vertical corridor, exit 2. (bottom to top, left to right)
        else if (exit == 2) {
            for (int y = 0; y < startRoom.roomTilesY; y++) {
                for (int x = 0; x < startRoom.roomTilesX; x++) {
                    if (startRoom.roomTiles[x, y] == Room.Tile.floor) {
                        firstTileX = x;
                        firstTileY = y;
                        y = startRoom.roomTilesY;
                        x = startRoom.roomTilesX;
                        repeat = startRoom.roomTilesY - firstTileY-1;
                    }
                }
            }
        }
        // Horizontal corridor, exit 3. (left to right, bottom to top)
        else if (exit == 3) {
            for (int x = 0; x < startRoom.roomTilesX; x++) {
                for (int y = 0; y < startRoom.roomTilesY; y++) {
                    if (startRoom.roomTiles[x, y] == Room.Tile.floor) {
                        firstTileX = x;
                        firstTileY = y;
                        // Check if corridor will connect. If yes, loop out, if no, jump 2 in the appriate direction and continue.
                        y = startRoom.roomTilesY;
                        x = startRoom.roomTilesX;
                        repeat = firstTileX;
                    }
                }
            }
        }
        Vector2 exitDir = lvlGrid.DirectionVector(exit);
        AssignTiles(startRoom, endRoom, firstTileX, firstTileY, exitDir, repeat);
    }

    int CheckLineConnection(Room _endRoom, int roomTileX, int roomTileY, Vector2Int _dir, Vector2 origDir) {
        // Get the first tile pos vector.
        Vector2Int roomTilePos = new Vector2Int(roomTileX, roomTileY);
        Vector2Int dir = _dir;
        int tilesToFloor = 0;
        // Check to see if there is a floor tile in this direction.
        while (_endRoom.roomTiles[roomTilePos.x,roomTilePos.y] != Room.Tile.floor) {
            roomTilePos += dir;
            // If im outside of the room's bounds break out of the while loop.
            if (roomTilePos.x < 0 || roomTilePos.x >= 10 || roomTilePos.y < 0 || roomTilePos.y >= 10) {
                break;
            }
            tilesToFloor++;
            if (_endRoom.roomTiles[roomTilePos.x,roomTilePos.y] == Room.Tile.floor) {
                foundFloor = true;
                Debug.Log("Found a tile: "+tilesToFloor+" tiles away from main our start tile headin': "+dir);
                break;
            }
            // Check around.
        }
        return tilesToFloor;
    }

    void CheckAroundTile() {

    }

    (Vector2Int, Vector2Int) GetOtherDirs(Vector2 _dir) {
        Vector2Int secondDir = Vector2Int.zero;
        Vector2Int thirdDir = Vector2Int.zero;
        if (_dir == Vector2.up) {
            secondDir = Vector2Int.right;
            thirdDir = Vector2Int.left;
        }
        else if (_dir == Vector2.right) {
            secondDir = Vector2Int.up;
            thirdDir = Vector2Int.down;
        }
        else if (_dir == Vector2.down) {
            secondDir = Vector2Int.right;
            thirdDir = Vector2Int.left;
        }
        else {
            secondDir = Vector2Int.down;
            thirdDir = Vector2Int.up;
        }
        return (secondDir, thirdDir);
    }

    void AssignTiles(Room _startRoom, Room _endRoom, int startTileX, int startTileY, Vector2 _dir, int repeat) {
        // Room number * tilesize + tile number;
        // Get the start room's tile.
        int tileXLvlValue = _startRoom.indexX*10 + startTileX;
        int tileYLvlValue = _startRoom.indexY*10 + startTileY;
        Vector2Int gridTilePos = new Vector2Int(tileXLvlValue, tileYLvlValue);
        Vector2Int dir = new Vector2Int((int)_dir.x, (int)_dir.y);
        // Put down floor tiles in a straight line until the end of the start room.
        for (int i = 0; i < repeat; i++) {
            gridTilePos += dir;
            lvlGrid.floorTilemap.SetTile((Vector3Int)gridTilePos, lvlGrid.floorTile);
            lvlGrid.wallTilemap.SetTile((Vector3Int)gridTilePos, null);
        }
        // Move an extra tile to be in the end room.
        gridTilePos += dir;
        // Get the end room tile.
        int endRoomTileX = gridTilePos.x - _endRoom.indexX*10;
        int endRoomTileY = gridTilePos.y - _endRoom.indexY*10;
        // Get the other directions to check (not back).
        Vector2Int secondDir = Vector2Int.zero;
        Vector2Int thirdDir = Vector2Int.zero;
        (secondDir, thirdDir) = GetOtherDirs(dir);
        //Debug.Log("Second direction: "+secondDir+", third direction: "+thirdDir);
        // Start putting floor tiles in the end room in a line, at every tile check the side lines (other directions) for floor tiles.
        int reps = 0;
        int tilesToFloor = 0;
        while (_endRoom.roomTiles[endRoomTileX,endRoomTileY] != Room.Tile.floor) {
            // If this tile is not a floor tile, check all the tiles in a line on both sides.
            if (_endRoom.roomTiles[endRoomTileX,endRoomTileY] != Room.Tile.floor) {
                tilesToFloor = CheckLineConnection(_endRoom, endRoomTileX, endRoomTileY, secondDir, dir);
                if (foundFloor) {
                    //Debug.Log("Well il be damned, we found sum'in boss. What now?");
                    ApplyTilesInLine(tilesToFloor, secondDir, gridTilePos);
                    foundFloor = false;
                    break;
                }
                tilesToFloor = CheckLineConnection(_endRoom, endRoomTileX, endRoomTileY, thirdDir, dir);
                if (foundFloor) {
                    //Debug.Log("Well il be damned, we found sum'in boss. What now?");
                    ApplyTilesInLine(tilesToFloor, thirdDir, gridTilePos);
                    foundFloor = false;
                    break;
                }
            }
            // Remove wall tile, apply floor tile.
            lvlGrid.floorTilemap.SetTile((Vector3Int)gridTilePos, lvlGrid.floorTile);
            lvlGrid.wallTilemap.SetTile((Vector3Int)gridTilePos, null);
            // Get the next room tile position.
            gridTilePos += dir;
            endRoomTileX = gridTilePos.x - _endRoom.indexX*10;
            endRoomTileY = gridTilePos.y - _endRoom.indexY*10;
            // Check if im now outside of the room.
            reps++;
            if (reps == 10) {break;}
        }
    }

    void ApplyTilesInLine(int tilesToFloor, Vector2Int dir, Vector2Int _gridTilePos) {
        Debug.Log("Im paving floor tiles. In a straight line.");
        Vector2Int gridTilePos = _gridTilePos;
        // Remove wall tile, apply floor tile.
        for (int i = 0; i < tilesToFloor; i++) {
            lvlGrid.floorTilemap.SetTile((Vector3Int)gridTilePos, lvlGrid.floorTile);
            lvlGrid.wallTilemap.SetTile((Vector3Int)gridTilePos, null);
            gridTilePos += dir;
        }
    }
}

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
        // Horizontal corridor, exit 1.
        for (int x = startRoom.roomTilesX; x > 0; x--) {
            for (int y = 0; y < startRoom.roomTilesY; y++) {
                if (startRoom.roomTiles[x, y] == Room.Tile.floor) {
                    firstTileX = x;
                    firstTileY = y;
                    y = startRoom.roomTilesY;
                    x =0;
                }
            }
        }
        Vector2 exitDir = lvlGrid.DirectionVector(exit);
        int repeat = startRoom.roomTilesX - firstTileX;
        AssignTiles(firstTileX, firstTileY, exitDir, repeat);
    }

    void AssignTiles(int tileX, int tileY, Vector2 _dir, int repeat) {
        Vector2Int tilePos = new Vector2Int(tileX, tileY);
        Vector2Int dir = new Vector2Int((int)_dir.x, (int)_dir.y);
        for (int i = 0; i < repeat; i++) {
            tilePos += dir;
            lvlGrid.floorTilemap.SetTile((Vector3Int)tilePos, lvlGrid.floorTile);
            lvlGrid.wallTilemap.SetTile((Vector3Int)tilePos, null);
        }
    }
}

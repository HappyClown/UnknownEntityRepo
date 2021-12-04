using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelTile
{
    public bool noTile;
    public bool occupiedByObject = false;
    public bool groundTile;
    public bool wallTile;
    public bool isWallTop;
    public bool isGroundCornerTile;
    public Tilemap myVisibleTileMap {get; set; }
    public Vector3Int tilePos {get; set; }
    public LevelTile topNeighbor, rightNeighbor, bottomNeighbor, leftNeighbor;
    // Constructor.
    public LevelTile (Vector3Int _tilePos) {
        tilePos = _tilePos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Diagnostics;

public class TilemapDressing : MonoBehaviour
{
    public Tilemap[] tilemaps; // Tilemaps organised from bottom to top
    public int wallTilemapIndex;
    public Tilemap wallTilemap;
    //public Tilemap groundTilemap;
    //public int[] groundTilemapIndex;
    public LevelTile[,] levelTiles;
    public List<LevelTile> groundLevelTiles = new List<LevelTile>();
    public List<LevelTile> wallLevelTiles = new List<LevelTile>();
    public List<Vector3> tileWorldLocations = new List<Vector3>();
    public List<LevelTile> wallTopTiles = new List<LevelTile>();
    public bool runTheWholeShabang = false;
    private int maxTileBoundSizeX = 0;
    private int maxTileBoundSizeY = 0;

    [Header("Testing Purposes")]
    public bool debug;
    public GameObject pointerGO;
    public GameObject grassGO;
    public Transform grassParent;
    public Vector3 tileCenterOffset = new Vector3(0.5f, 0.5f, 0f);
    public float tuftCircleRadius = 1f;
    public int amountOfTufts = 5;
    public GameObject zeroGO, oneGO, twoGO;
    public GameObject cornerGO, groundTileGO, wallTileGO, wallTopGO;
    [Header("Dressing Objects")]
    public GameObject candleClusterGO;
    public int amntOfCandleClusters;

    void Start() {
    }

    void Update() {
        if (runTheWholeShabang) {
            //GetTiles();
            StartCoroutine(GetTileInfoRoutine());
            runTheWholeShabang = false;
        }
    }

    public void GetMaxCellBounds() {
        foreach(Tilemap tilemap in tilemaps) {
            tilemap.CompressBounds();
            if (tilemap.cellBounds.size.x > maxTileBoundSizeX) {
                maxTileBoundSizeX = tilemap.cellBounds.size.x;
            }
            if (tilemap.cellBounds.size.y > maxTileBoundSizeY) {
                maxTileBoundSizeY = tilemap.cellBounds.size.y;
            }
        }
    }

    public IEnumerator GetTileInfoRoutine() {
        // var timer = new Stopwatch();
        // timer.Start();
        // Get the largest possible tilemap boundary.
        GetMaxCellBounds();
        // Establish the tile array size (how many tiles).
        levelTiles = new LevelTile[50, 50];
        // Like the A grid grid, start from 0,0 (with an offset of whatever is needed minx miny) then go row by row using GetTile on every tile to get its tilemap.
        for (int y = 0; y < 50; y++)
        {
            for (int x = 0; x < 50; x++)
            {
                // print("-------------NEW TILE--------------");
                bool addedGroundTile = false;
                levelTiles[x,y] = new LevelTile(new Vector3Int(x-25, y-25));
                // Check if there is a tile here on each tilemap.
                foreach (Tilemap tilemap in tilemaps)
                {
                    //Offset decided by; minus MaxCompressedbounds divided by 2 (needs to be automated).
                    if (tilemap.GetTile(new Vector3Int(x-25, y-25))) {
                        // Because of the order in the Tilemaps list, the walls will be the last ones checked therefore even if there is a ground level tilemap under the wall the wall will still be the visible tilemap.
                        levelTiles[x,y].myVisibleTileMap = tilemap;
                        if (tilemap != tilemaps[wallTilemapIndex] && !addedGroundTile) {
                            groundLevelTiles.Add(levelTiles[x,y]);
                            addedGroundTile = true;
                        }
                        if (tilemap == tilemaps[wallTilemapIndex]) {
                            if (addedGroundTile) {
                                groundLevelTiles.RemoveAt(groundLevelTiles.Count-1);
                            }
                            wallLevelTiles.Add(levelTiles[x,y]);
                        }
                    }
                }
                if (debug) {
                    pointerGO.transform.position = new Vector2(x-25f, y-24.5f);
                    //yield return null;
                }
            }
        }
        // Go through the grid again row by row this time assigning neighbors based on the tilemaps.
        for (int y = 1; y < 49; y++)
        {
            for (int x = 1; x < 49; x++)
            {
                    if (levelTiles[x,y+1] != null) {
                        levelTiles[x,y].topNeighbor = levelTiles[x,y+1];
                    }
                    if (levelTiles[x+1,y] != null) {
                        levelTiles[x,y].rightNeighbor = levelTiles[x+1,y];
                    }
                    if (levelTiles[x,y-1] != null) {
                        levelTiles[x,y].bottomNeighbor = levelTiles[x,y-1];
                    }
                    if (levelTiles[x-1,y] != null) {
                        levelTiles[x,y].leftNeighbor = levelTiles[x-1,y];
                    }
                if (debug) {
                    pointerGO.transform.position = new Vector2(x-25f, y-24.5f);
                    //yield return null;
                }
            }
        }
        if (debug) {
            // This is only to identify that the tile has assigned the correct tilemap.
            foreach(LevelTile levelTile in levelTiles) {
                if (levelTile != null) {
                    DisplayTilemapNumber(levelTile);
                    //yield return null;
                }
            }
        }
        if (debug) {
            DisplayWallTiles();
            DisplayGroundTiles();
        }
        FindGroundCornerTiles();
        FindWallTops();
        // timer.Stop();
        // print("Time to do all the tile info gathering: "+timer.Elapsed);
        yield return null;
    }

    public void FindGroundCornerTiles() {
        // Identify corners.
        int wallCount = 0;
        foreach (LevelTile groundLevelTile in groundLevelTiles)
        {
            if (groundLevelTile.topNeighbor.myVisibleTileMap == wallTilemap) {
                wallCount++;
            }
            if (groundLevelTile.rightNeighbor.myVisibleTileMap == wallTilemap) {
                wallCount++;
            }
            if (groundLevelTile.bottomNeighbor.myVisibleTileMap == wallTilemap) {
                wallCount++;
            }
            if (groundLevelTile.leftNeighbor.myVisibleTileMap == wallTilemap) {
                wallCount++;
            }
            // if (wallCount == 1) {
            //     Instantiate(cornerGO, new Vector3(groundLevelTile.tilePos.x+0.5f, groundLevelTile.tilePos.y+0.5f), Quaternion.identity);
            // }
            if (wallCount == 2) {
                if (debug) {
                    Instantiate(cornerGO, new Vector3(groundLevelTile.tilePos.x+0.5f, groundLevelTile.tilePos.y+0.5f), Quaternion.identity);
                }
            }
            wallCount = 0;
        }
    }

    // Get a list of all the wallTop tiles.
    void FindWallTops() {
        foreach (LevelTile wallLeveltile in wallLevelTiles)
        {
            if (wallLeveltile.topNeighbor.myVisibleTileMap == wallTilemap) {
                Vector3Int topNeighborPos = new Vector3Int(wallLeveltile.tilePos.x, wallLeveltile.tilePos.y+1);
                if (debug) {
                    Instantiate(wallTopGO, new Vector3(topNeighborPos.x+0.5f, topNeighborPos.y+0.5f), Quaternion.identity);
                }
                // To get the right levelTile from a tilePos, need to add back the offset.
                levelTiles[topNeighborPos.x+25, topNeighborPos.y+26].isWallTop = true;
                wallTopTiles.Add(wallLeveltile.topNeighbor);
            }
        }
        // // Randomly assign clusters to wall tops in the list, making sure the tile is unoccupied.
        // for(int i = 0; i < amntOfCandleClusters; i++) {
        //     int rand = Random.Range(0, wallTopTiles.Count);
        //     if(!wallTopTiles[rand].occupiedByObject) {
        //         Instantiate(candleClusterGO, new Vector3(wallTopTiles[rand].tilePos.x, wallTopTiles[rand].tilePos.y), Quaternion.identity);
        //         wallTopTiles[rand].occupiedByObject = true;
        //     }
        //     else {
        //         print("Candle cluster could not spawn, tile was occupado!");
        //     }
        // }
    }
    public LevelTile GetRandomWallTopTile() {
        int rand = -1;
        bool tileFound = false;
        while (!tileFound){
            rand = Random.Range(0, wallTopTiles.Count);
            if(!wallTopTiles[rand].occupiedByObject) {
                wallTopTiles[rand].occupiedByObject = true;
                tileFound = true;
            }
            else {
                print("Tile was occupado! Trying again.");
            }
        }
        return wallTopTiles[rand];
    }


    public void DisplayWallTiles() {
        foreach (LevelTile wallLevelTile in wallLevelTiles)
        {
            Vector3 offset = new Vector3(0.5f, 0.5f);
            Instantiate(wallTileGO, wallLevelTile.tilePos + offset, Quaternion.identity);
        }
    }

    public void DisplayGroundTiles() {
        foreach (LevelTile groundLevelTile in groundLevelTiles)
        {
            Vector3 offset = new Vector3(0.5f, 0.5f);
            Instantiate(groundTileGO, groundLevelTile.tilePos + offset, Quaternion.identity);
        }
    }

    public void DisplayTilemapNumber(LevelTile _leveltile) {
        if (_leveltile.myVisibleTileMap == tilemaps[0]) {
            Instantiate(zeroGO, _leveltile.tilePos, Quaternion.identity);
        }
        else if (_leveltile.myVisibleTileMap == tilemaps[1]) {
            Instantiate(oneGO, _leveltile.tilePos, Quaternion.identity);
        }
        else if (_leveltile.myVisibleTileMap == tilemaps[2]) {
            Instantiate(twoGO, _leveltile.tilePos, Quaternion.identity);
        }
    }

}

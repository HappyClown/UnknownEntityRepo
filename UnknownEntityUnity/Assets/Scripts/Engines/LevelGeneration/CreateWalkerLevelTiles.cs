using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class CreateWalkerLevelTiles : MonoBehaviour
{
    // Size of the grid in Unity units.
    public Vector2 tileGridWorldSize = new Vector2(10, 10);
    public float tileRadius = 0.5f;
    float tileDiameter;
    int gridSizeX;
    int gridSizeY;

    enum Tile{empty, floor, wall}
    Tile[,] gridTiles;

    struct walker{
        public Vector2 dir;
        public Vector2 pos;
    }
    List<walker> walkers;
    public float walkerChangeDirPerc = 0.5f;
    public float walkerDestroyPerc = 0.05f;
    public float walkerSpawnPerc = 0.05f;
    public int maxWalkers = 10;
    public float gridPercToFill;
    public GameObject tileObj, wallObj;

    public void SetupCreateLevel() {
        Stopwatch levelSW = new Stopwatch();
        levelSW.Start();
        Setup();
        CreateFloors();
        CreateWalls();
        SpawnLevel();
        levelSW.Stop();
        print("Walker room created in: " + levelSW.ElapsedMilliseconds + "ms");
    }

    void Setup() {
        tileDiameter = tileRadius*2;
        gridSizeX = Mathf.RoundToInt(tileGridWorldSize.x / tileDiameter);
        gridSizeY = Mathf.RoundToInt(tileGridWorldSize.y / tileDiameter);
        gridTiles = new Tile[gridSizeX, gridSizeY];
        for (int x = 0; x < gridSizeX-1; x++) {
            for (int y = 0; y < gridSizeY-1; y++) {
                gridTiles[x,y] = Tile.empty;
            }
        }
        // Setup the first walker in the middle of the grid.
        walkers = new List<walker>();
        walker newWalker = new walker();
        newWalker.dir = RandomDirection();
        Vector2 spawnPos = new Vector2(Mathf.RoundToInt(gridSizeX/2f), Mathf.RoundToInt(gridSizeY/2f));
        newWalker.pos = spawnPos;
        walkers.Add(newWalker);
    }
    // Mark grid spaces as floor.
    void CreateFloors() {
        int iterations = 0;
        do {
            // Mark grid tile as floor at every walker position.
            foreach (walker myWalker in walkers) {
                gridTiles[(int)myWalker.pos.x, (int)myWalker.pos.y] = Tile.floor;
            }
            // Chance to destroy a walker.
            int numberChecks = walkers.Count;
            for (int i = 0; i < numberChecks; i++) {
                if (walkers.Count > 1 && Random.value < walkerDestroyPerc) {
                    walkers.RemoveAt(i);
                    break;
                }
            }
            // Chance for walker to chance direction.
            for (int i = 0; i < walkers.Count; i++) {
                if (Random.value < walkerChangeDirPerc) {
                    walker thisWalker = walkers[i];
                    thisWalker.dir = RandomDirection();
                    walkers[i] = thisWalker;
                }
            }
            // Chance to spawn a new walker.
            numberChecks = walkers.Count;
            for (int i = 0; i < numberChecks; i++) {
                if (Random.value < walkerSpawnPerc && walkers.Count < maxWalkers) {
                    walker newWalker = new walker();
                    newWalker.dir = RandomDirection();
                    newWalker.pos = walkers[i].pos;
                    walkers.Add(newWalker);
                }
            }
            // Move walkers.
            for (int i = 0; i < walkers.Count; i++) {
                walker thisWalker = walkers[i];
                thisWalker.pos += thisWalker.dir;
                walkers[i] = thisWalker;
            }
            // Avoid boarders of the grid to leave at least 1 tile for walls.
            for (int i = 0; i < walkers.Count; i++) {
                walker thisWalker = walkers[i];
                thisWalker.pos.x = Mathf.Clamp(thisWalker.pos.x, 1, gridSizeX-2); // -(nodeDiameter*2) ???
                thisWalker.pos.y = Mathf.Clamp(thisWalker.pos.y, 1, gridSizeY-2);
                walkers[i] = thisWalker;
            }
            if ((float)NumberOfFloors() / (float)gridTiles.Length > gridPercToFill) {
                break;
            }
            iterations++;
        }while(iterations < 9999);
    }
    // Mark grid spaces as wall.
    void CreateWalls() {
        for (int x = 0; x < gridSizeX-1; x++) {
            for (int y = 0; y < gridSizeY-1; y++) {
                if (gridTiles[x, y] == Tile.floor) {
                    if (gridTiles[x, y+1] == Tile.empty) {
                        gridTiles[x, y+1] = Tile.wall;
                    }
                    if (gridTiles[x, y-1] == Tile.empty) {
                        gridTiles[x, y-1] = Tile.wall;
                    }
                    if (gridTiles[x+1, y] == Tile.empty) {
                        gridTiles[x+1, y] = Tile.wall;
                    }
                    if (gridTiles[x-1, y] == Tile.empty) {
                        gridTiles[x-1, y] = Tile.wall;
                    }
                }
            }
        }
    }

    void SpawnLevel() {
        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {
                switch(gridTiles[x,y]) {
                    case Tile.empty:
                        break;
                    case Tile.floor:
                        Spawn(x, y, tileObj);
                        break;
                    case Tile.wall:
                        Spawn(x, y, wallObj);
                        break;
                }
            }
        }
    }

    void Spawn(float x, float y, GameObject toSpawn) {
        Vector2 offset = new Vector2((tileGridWorldSize.x/2.0f)-tileRadius, (tileGridWorldSize.y/2.0f)-tileRadius);
        Vector2 spawnPos = new Vector2(x, y) * tileDiameter - offset;
        Instantiate(toSpawn, spawnPos, Quaternion.identity);
    }

    int NumberOfFloors() {
        int count = 0;
        foreach(Tile tile in gridTiles) {
            if (tile == Tile.floor) {
                count++;
            }
        }
        return count;
    }

    Vector2 RandomDirection() {
        int choice = Mathf.FloorToInt(Random.value * 3.99f);
        switch(choice){
            case 0:
                return Vector2.up;
            case 1:
                return Vector2.right;
            case 2:
                return Vector2.down;
            default:
                return Vector2.left;
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(tileGridWorldSize.x*tileDiameter/2, tileGridWorldSize.y*tileDiameter/2, 1f));
    }

}

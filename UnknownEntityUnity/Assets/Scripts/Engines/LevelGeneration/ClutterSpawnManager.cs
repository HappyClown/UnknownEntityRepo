using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClutterSpawnManager : MonoBehaviour
{
    public TilemapDressing tilemapDressing;
    [Header("WallTop Clutter")]
    public List<GameObject> smallWallTopClutterGOs = new List<GameObject>();
    public int smallWallTopPointCost = 5, smallWallTopPointMinimum = 5, smallWallTopPointGuarantee = 5;
    public List<GameObject> mediumWallTopClutterGOs = new List<GameObject>();
    public int mediumWallTopPointCost = 10, mediumWallTopPointMinimum = 20, mediumWallTopPointGuarantee = 20;
    public List<GameObject> largeWallTopClutterGOs = new List<GameObject>();
    public int largeWallTopPointCost = 20, largeWallTopPointMinimum = 30, largeWallTopPointGuarantee = 30;
    public int wallTopPointTotal;
    public int wallTopPointsLeft;
    private int wallTopPointThreshold;

    void Start() {
        StartCoroutine(SpawnWallTopClutter());
    }

    void GetWallTopPointTotal() {
        wallTopPointTotal = tilemapDressing.wallTopTiles.Count;
        wallTopPointsLeft = wallTopPointTotal;
        print("Total amount of WallTop clutter points: "+wallTopPointsLeft);
    }

    IEnumerator SpawnWallTopClutter() {
        yield return new WaitForSeconds(2f);
        GetWallTopPointTotal();
        // Go through the guaranteed spawn rule, starting from the smallest clutter point cost check if there are more points left then the pointGuarantee amount.
        if (wallTopPointsLeft >= smallWallTopPointGuarantee) {
            //Spawn one small clutter object on a random tile.
            Vector3Int spawnPos = tilemapDressing.GetRandomWallTopTile().tilePos;
            int rand = Random.Range(0, smallWallTopClutterGOs.Count);
            // CHANGE to grab object from pool.******************************************
            Instantiate(smallWallTopClutterGOs[rand], spawnPos, Quaternion.identity);
            wallTopPointsLeft -= smallWallTopPointCost;
            print("WallTop Clutter Points left: "+wallTopPointsLeft);
            AdjustWallTopPointThreshold();
        }
        if (wallTopPointsLeft >= mediumWallTopPointGuarantee) {
            //Spawn one medium clutter object on a random tile.
            Vector3Int spawnPos = tilemapDressing.GetRandomWallTopTile().tilePos;
            int rand = Random.Range(0, mediumWallTopClutterGOs.Count);
            // CHANGE to grab object from pool.******************************************
            Instantiate(mediumWallTopClutterGOs[rand], spawnPos, Quaternion.identity);
            wallTopPointsLeft -= mediumWallTopPointCost;
            print("WallTop Clutter Points left: "+wallTopPointsLeft);
            AdjustWallTopPointThreshold();
        }
        if (wallTopPointsLeft >= largeWallTopPointGuarantee) {
            //Spawn one large clutter object on a random tile.
            Vector3Int spawnPos = tilemapDressing.GetRandomWallTopTile().tilePos;
            int rand = Random.Range(0, largeWallTopClutterGOs.Count);
            // CHANGE to grab object from pool.******************************************
            Instantiate(largeWallTopClutterGOs[rand], spawnPos, Quaternion.identity);
            wallTopPointsLeft -= largeWallTopPointCost;
            print("WallTop Clutter Points left: "+wallTopPointsLeft);
            AdjustWallTopPointThreshold();
        }
        // Start spawning walltop GOs randomly until there are not enough points left.
        while (wallTopPointsLeft > smallWallTopPointCost) {
            Vector3Int spawnPos = tilemapDressing.GetRandomWallTopTile().tilePos;
            Instantiate(GetRandomWallTopGO(), spawnPos, Quaternion.identity);
            print("WallTop Clutter Points left: "+wallTopPointsLeft);
            AdjustWallTopPointThreshold();
        }
        yield return null;
    }

    void AdjustWallTopPointThreshold () {
        if (wallTopPointsLeft < mediumWallTopPointMinimum) {
            wallTopPointThreshold = 1;
        }
        else if (wallTopPointsLeft < largeWallTopPointMinimum) {
            wallTopPointThreshold = 2;
        }
        else {
            wallTopPointThreshold = 3;
        }
    }
    GameObject GetRandomWallTopGO () {
        // Randomize from the available thresholds.
        int rand = Random.Range(1, wallTopPointThreshold+1);
        if (rand == 3) {
            rand = Random.Range(0, largeWallTopClutterGOs.Count);
            wallTopPointsLeft -= largeWallTopPointCost;
            print("Returning LARGE clutter.");
            return largeWallTopClutterGOs[rand];
        }
        else if (rand == 2) {
            rand = Random.Range(0, mediumWallTopClutterGOs.Count);
            wallTopPointsLeft -= mediumWallTopPointCost;
            print("Returning MEDIUM clutter.");
            return mediumWallTopClutterGOs[rand];
        }
        else if (rand == 1) {
            rand = Random.Range(0, smallWallTopClutterGOs.Count);
            wallTopPointsLeft -= smallWallTopPointCost;
            print("Returning SMALL clutter.");
            return smallWallTopClutterGOs[rand];
        }
        return null;
    }
}

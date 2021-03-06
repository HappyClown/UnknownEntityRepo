using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PremadeRoom : MonoBehaviour
{
    public Transform[] potentialDoors;
    public Transform[] potentialDoorsUp;
    public Transform[] potentialDoorsRight;
    public Transform[] potentialDoorsDown;
    public Transform[] potentialDoorsLeft;
    public Transform[] destructibles;
    public Transform[] obstacles;
    public Transform[] enemySpawnPoints;
    public Transform[] penaltyTiles;
    public Tile floorTile;
    public Tile wallTile;
    public Tilemap floorTileMap;
    public Tilemap wallTileMap;
    public Grid grid;
}
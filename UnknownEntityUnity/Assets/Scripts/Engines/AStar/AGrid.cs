﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Diagnostics;

public class AGrid : MonoBehaviour
{
    public bool displayGridGizmos;
    [Range(0, 1)]
    public float gizmosCubeSizePercent;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public TerrainType[] walkableRegions;
    public int obstacleProximityPenalty = 10;
    public int blurredPenaltyMapAmount = 3;
    private LayerMask walkableMask;
    private Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();

    public Tilemap[] groundTilemaps;
    public Grid tilemapGrid;

    private Node[,] grid;
    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    private int penaltyMin = int.MaxValue;
    private int penaltyMax = int.MinValue;

    public void SetupCreateGrid() {
        nodeDiameter = nodeRadius*2;
        // How many nodes fit in the grid on X and Y.
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        foreach(TerrainType region in walkableRegions) {
            walkableMask.value |= region.terrainMask.value;
            walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
        }

        CreateGrid();
    }

    public int MaxSize {
        get {
            return gridSizeX * gridSizeY;
        }
    }

    // Create a grid starting from the bottom left (0,0).
    void CreateGrid () {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = this.transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.up * gridWorldSize.y/2;

        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {
                // World coordinates of the node's center point.
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                
                bool walkable = true;
                
                // Check the ground tiles maps and see if there is a tile, if not > walkable = false
                int atLeastOne = 0;
                Vector3Int cellPos = tilemapGrid.WorldToCell(worldPoint);
                foreach (Tilemap tilemap in groundTilemaps) {
                    if (tilemap.GetTile(cellPos) != null) {
                        atLeastOne++;
                    }
                }
                if (atLeastOne == 0) {
                    walkable = false;
                }

                // Check if the node is "walkable" by checking if it overlaps with a 2DCollider on the Unwalkable layer.
                //walkable = !(Physics2D.OverlapBox(worldPoint, new Vector2(nodeRadius, nodeRadius), 0,unwalkableMask));
                if (Physics2D.OverlapBox(worldPoint, new Vector2(nodeRadius, nodeRadius), 0,unwalkableMask)) {
                    walkable = false;
                }
                int movementPenalty = 0;

                RaycastHit2D hit = Physics2D.Raycast(worldPoint + Vector3.forward, Vector3.back * 2, 100, walkableMask);
                //Debug.DrawRay(worldPoint + Vector3.forward, Vector3.back * 2, Color.green, 60f);
                if (hit) {
                    walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                }

                if (!walkable) {
                    movementPenalty += obstacleProximityPenalty;
                }
                
                // Fill the 2D grid array with the node's x and y grid coordinates.
                grid[x, y] = new Node(walkable, worldPoint, x, y, movementPenalty);
            }
        }

        BlurPenaltyMap(blurredPenaltyMapAmount);
        //sw.Stop();
        //print ("Grid created in: " + sw.ElapsedMilliseconds + "ms");
    }

    void BlurPenaltyMap(int blurSize) {
        int kernelSize = blurSize * 2 + 1;
        int kernelExtents = (kernelSize - 1) / 2;

        int[,] penaltiesHorizontalPass = new int[gridSizeX, gridSizeY];
        int[,] penaltiesVerticalPass = new int[gridSizeX, gridSizeY];

        for (int y = 0; y < gridSizeY; y++) {
            for (int x = -kernelExtents; x <= kernelExtents; x++) {
                int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                penaltiesHorizontalPass[0, y] += grid[sampleX, y].movementPenalty;
            }

            for (int x = 1; x < gridSizeX; x++) {
                int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
                int addIndex = Mathf.Clamp(x + kernelExtents, 0 , gridSizeX - 1);

                penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] - grid[removeIndex, y].movementPenalty + grid[addIndex, y].movementPenalty;
            }
        }

        for (int x = 0; x < gridSizeX; x++) {
            for (int y = -kernelExtents; y <= kernelExtents; y++) {
                int sampleY = Mathf.Clamp(y, 0, kernelExtents);
                penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
            }

            int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (kernelSize * kernelSize));
            grid[x, 0].movementPenalty = blurredPenalty;

            for (int y = 1; y < gridSizeY; y++) {
                int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);
                int addIndex = Mathf.Clamp(y + kernelExtents, 0 , gridSizeY - 1);

                penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y - 1] - penaltiesHorizontalPass[x, removeIndex] + penaltiesHorizontalPass[x, addIndex];
                blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernelSize * kernelSize));
                grid[x, y].movementPenalty = blurredPenalty;

                if (blurredPenalty > penaltyMax) {
                    penaltyMax = blurredPenalty;
                }
                if (blurredPenalty < penaltyMin) {
                    penaltyMin = blurredPenalty;
                }
            }
        }
    }

    // Get a List of a node's neighbouring tiles.
    public List<Node> GetNeighbours(Node node) {
        List<Node> neighbours = new List<Node>();
        // Starting from the node's bottom left neighbour (-1,-1), the node being (0,0), to its top right neighbour (1,1).
        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                // Use (x==0 && y==0) if you want to check for diagonals.
                // if (x == 0 && y == 0) {
                if (x == 0 && y == 0 || x == 1 && y == 1 || x == 1 && y == -1 || x == -1 && y == -1 || x == -1 && y == 1) {
                    continue;
                }
                // (checkX, checkY) being the neighbor's grid coordinate.
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                // Make sure that the neighbour is on the grid. Between (0,0) and (gridSizeX, gridSizeY).
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    } 
    // Get a node's grid coordinate from its Vector3 world position.
    public Node NodeFromWorldPoint (Vector3 worldPosition) {
        float percentX = (worldPosition.x + gridWorldSize.x/2) / (gridWorldSize.x);
        float percentY = (worldPosition.y + gridWorldSize.y/2) / (gridWorldSize.y);
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x,y];
    }
    /// <summary>
    /// Get the last available node in a given direction by stepping through one node at a time until a node is unwalkable returning the last available node or until the maximum number of steps has been reached returning the last node.
    /// </summary>
    /// <param name="normDirection">The direction to step in, should be the equivalent of Vector2.up/down/left/right.</param>
    public Node AvailableSteps(Node node, Vector2Int normDirection, int MaxSteps) {
        Vector2Int nodeGridCoord = new Vector2Int(node.gridX, node.gridY);
        // Return null if the starting node is not walkable.
        if (!node.walkable) {
            return null;
        }
        Node lastWalkableNode = node;
        // Start stepping in the given direction and checking to see if the nodes are walkable.
        for (int i = 0; i < MaxSteps; i++) {
            // Step in the direction.
            nodeGridCoord += normDirection;
            // If the node is not on the grid return the last walkable node.
            if (nodeGridCoord.x < 0 || nodeGridCoord.x >= gridSizeX || nodeGridCoord.y < 0 || nodeGridCoord.y >= gridSizeY) {
                return lastWalkableNode;
            }
            // Return the last walkable node if the current one isn't.
            if (!grid[nodeGridCoord.x, nodeGridCoord.y].walkable) {
                return lastWalkableNode;
            }
            // If the current node being checked is walkable, set it as the last walkable node and keep going.
            lastWalkableNode = grid[nodeGridCoord.x, nodeGridCoord.y];
        }
        return null;
    }
    // Draw stuff in the scene window.
    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1f));
        if (grid != null && displayGridGizmos) {
            foreach (Node n in grid) {

                Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, n.movementPenalty));

                Gizmos.color = (n.walkable)?Gizmos.color:Color.red;
                // // if (n.walkable && n.movementPenalty > 0) {
                // //     Gizmos.color = Color.blue;
                // // }
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter * gizmosCubeSizePercent));
            }
        }
    }
    [System.Serializable]
    public class TerrainType {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }
}

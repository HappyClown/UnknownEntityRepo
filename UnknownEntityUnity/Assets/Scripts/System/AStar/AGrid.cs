using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGrid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public Node[,] grid;
    public float nodeDiameter;
    public int gridSizeX, gridSizeY;


    void Start() {
        nodeDiameter = nodeRadius*2;
        // How many nodes fit in the grid on X and Y.
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }
    // Create a grid starting from the bottom left (0,0).
    void CreateGrid () {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = this.transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.up * gridWorldSize.y/2;

        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {
                // World coordinates of the node's center point.
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                // Check if the node is "walkable" by checking if it overlaps with a 2DCollider on the Unwalkable layer.
                bool walkable = !(Physics2D.OverlapBox(worldPoint, new Vector2(nodeRadius, nodeRadius), unwalkableMask));
                // Fill the 2D grid array with the node's x and y grid coordinates.
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }
    // Get a List of a node's neighbouring tiles.
    public List<Node> GetNeighbours(Node node) {
        List<Node> neighbours = new List<Node>();
        // Starting from the node's bottom left neighbour (-1,-1), the node being (0,0), to its top right neighbour (1,1).
        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (x == 0 && y == 0) {
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

    public List<Node> path;
    // Draw stuff in the scene window.
    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1f));
        if (grid != null) {
            foreach (Node n in grid) {
                Gizmos.color = (n.walkable)?Color.white:Color.red;
                if (path != null) {
                    if (path.Contains(n)) {
                        Gizmos.color = Color.black;
                    }
                }
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter/2f));
            }
        }
    }
}

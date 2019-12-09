using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;
    AGrid aGrid;
    void Awake() {
        if (aGrid == null) {
            aGrid = this.GetComponent<AGrid>();
        }
    }
    void Update() {
        FindPath(seeker.position, target.position);
    } 
    // Get the shortest* path from a start position to a target position.
    void FindPath(Vector3 startPos, Vector3 targetPos) {
        // Get the node on which the start and target world positions are.
        Node startNode = aGrid.NodeFromWorldPoint(startPos);
        Node targetNode = aGrid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0) {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++) {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost) {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode) {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach( Node neighbour in aGrid.GetNeighbours(currentNode)) {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour)) {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        aGrid.path = path;
    }
    // Get a value for the distance on the grid from A to B, diagonals being worth 14 and horizontals/verticals being worth 10.
    int GetDistance(Node nodeA, Node nodeB) {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        // The smallest distance doing, minus smallest distance to both axis, because they are diagonals.
        // The result of the highest minus the smallest doing, minus the result on the longest axis.
        // This gives you the shortest unobstructed path to the target on a grid allowing diagonal movements.
        if (dstX > dstY) {
            return 14 * dstY + 10 * (dstX-dstY); 
        }
        else {
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }
}

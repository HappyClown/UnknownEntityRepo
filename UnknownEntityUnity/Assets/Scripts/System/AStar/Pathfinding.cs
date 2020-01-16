﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour
{
    PathRequestManager requestManager;
    AGrid aGrid;

    void Awake() {
        if (requestManager == null) {
            requestManager = GetComponent<PathRequestManager>();
        }
        if (aGrid == null) {
            aGrid = this.GetComponent<AGrid>();
        }
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    // Get the shortest* path from a start position to a target position.
    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        // Get the node on which the start and target world positions are.
        Node startNode = aGrid.NodeFromWorldPoint(startPos);
        Node targetNode = aGrid.NodeFromWorldPoint(targetPos);

        if (startNode.walkable && targetNode.walkable)  {
            Heap<Node> openSet = new Heap<Node>(aGrid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0) {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode) {
                    //sw.Stop();
                    //print ("Path Found: " + sw.ElapsedMilliseconds + "ms");
                    pathSuccess = true;
                    break;
                }

                foreach( Node neighbour in aGrid.GetNeighbours(currentNode)) {
                    if (!neighbour.walkable || closedSet.Contains(neighbour)) {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour)) {
                            openSet.Add(neighbour);
                        }
                        else {
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess) {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    Vector3[] RetracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path) {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++) {
            Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY);
            if (directionNew != directionOld) {
                waypoints.Add(path[i].worldPos);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
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
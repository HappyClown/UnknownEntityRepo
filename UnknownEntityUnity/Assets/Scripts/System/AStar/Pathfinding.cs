using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour
{
    PathRequestManager requestManager;
    AGrid aGrid;
    List<Node> path = new List<Node>();
    List<Vector3> losWaypoints = new List<Vector3>();
    List<Vector3> simplifiedWaypoints = new List<Vector3>();
    List<Vector3> posSampled = new List<Vector3>();
    bool pathHasPenalty;
    public bool drawPathFindingGizmos;
    public LayerMask losLayerMask;

    void Awake() {
        if (requestManager == null) {
            requestManager = GetComponent<PathRequestManager>();
        }
        if (aGrid == null) {
            aGrid = this.GetComponent<AGrid>();
        }
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos, float unitIntel) {
        StartCoroutine(FindPath(startPos, targetPos, unitIntel));
    }

    // Get the shortest* path from a start position to a target position.
    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos, float unitIntel) {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;
        // Get the node on which the start and target world positions are.
        Node startNode = aGrid.NodeFromWorldPoint(startPos);
        Node targetNode = aGrid.NodeFromWorldPoint(targetPos);
        // print("Is my start node walkable? " + startNode.walkable);

        if (/* startNode.walkable &&  */targetNode.walkable)  {
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
                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + Mathf.RoundToInt(neighbour.movementPenalty * unitIntel);
                    //print(Mathf.RoundToInt(neighbour.movementPenalty * unitIntel));
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
        //print("FindPath; pathSuccess: "+pathSuccess);
    }

    Vector3[] RetracePath(Node startNode, Node endNode) {
        //List<Node> path = new List<Node>();
        path.Clear();
        pathHasPenalty = false;
        int totalPathCost = 0;
        //int nodeAmnt = 0;
        //float costPerNode = 0;

        Node currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);

            //nodeAmnt++;
            totalPathCost += (10 + currentNode.movementPenalty);
            if (currentNode.movementPenalty != 0) {
                pathHasPenalty = true;
            }

            currentNode = currentNode.parent;
        }

        //print("Total path cost: " + totalPathCost);
        //print("Total Amount of nodes: " + nodeAmnt);
        //costPerNode = totalPathCost / nodeAmnt;
        //print("Cost per node: " + costPerNode);

        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        waypoints = LineOfSightPath(waypoints, totalPathCost);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path) {
        //List<Vector3> waypoints = new List<Vector3>();
        simplifiedWaypoints.Clear();
        Vector2 directionOld = Vector2.zero;
        for (int i = 1; i < path.Count; i++) {
            Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY);
            if (directionNew != directionOld) {
                simplifiedWaypoints.Add(path[i-1].worldPos);
            }
            directionOld = directionNew;
        }
        if (path.Count-1 < path.Count && path.Count-1 > -1) {
            simplifiedWaypoints.Add(path[path.Count-1].worldPos);
        }
        return simplifiedWaypoints.ToArray();
    }

    Vector3[] LineOfSightPath(Vector3[] waypoints, float totPathCost) {
        //List<Vector3> losWaypoints = new List<Vector3>();
        losWaypoints.Clear();

        Vector3 fromWaypoint = Vector3.zero;
        Vector3 targWaypoint = Vector3.zero;
        for (int i = 0; i < waypoints.Length-1; i++)
        {
            fromWaypoint = waypoints[i];
            for (int ii = i+1; ii < waypoints.Length; ii++)
            {
                targWaypoint = waypoints[ii];
                bool hit = false;
                Vector3 dir = targWaypoint - fromWaypoint;
                float dist = dir.magnitude;
                if (Physics2D.Raycast(fromWaypoint, dir, dist, losLayerMask)) {
                    hit = true;
                }
                if (hit) {
                    if (drawPathFindingGizmos) {
                        UnityEngine.Debug.DrawRay(fromWaypoint, dir, Color.red, 1);
                    }
                    // If I hit an obstacle while checking the target waypoint, add the previous target waypoint to my list.
                    losWaypoints.Add(waypoints[ii-1]);
                    if (ii == waypoints.Length-1) {
                        losWaypoints.Add(waypoints[ii]);
                        i = ii;
                        break;
                    }
                    else if (ii > i+1) {
                        i = ii-2;
                        break;
                    }
                }
                if (!hit) {
                    if (drawPathFindingGizmos) {
                        UnityEngine.Debug.DrawRay(fromWaypoint, dir, Color.white, 1);
                    }
                    // If I dont hit anything:
                    // If checking from my first waypoint, add my first waypoint to my list. 
                    if (i == 0 && losWaypoints.Count == 0) {
                        losWaypoints.Add(waypoints[i]);
                    }
                    // If the waypoint
                    if (ii == waypoints.Length-1) {
                        losWaypoints.Add(waypoints[ii]);
                        i = ii;
                        break;
                    }
                }
            }

        }
        // If any of the nodes had a movement penalty, calculate the movement cost of the LoS Path versus following the Simplified Path.
        if (pathHasPenalty) {
            if (totPathCost < LoSPathCost(losWaypoints)) {
                return waypoints;
            }
            else {
                return losWaypoints.ToArray();
            }
        }
        else {
            return losWaypoints.ToArray();
        }
    }

    // Get samples of the node penalty costs every x along a distance and direction. To estimate the cost of a straight line that does not go through node centers. 
    float LoSPathCost(List<Vector3> waypointsToSample) {
            float sampleSpacing = aGrid.nodeRadius * 2;
            float totalCost = 0f;
            float totalDist = 0f;
            int totalSampleAmnt = 0;
            //posSampled.Clear();
        for (int i = 0; i < waypointsToSample.Count-1; i++)
        {
            Vector3 fromPos = waypointsToSample[i];
            Vector3 toPos = waypointsToSample[i+1];
            UnityEngine.Debug.DrawLine(fromPos, toPos, Color.green, 1);
            Vector3 dir = toPos - fromPos;
            float dist = dir.magnitude;
            totalDist += dist;
            int sampleAmnt = Mathf.RoundToInt(dist/sampleSpacing);
            for (int ii = 1; ii <= sampleAmnt; ii++)
            {
                Vector3 samplePos = fromPos + (dir/sampleAmnt)*ii;
                Node sampleNode = aGrid.NodeFromWorldPoint(samplePos);
                //posSampled.Add(samplePos);
                totalCost += 10 + sampleNode.movementPenalty;
                totalSampleAmnt++;
            }
        }
        //print("LOS Sampling Total Dist: " + totalDist);
        //print("LOS Sampling Total Sample Amount: " + totalSampleAmnt);
        //print("LOS Sampling Total Cost: " + totalCost);
        return totalCost;
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
    #region Drawing Gizmos
    public void DrawWithGizmosPath(List<Node> pathNodes)
    {
        Gizmos.color = Color.yellow;
        if (pathNodes.Count > 0) {
            foreach(Node pathNode in pathNodes) {
                Gizmos.DrawCube(pathNode.worldPos, Vector3.one * (aGrid.nodeRadius*2));
            }
        }
    }
    public void DrawWithGizmosSimpleWaypoints(List<Vector3> waypoints)
    {
        Gizmos.color = Color.green;
        if (waypoints.Count > 0) {
            foreach(Vector3 waypoint in waypoints) {
                Gizmos.DrawCube(waypoint, Vector3.one * (aGrid.nodeRadius*1.8f));
            }
        }
    }
    public void DrawWithGizmosLoSWaypoints(List<Vector3> losWaypoints)
    {
        Gizmos.color = Color.red;
        if (losWaypoints.Count > 0) {
            foreach(Vector3 losWaypoint in losWaypoints) {
                Gizmos.DrawCube(losWaypoint, Vector3.one * (aGrid.nodeRadius*1.6f));
            }
        }
    }
    public void DrawWithGizmosSamplePositions(List<Vector3> posSamplings) {
        Gizmos.color = Color.cyan;
        foreach(Vector3 pos in posSamplings) 
        {
            Gizmos.DrawCube(pos, Vector3.one * 0.4f);
        }
    }
    
    void OnDrawGizmos()
    {
        if (drawPathFindingGizmos) {
            DrawWithGizmosPath(path);
            DrawWithGizmosSimpleWaypoints(simplifiedWaypoints);
            DrawWithGizmosLoSWaypoints(losWaypoints);
            DrawWithGizmosSamplePositions(posSampled);
        }
    }
    #endregion
}

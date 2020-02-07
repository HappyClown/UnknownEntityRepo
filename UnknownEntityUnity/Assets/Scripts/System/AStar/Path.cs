using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path 
{
    public readonly Vector3[] lookPoints;
    public readonly Line[] turnBoundaries;
    public readonly int finishLineIndex;
    public readonly int slowDownIndex;
    private Vector3 pathStartPos = Vector3.zero;

    public Path(Vector3[] waypoints, Vector3 startPos, float turnDst, float stoppingDst) {
        pathStartPos = startPos;
        lookPoints = waypoints;
        turnBoundaries = new Line[lookPoints.Length];
        finishLineIndex = turnBoundaries.Length - 1;

        Vector2 previousPoint = V3ToV2(startPos);
        for (int i = 0; i < lookPoints.Length; i++) {
            Vector2 currentPoint = V3ToV2(lookPoints[i]);
            Vector2 dirToCurrentPoint = (currentPoint - previousPoint).normalized;
            Vector2 turnBoundaryPoint = (i == finishLineIndex)? currentPoint: currentPoint - dirToCurrentPoint * turnDst;
            turnBoundaries[i] = new Line(turnBoundaryPoint, previousPoint - dirToCurrentPoint * turnDst);
            previousPoint = turnBoundaryPoint;
        }
        // Find the first waypoint that is closer to the target then the stopping distance.
        // Slowing down can only occur after that point has been passed.
        float dstFromEndPoint = 0;
        for (int i = lookPoints.Length - 1; i > 0; i--) {
            dstFromEndPoint += Vector3.Distance (lookPoints[i], lookPoints[i - 1]);
            if (dstFromEndPoint > stoppingDst) {
                slowDownIndex = i;
                break;
            }
        }
    }

    Vector2 V3ToV2(Vector3 v3) {
        return new Vector2 (v3.x, v3.y);
    }

    public void DrawWithGizmos() {
        Gizmos.color = Color.black;
        if (lookPoints.Length > 0) {
          Gizmos.DrawLine(pathStartPos, lookPoints[0]);
          Gizmos.DrawCube(pathStartPos, Vector3.one * 0.1f);
        }
        for (int i = 0; i < lookPoints.Length - 1; i++) {
            Gizmos.DrawCube(lookPoints[i] + Vector3.back, Vector3.one * 0.2f);
            Gizmos.DrawLine(lookPoints[i], lookPoints[i+1]);
        }

        Gizmos.color = Color.white;
        foreach(Line l in turnBoundaries) {
            l.DrawWithGizmos(0.75f);
        }
    }
}

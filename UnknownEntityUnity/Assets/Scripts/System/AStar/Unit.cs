using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform target;
    public float speed = 1f;
    public float turnDst = 0.5f;

    Path path;
    // Vector3[] path;
    // int targetIndex;

    void Start() {
        PathRequestManager.RequestPath(this.transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful) {
        if (pathSuccessful) {
            path = new Path(waypoints, transform.position, turnDst);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath() {
        //Vector3 currentWaypoint = path[0];
        // Adjusting the waypoint's Z to the Unit's Z "height".
        //Vector3 currentWaypointAdjusted = new Vector3(currentWaypoint.x, currentWaypoint.y, this.transform.position.z);
        while (true) {
            // if (this.transform.position == currentWaypointAdjusted) {
            //     targetIndex++;
            //     if (targetIndex >= path.Length) {
            //         yield break;
            //     }
            //     currentWaypoint = path[targetIndex];
            // }
            // currentWaypointAdjusted = new Vector3(currentWaypoint.x, currentWaypoint.y, this.transform.position.z);
            // this.transform.position = Vector3.MoveTowards(this.transform.position, currentWaypointAdjusted, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos() {
        if (path != null) {
            // for (int i = targetIndex; i < path.Length; i++) {
            //     Gizmos.color = Color.black;
            //     Gizmos.DrawCube(path[i], Vector3.one / 5f);

            //     if (i == targetIndex) {
            //         Gizmos.DrawLine(this.transform.position, path[i]);
            //     }
            //     else {
            //         Gizmos.DrawLine(path[i-1], path[i]);
            //     }
            // }
            path.DrawWithGizmos();
        }
    }
}

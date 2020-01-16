using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    const float firstPathUpdateOnLevelLoad = 0.5f;
    const float minPathUpdateTime = 0.2f;
    const float pathUpdateMoveThreshold = 0.5f;
    public Transform target;
    public float speed = 1f;
    public float turnSpeed = 3f;
    public float turnDst = 0.5f;
    public float stoppingDst = 10f;
    public bool slowDown = false;
    public Transform mySprite;

    Path path;

    void Start() {
        StartCoroutine(UpdatePath());
    }

    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful) {
        if (pathSuccessful) {
            path = new Path(waypoints, transform.position, turnDst, stoppingDst);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator UpdatePath() {
        // Check if the level has been loaded for a certain amount of time before asking for the first path update.
        if (Time.timeSinceLevelLoad < firstPathUpdateOnLevelLoad) {
            yield return new WaitForSeconds (firstPathUpdateOnLevelLoad);
        }
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        // Get the squared move threshold value(comparing squared magnitudes is faster then checking distance).
        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;

        while (true) {
            // Check if the path needs to be updated every X seconds.
            yield return new WaitForSeconds (minPathUpdateTime);
            if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold) {
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                targetPosOld = target.position;
            }
        }
    }

    IEnumerator FollowPath() {
        bool followingPath = true;
        int pathIndex = 0;
        if (path.lookPoints.Length > 0) {
          transform.LookAt(path.lookPoints[0]);
        }

        float speedPercent = 1;

        while (followingPath) {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.y);
            while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D)) {
                if (pathIndex >= path.finishLineIndex) {
                    followingPath = false;
                    pathIndex = 0;
                    break;
                } else {
                    pathIndex++;
                    if (pathIndex >= path.turnBoundaries.Length) {
                        Debug.Log("Too big, dosnt fit.");
                    }
                }
            }

            if (followingPath) {
                if (pathIndex >= path.slowDownIndex && slowDown && stoppingDst > 0) {
                    speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDst);
                    if (speedPercent < 0.01f) {
                        followingPath = false;
                    }
                }
                // Turn to face the next waypoint in the path.
                // The turn sharpness or size is determined by the speed at which the unit rotates to look at the next point.
                Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
                mySprite.position = new Vector3(this.transform.position.x, this.transform.position.y, mySprite.position.z);
            }

            yield return null;
        }
    }

    public void OnDrawGizmos() {
        if (path != null) {

            path.DrawWithGizmos(transform.position);
        }
    }
}

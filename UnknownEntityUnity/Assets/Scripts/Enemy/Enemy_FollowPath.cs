using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_FollowPath : MonoBehaviour
{
    const float firstPathUpdateOnLevelLoad = 0.5f;
    const float minPathUpdateTime = 0.2f;
    const float pathUpdateMoveThreshold = 0.5f;
    public Transform target;
    public Transform thisEnemyTrans;
    public float stopRangeToTarget;
    public bool directlyMovingtoTarget;
    Vector3 directTargetPos;
    public SO_EnemyBase enemy;
    public FlipObjectX flip;
    public Enemy_Refs eRefs;
    public bool allowPathUpdate = true, customPathRequested = false;
    public bool followOnStart;
    public bool followingPath;
    public float speedModifier = 1f;
    public bool drawPathGizmos;
    Path path;
    // Direct move stuff.
    // Check line of sight between this enemy and the target wit ha circle cast the size of this enemy's collision.

    void Start() {
        if (followOnStart) {
            StartCoroutine(UpdatePath());
        }
    }

    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful) {
        if (pathSuccessful && (allowPathUpdate || customPathRequested)) {
            path = new Path(waypoints, transform.position, enemy.turnDst, enemy.slowDownDist);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }
    // When to request a new path.
    public IEnumerator UpdatePath() {
        // Check if the level has been loaded for a certain amount of time before asking for the first path update.
        if (Time.timeSinceLevelLoad < firstPathUpdateOnLevelLoad) {
            yield return new WaitForSeconds (firstPathUpdateOnLevelLoad);
        }
        // Request a path.
        PathRequestManager.RequestPath(transform.position, target.position, enemy.intelligence, OnPathFound);
        // Get the squared move threshold value(comparing squared magnitudes is faster then checking distance).
        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;
        // Start path update request loop. Insert different reasons to update path to target.
        while (true) {
            // Check if the path needs to be updated every X seconds based on how far the target has moved.
            yield return new WaitForSeconds (minPathUpdateTime);
            if (allowPathUpdate) {
                // Cancel custom path request authorization.
                customPathRequested = false;
                // Check if the Player has moved enough to trigger a new path request.
                if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold) {
                    // Check to see if a direct line of sight exists to the target. (Ignores Movement slowdown fields)
                    if (!CheckDirectMoveToTarget()) {
                        print ("Direct line of sight available, do not request path, walk directly towards target.");
                        StopCoroutine(FollowPath());
                        followingPath = false;
                        StopCoroutine(DirectMoveToTarget());
                        directlyMovingtoTarget = false;
                        yield return null;
                        StartCoroutine(DirectMoveToTarget());
                        targetPosOld = target.position;
                        continue;
                    }
                    StopCoroutine("DirectMoveToTarget");
                    directlyMovingtoTarget = false;
                    // Request a path.
                    PathRequestManager.RequestPath(transform.position, target.position, enemy.intelligence, OnPathFound);
                    // Store the target's position when the path was requested.
                    targetPosOld = target.position;
                }
            }
        }
    }

    bool CheckDirectMoveToTarget() {
        if (eRefs.CircleCastLOSToTarget(thisEnemyTrans.position, target.position, 0.25f).collider != null) {
            print ("Something was hit by the circle cast, returning: true");
            print(eRefs.CircleCastLOSToTarget(thisEnemyTrans.position, target.position, 0.25f).collider.gameObject.name);
            return true;
        }
        else {
            return false;
        }
    }
    IEnumerator DirectMoveToTarget() {
        directlyMovingtoTarget = true;
        transform.forward = target.position - thisEnemyTrans.position;
        directTargetPos = target.position;
        while (directlyMovingtoTarget && eRefs.DistToTarget(thisEnemyTrans.position, directTargetPos) > stopRangeToTarget) {
            print("Ping, direct move to target is running.");
            transform.Translate(Vector3.forward * Time.deltaTime * enemy.moveSpeed * speedModifier, Space.Self);
            flip.Flip();
            yield return null;
        }
        directlyMovingtoTarget = false;
    }

    public void RequestPathToTarget(Vector3 _pathTarget) {
        customPathRequested = true;
        Vector3 xyTarget = new Vector3(_pathTarget.x, _pathTarget.y, target.position.z);
        PathRequestManager.RequestPath(transform.position, xyTarget, enemy.intelligence, OnPathFound);
    }
    
    IEnumerator FollowPath() {
        followingPath = true;
        int pathIndex = 0;
        if (path.lookPoints.Length > 0) {
          transform.LookAt(path.lookPoints[0]);
        }
        float speedPercent = 1;
        // if (path.turnBoundaries.Length < pathIndex) {
        //     followingPath = false;
        // }
        while (followingPath) {
            // Check to see if the unit has reached its destination.
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.y);
            while (path.turnBoundaries.Length > -1 && pathIndex <= path.turnBoundaries.Length-1 && path.turnBoundaries[pathIndex].HasCrossedLine(pos2D)) {
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
            if (pathIndex > path.lookPoints.Length-1) {
                followingPath = false;
            }
            if (followingPath) {
                // Slow down when the enemy gets close to its target.
                // if (pathIndex >= path.slowDownIndex && enemy.slowDown && enemy.slowDownDist > 0) {
                //     speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / enemy.slowDownDist);
                //     if (speedPercent < 0.01f) {
                //         followingPath = false;
                //     }
                // }
                transform.forward = path.lookPoints[pathIndex] - transform.position;
                // Movement.
                transform.Translate(Vector3.forward * Time.deltaTime * enemy.moveSpeed * speedModifier * speedPercent, Space.Self);
                flip.Flip();
            }
            yield return null;
        }
    }

    public void StopAllMovementCoroutines() {
        StopCoroutine("FollowPath");
        followingPath = false;
        directlyMovingtoTarget = false;
        allowPathUpdate = false;
    }

    public void OnDrawGizmos() {
        if (drawPathGizmos && path != null) {
            path.DrawWithGizmos();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Enemy_Movement_RunAway : MonoBehaviour
{
    [Header ("Script References")]
    public Enemy_Refs eRefs;

    [Header ("To-set Variables")]
    public float circleCastRadius;
    public float aGridNodeDiam = 0.5f;
    public int maxLoops;
    public Transform runAwayToken;
    public bool runAwayDebugs;
    public float distToUpdatePath;

    [Header ("Read Only")]
    int loopCount = 0;
    Vector2 cWisePerpenDir, counterCWisePerpenDir, cWiseTestPos, counterCWiseTestPos, surfaceNormal;


    public void SetupRunAwayPos() {
        Stopwatch sw = new Stopwatch();
        if (runAwayDebugs) sw.Start();
        Vector2 targetPos = Vector2.zero;
        Vector2 prevDir = Vector2.zero;
        Vector2 plyrPos = eRefs.PlayerPos;
        Vector2 oppositeDirNorm = (plyrPos - (Vector2)this.transform.position).normalized * -1;
        RaycastHit2D hit, lastHit;

        float distLeft = eRefs.eSO.atkRange;
        hit = Physics2D.Raycast(this.transform.position, oppositeDirNorm, distLeft, eRefs.losLayerMask);

        if (hit) {
            lastHit = hit;
            targetPos = hit.point - (hit.normal*-1)*(circleCastRadius+0.1f);
            distLeft -= (hit.distance-circleCastRadius);
            surfaceNormal = hit.normal;
            loopCount = 0;
            while (loopCount < maxLoops) {
                loopCount++;
                RaycastHit2D loopHit = new RaycastHit2D();
                // Get the perpendicular direction the furtest away from the player that is not inside an obstacle and apply the remaining direction that way until it has been fully used.
                cWisePerpenDir = new Vector2(surfaceNormal.y, -surfaceNormal.x);
                counterCWisePerpenDir = new Vector2(-surfaceNormal.y, surfaceNormal.x);
                cWiseTestPos = targetPos + cWisePerpenDir;
                counterCWiseTestPos = targetPos + counterCWisePerpenDir;
                // Try clockwise.
                if (eRefs.DistToTarget(cWiseTestPos, eRefs.PlayerPos) > eRefs.DistToTarget(counterCWiseTestPos, eRefs.PlayerPos) && !Physics2D.Raycast(targetPos, cWisePerpenDir, aGridNodeDiam, eRefs.losLayerMask)) {
                    if (runAwayDebugs) { UnityEngine.Debug.DrawRay(targetPos, cWisePerpenDir*aGridNodeDiam, Color.cyan, 5f); print("Chose to shoot clockwise."); }
                    loopHit = Physics2D.Raycast(targetPos, cWisePerpenDir, distLeft, eRefs.losLayerMask);
                    prevDir = cWisePerpenDir;
                }
                // Try counter-clockwise.
                else if (!Physics2D.Raycast(targetPos, counterCWisePerpenDir, aGridNodeDiam, eRefs.losLayerMask)) {
                    if (runAwayDebugs) { UnityEngine.Debug.DrawRay(targetPos, counterCWisePerpenDir*aGridNodeDiam, Color.cyan, 5f); print("Chose to shoot counter-clockwise."); }
                    loopHit = Physics2D.Raycast(targetPos, counterCWisePerpenDir, distLeft, eRefs.losLayerMask);
                    prevDir = counterCWisePerpenDir;
                }
                else {
                    break;
                }
                if (loopHit) {
                    targetPos = loopHit.point - (loopHit.normal*-1)*(circleCastRadius+0.1f);
                    distLeft -= (loopHit.distance-circleCastRadius);
                    surfaceNormal = loopHit.normal;
                }
                // If nothing was hit, set the position to move to, to the last direction plus the distance left.
                else {
                    targetPos = targetPos + (prevDir*distLeft);
                    break;
                }
            }
        }
        else {
            targetPos = (Vector2)this.transform.position + oppositeDirNorm*distLeft;
        }
        runAwayToken.position = targetPos;
        eRefs.eFollowPath.target = runAwayToken;
        if (runAwayDebugs) sw.Stop(); print("The run away target postion took: "+sw.ElapsedMilliseconds+"ms to determine.");
    }

    public bool UpdateTargetPosCondition() {
        // If I reach my run away position.
        //if (eRefs.SqrDistToTarget(runAwayToken.position, this.transform.position) < )
        return false;
    }
}

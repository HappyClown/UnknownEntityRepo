using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class RangedSkeleton_Actions : Enemy_Actions
{
    public RangedSkeleton_ThrowProjectile throwProj;
    public Enemy_Refs eRefs;
    public LayerMask runlayers;
    public bool runAwayDebugs;
    public Transform circleCastVisualizer;
    public Transform[] circleCastVisualizers;
    public float circleCastRadius;
    public float aGridNodeDiam = 0.5f;
    int loopCount = 0;
    public int maxLoops;
    Vector2 cWisePerpenDir, counterCWisePerpenDir, cWiseTestPos, counterCWiseTestPos, surfaceNormal;

    public override void StartChecks() {
        throwProj.StartCheck();
    }

    public override void StopActions() {
        throwProj.StopAllCoroutines();
        throwProj.enabled = false;
    }

    public void RunAway() {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        Vector2 targetPos = Vector2.zero;
        Vector2 prevDir = Vector2.zero;
        Vector2 plyrPos = eRefs.plyrTrans.position;
        Vector2 oppositeDirNorm = (plyrPos - (Vector2)this.transform.position).normalized * -1;
        RaycastHit2D hit, lastHit;
        // Change the circle cast radius to be set in the inspector (it should ?always? match or be slightly bigger then node radius.)
        // Try making the distance as much as the difference between the unit's max attack range and distance from unit to player, for the unit to maximally walk away up to its attack range. 
        // OR just set it to the units attack range. 
        // OR leave it a bit farther to make sure the unit always keeps walking. OR calculate it based on the unit's movement speed and run away duration (attack cooldown) to be certain that it always walks during its cooldown and that it dosnt have to calculate a really far away path for nothing (probably not this since the path isnt walked along the lines this will cast).
        
        float distLeft = eRefs.eSO.atkRange;
        hit = Physics2D.Raycast(this.transform.position, oppositeDirNorm, distLeft, runlayers);

        if (hit) {
            lastHit = hit;
            targetPos = hit.point - (hit.normal*-1)*(circleCastRadius+0.1f);
            distLeft -= (hit.distance-circleCastRadius);
            surfaceNormal = hit.normal;
            if (runAwayDebugs) circleCastVisualizers[0].position = new Vector3(targetPos.x, targetPos.y, circleCastVisualizer.position.z);
            loopCount = 0;
            while (loopCount < maxLoops) {
                loopCount++;
                if (runAwayDebugs) circleCastVisualizers[loopCount].position = new Vector3(targetPos.x, targetPos.y, circleCastVisualizer.position.z);
                RaycastHit2D loopHit = new RaycastHit2D();
                // Get the perpendicular direction the furtest away from the player that is not inside an obstacle and apply the remaining direction that way until it has been fully used.
                cWisePerpenDir = new Vector2(surfaceNormal.y, -surfaceNormal.x);
                counterCWisePerpenDir = new Vector2(-surfaceNormal.y, surfaceNormal.x);
                cWiseTestPos = targetPos + cWisePerpenDir;
                counterCWiseTestPos = targetPos + counterCWisePerpenDir;
                // Try clockwise.
                if (eRefs.DistToPlayer(cWiseTestPos) > eRefs.DistToPlayer(counterCWiseTestPos) && !Physics2D.Raycast(targetPos, cWisePerpenDir, aGridNodeDiam, runlayers)) {
                    if (runAwayDebugs) {
                        UnityEngine.Debug.DrawRay(targetPos, cWisePerpenDir*aGridNodeDiam, Color.cyan, 5f);
                        print("Chose to shoot clockwise.");
                    }
                    loopHit = Physics2D.Raycast(targetPos, cWisePerpenDir, distLeft, runlayers);
                    prevDir = cWisePerpenDir;
                }
                // Try counter-clockwise.
                else if (!Physics2D.Raycast(targetPos, counterCWisePerpenDir, aGridNodeDiam, runlayers)) {
                    if (runAwayDebugs) {
                        UnityEngine.Debug.DrawRay(targetPos, counterCWisePerpenDir*aGridNodeDiam, Color.cyan, 5f);
                        print("Chose to shoot counter-clockwise."); 
                    }
                    loopHit = Physics2D.Raycast(targetPos, counterCWisePerpenDir, distLeft, runlayers);
                    prevDir = counterCWisePerpenDir;
                }
                else {
                    break;
                }
                if (loopHit) {
                    if (runAwayDebugs) circleCastVisualizer.position = new Vector3(targetPos.x, targetPos.y, circleCastVisualizer.position.z);
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
        if (runAwayDebugs) circleCastVisualizers[loopCount].position = new Vector3(targetPos.x, targetPos.y, circleCastVisualizer.position.z);
        eRefs.unit.RequestPathToTarget(targetPos);
        sw.Stop();
        print("The run away target postiion took: "+sw.ElapsedMilliseconds+"ms to determine.");
    }
}

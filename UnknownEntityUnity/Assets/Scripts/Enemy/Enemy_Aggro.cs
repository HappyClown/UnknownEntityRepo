using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Aggro : MonoBehaviour
{
    public Enemy_Refs eRefs;
    public bool checkingAggro, aggroed;
    private bool coroutineRunning;
    private float targCheckRadiusSqr;
    public float checkAggroDelay;
    public LayerMask blockLOSLayers;
    public CircleCollider2D myCol;
    public bool aggroDebugs;

    // Triggered by the Player.
    public void EnableAggro(float targRadius) {
        if (!aggroed) {
            checkingAggro = true;
            // Add the circle collider's radius to adjust the enemy's "this.transform.position" to the edge of its circle collider.
            targCheckRadiusSqr = (targRadius+myCol.radius) * (targRadius+myCol.radius);
            if (!coroutineRunning) {
                StartCoroutine(CheckAggro());
            }
            //Debug.Log("Enemy: " + this.name + " is starting to check its aggro range and LoS!");
        }
    }
    // public void DisableAggro() {
    //     checkingAggro = false;
    // }
    IEnumerator CheckAggro() {
        coroutineRunning = true;
        yield return null;
        if (aggroDebugs) Debug.Log("Enemy: " + this.name + " CheckAggro coroutine has started.");
        float aggroRangeSqr = 0f;
        aggroRangeSqr = eRefs.eSO.aggroRange * eRefs.eSO.aggroRange;
        while(checkingAggro) {
            if (aggroDebugs) Debug.Log("Enemy: " + this.name + " just checked aggro.");
            float distToTargetSqr = eRefs.SqrDistToTarget(this.transform.position, eRefs.PlayerShadowPos);
            // Check if the Player is within aggro range.
            if (distToTargetSqr < aggroRangeSqr) {
                if (aggroDebugs)Debug.DrawRay(this.transform.position, eRefs.NormDirToTargetV2(this.transform.position, eRefs.PlayerShadowPos)*eRefs.DistToTarget(this.transform.position, eRefs.PlayerShadowPos), Color.green, 0.5f);
                // If I dont hit anything that blocks line of sight, aggro.
                if (!Physics2D.Raycast(this.transform.position, eRefs.NormDirToTargetV2(this.transform.position, eRefs.PlayerShadowPos), eRefs.DistToTarget(this.transform.position, eRefs.PlayerShadowPos), blockLOSLayers)) {
                    eRefs.eAction.StartChecks();
                    //Enemy has aggroed its target, request first path.
                    eRefs.eFollowPath.StartCoroutine(eRefs.eFollowPath.UpdatePath());
                    checkingAggro = false;
                    coroutineRunning = false;
                    aggroed = true;
                    break;
                }
            } 
            // Stop the aggro checking coroutine if the enemy is outside of the target's aggro check range. Only works if the enemy has not aggroed yet.
            else if (distToTargetSqr > targCheckRadiusSqr) {
                checkingAggro = false;
                coroutineRunning = false;
                break;
            }
            // Wait a certain amount of time before checking again. (for more precision, increase rate based on Player distance)
            yield return new WaitForSeconds(checkAggroDelay);
        }
        coroutineRunning = false;
        yield return null;
    }
}

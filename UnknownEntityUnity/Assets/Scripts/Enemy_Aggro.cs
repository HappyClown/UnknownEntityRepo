using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Aggro : MonoBehaviour
{
    public bool checkingAggro, aggroed;
    private bool coroutineRunning;
    public SO_EnemyBase enemy;
    public Unit unit;
    public Transform target;
    private float targCheckRadiusSqr;
    public float checkAggroDelay;
    public LayerMask blockLOSLayers;
    public CircleCollider2D myCol;

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
        Debug.Log("Enemy: " + this.name + " CheckAggro coroutine has started.");
        float aggroRangeSqr = 0f;
        aggroRangeSqr = enemy.aggroRange * enemy.aggroRange;
        while(checkingAggro) {
            Debug.Log("Enemy: " + this.name + " just checked aggro.");
            float distToTargetSqr = (target.position - this.transform.position).sqrMagnitude;
            if (distToTargetSqr < aggroRangeSqr) {
                Debug.DrawLine(this.transform.position, target.position, Color.green, 0.5f);
                if (!Physics2D.Raycast(this.transform.position, target.position - this.transform.position, enemy.aggroRange, blockLOSLayers)) {
                    //Enemy has aggroed its target, request first path.
                    unit.StartCoroutine(unit.UpdatePath());
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
            yield return new WaitForSeconds(checkAggroDelay);
        }
        coroutineRunning = false;
        yield return null;
    }
}

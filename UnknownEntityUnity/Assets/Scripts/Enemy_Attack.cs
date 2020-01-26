using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack : MonoBehaviour
{
    public Unit unitMovement;
    public SO_EnemyBase enemy;
    public Transform target;
    public LayerMask blockLOSLayers;
    public Sprite mySprite;
    private float sqrAtkRange;
    

    void Start()
    {
        sqrAtkRange = enemy.atkRange * enemy.atkRange;
    }

    void Update()
    {
        if (unitMovement.followingPath) {// and after the first waypoint beyond atk range dist following the path
            // Check if the target is within attack range.
            float distToTargetSqr = (target.position - this.transform.position).sqrMagnitude;
            if (distToTargetSqr <= sqrAtkRange) {
                // Check to see if there are obstacles in the way.
                if (!Physics2D.Raycast(this.transform.position, target.position - this.transform.position, enemy.atkRange, blockLOSLayers)) {
                    unitMovement.followingPath = false;
                    // Trigger Attack
                }
            }
        }
    }

    IEnumerator Attack() {
        float timer = 0f;
        int animIndex = 0;
        while (true) {
            timer += Time.deltaTime;
            if (timer > enemy.animTimings[animIndex]) {
                mySprite = enemy.animSprites[animIndex];
                animIndex++;
                if (animIndex > enemy.animTimings.Length-1) {
                    
                }
            }

            yield return null;
        }
        yield return null;
    }
}

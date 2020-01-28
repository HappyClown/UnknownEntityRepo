using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack : MonoBehaviour
{
    public Unit unitMovement;
    public SO_EnemyBase enemy;
    public Transform target;
    public LayerMask blockLOSLayers;
    public SpriteRenderer mySpriteR;
    public PolygonCollider2D myCol;
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
                    StartCoroutine(Attack());
                    // Trigger Attack
                }
            }
        }
    }

    IEnumerator Attack() {
        float timer = 0f;
        int animIndex = 0;
        unitMovement.allowPathUpdate = false;
        while (true) {
            timer += Time.deltaTime;
            if (timer > enemy.animTimings[animIndex]) {
                mySpriteR.sprite = enemy.animSprites[animIndex];
                // Activate the attack collider at this enemy's position towards the player.
                if (!myCol.enabled && animIndex == enemy.colTimingIndex) {
                    myCol.points = enemy.atkCol.points;
                    myCol.transform.position = this.transform.position;
                    myCol.transform.up = target.position - myCol.transform.position;
                    myCol.enabled = true;
                }
                animIndex++;
                if (animIndex > enemy.animTimings.Length-1) {
                    myCol.enabled = false;
                    unitMovement.allowPathUpdate = true;
                    break;
                }
            }
            yield return null;
        }
    }
}
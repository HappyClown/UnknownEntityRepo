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
    private bool detectCol;
    public ContactFilter2D playerFilter;
    private float damageRoll;
    private bool colliderOn;
    public bool attacking;
    public float cooldownTimer;
    
    float DamageRoll {
        get {
            return Random.Range(enemy.minDamage, enemy.maxDamage);
        }
    }

    void Update()
    {
        // There are four checks to determine if a basic attack can be initiated; Cooldown, Attacking already, Distance to target, Line of sight.
        // Check if my attack is on cooldown.
        if (cooldownTimer >= enemy.attackCooldown) {
            // Check if im not already attacking. Set back to true in the coroutine.
            if (!attacking) {
                // Check if the target is within attack range.
                float distToTargetSqr = (target.position - this.transform.position).sqrMagnitude;
                float sqrAtkRange = enemy.atkRange * enemy.atkRange;
                if (distToTargetSqr <= sqrAtkRange) {
                    // Check to see if there are obstacles in the way.
                    if (!Physics2D.Raycast(this.transform.position, target.position - this.transform.position, enemy.atkRange, blockLOSLayers)) {
                        // Trigger Attack coroutine.
                        StartCoroutine(Attack());
                    }
                }
            }
        }
        else {
            cooldownTimer += Time.deltaTime;
        }
    }

    IEnumerator Attack() {
        attacking = true;
        unitMovement.StopFollowPathCoroutine();
        float timer = 0f;
        int animIndex = 0;
        Vector2 attackDir = target.position - this.transform.position;
        Debug.DrawLine(target.position, this.transform.position, Color.magenta, enemy.animTimings[enemy.animSprites.Length-1]);
        while (attacking) {
            timer += Time.deltaTime;
            if (timer > enemy.animTimings[animIndex]) {
                mySpriteR.sprite = enemy.animSprites[animIndex];
                // Activate the attack collider at this enemy's position towards the player.
                if (!myCol.enabled && animIndex == enemy.colTimingIndex) {
                    myCol.points = enemy.atkCol.points;
                    myCol.transform.position = this.transform.position;
                    myCol.transform.up = attackDir;
                    myCol.enabled = true;
                    colliderOn = true;
                }
                if (colliderOn) {
                    Collider2D[] hitCols = new Collider2D[1];
                    Physics2D.OverlapCollider(myCol, playerFilter, hitCols);
                    if (hitCols[0] != null) {
                        hitCols[0].GetComponent<Character_Health>().currentHealth -= DamageRoll;
                        // Debug.Log("Player was damaged for: " + DamageRoll);
                        colliderOn = false;
                    }
                }
                animIndex++;
                if (animIndex > enemy.animTimings.Length-1) {
                    ExitAttackReset();
                }
            }
            yield return null;
        }
    }

    void ExitAttackReset() {
        myCol.enabled = false;
        colliderOn = false;
        unitMovement.allowPathUpdate = true;
        attacking = false;
        cooldownTimer = 0f;
    }
}
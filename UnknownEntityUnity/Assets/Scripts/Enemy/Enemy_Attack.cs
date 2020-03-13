using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy_Attack : MonoBehaviour
{
    public Enemy_Refs eRefs;
    public Transform atkOrigin;
    public LayerMask blockLOSLayers;
    public PolygonCollider2D myCol;
    private bool detectCol;
    public ContactFilter2D playerFilter;
    private float damageRoll;
    private bool colliderOn;
    public bool attacking;
    public float cooldownTimer;
    public Vector2 attackDir;
    public UnityEvent unityEvent;
    
    float DamageRoll {
        get {
            return Random.Range(eRefs.eSO.minDamage, eRefs.eSO.maxDamage);
        }
    }

    // Turn this into a coroutine triggered by the aggro script?
    void Update()
    {
        // There are four checks to determine if a basic attack can be initiated; Cooldown, Attacking already, Distance to target, Line of sight.
        // Check if my attack is on cooldown.
        if (cooldownTimer >= eRefs.eSO.attackCooldown) {
            // Check if im not already attacking. Set back to true in the coroutine.
            if (!attacking) {
                // Check if the target is within attack range.
                float distToTargetSqr = (eRefs.plyrTrans.position - this.transform.position).sqrMagnitude;
                float sqrAtkRange = eRefs.eSO.atkRange * eRefs.eSO.atkRange;
                if (distToTargetSqr <= sqrAtkRange) {
                    // Check to see if there are obstacles in the way.
                    if (!Physics2D.Raycast(this.transform.position, eRefs.plyrTrans.position - this.transform.position, eRefs.eSO.atkRange, blockLOSLayers)) {
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
        // Stop following path to Player which also stops all movement.
        eRefs.unit.StopFollowPathCoroutine();
        float timer = 0f;
        int animIndex = 0;
        // Get the attack direction as soon as the attack is triggered.
        attackDir = eRefs.NormDirToPlayerV2(atkOrigin.position);
        //Debug.DrawLine(target.position, this.transform.position, Color.magenta, enemy.animTimings[enemy.animSprites.Length-1]);
        // Timer based attack loop.
        while (attacking) {
            timer += Time.deltaTime;
            if (timer > eRefs.eSO.animEvents[animIndex]) {
                // Change the enemy sprite to match the current animation, based on the animation timings.
                eRefs.eSpriteR.sprite = eRefs.eSO.animSprites[animIndex];
                // Activate the attack collider at this enemy's position towards the player.
                if (!colliderOn && animIndex == eRefs.eSO.colTimingIndex) {
                    unityEvent.Invoke();
                //     myCol.points = enemy.atkCol.points;
                //     myCol.transform.position = this.transform.position;
                //     myCol.transform.up = attackDir;
                //     myCol.enabled = true;
                    colliderOn = true;
                }
                // Check if the player is within the attack collider, apply damage.
                // if (colliderOn) {
                //     Collider2D[] hitCols = new Collider2D[1];
                //     //List<Collider2D> hitColss = new List<Collider2D>();
                //     Physics2D.OverlapCollider(myCol, playerFilter, hitCols);
                //     if (hitCols[0] != null) {
                //         hitCols[0].GetComponent<Character_Health>().currentHealth -= DamageRoll;
                //         // Debug.Log("Player was damaged for: " + DamageRoll);
                //         colliderOn = false;
                //     }
                // }
                animIndex++;
                if (animIndex > eRefs.eSO.animEvents.Length-1) {
                    ExitAttackReset();
                }
            }
            yield return null;
        }
    }

    void ExitAttackReset() {
        //myCol.enabled = false;
        colliderOn = false;
        eRefs.unit.allowPathUpdate = true;
        attacking = false;
        cooldownTimer = 0f;
    }
}
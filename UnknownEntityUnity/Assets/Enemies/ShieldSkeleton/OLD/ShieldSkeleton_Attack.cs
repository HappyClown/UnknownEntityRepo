using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSkeleton_Attack : Enemy_Attack
{
    ShieldSkeleton shieldBash;
    ShieldSkeleton shieldUp;
    IEnumerator AttackCheck() {
        // There are four checks to determine if a basic attack can be initiated; Cooldown, Attacking already, Distance to target, Line of sight.
        // Check if my attack is on cooldown.
        if (cooldownTimer >= eRefs.eSO.attackCooldown) {
            // Check if im not already attacking. Set back to true in the coroutine.
            if (!attacking) {
                // Check if the target is within attack range.
                float distToTargetSqr = (eRefs.PlayerPos - this.transform.position).sqrMagnitude;
                float sqrAtkRange = eRefs.eSO.atkRange * eRefs.eSO.atkRange;
                if (distToTargetSqr <= sqrAtkRange) {
                    // Check to see if there are obstacles in the way.
                    if (!Physics2D.Raycast(this.transform.position, eRefs.PlayerPos - this.transform.position, eRefs.eSO.atkRange, blockLOSLayers)) {
                        // Trigger Attack coroutine.
                        StartCoroutine(WindUp());
                    }
                }
            }
        }
        else {
            cooldownTimer += Time.deltaTime;
        }
        yield return null;
    }

    IEnumerator WindUp() {
        // Wind up animation / sprites.

        yield return null;
    }
    IEnumerator Attack() {
        // Attack animation / sprites.

        yield return null;
    }
    IEnumerator WindDown() {
        // Wind down animation / sprites.

        yield return null;
    }
}

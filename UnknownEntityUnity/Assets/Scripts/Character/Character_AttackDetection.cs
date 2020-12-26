using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AttackDetection : MonoBehaviour
{
    public ContactFilter2D hitLayers;
    // public PolygonCollider2D[] attackColliders;
    // public bool[] activeAttacks;
    // public PolygonCollider2D attackCollider;
    // public List<Collider2D> collidersHit;
    // public List<Collider2D> collidersDamaged;

    public IEnumerator AttackCollider(SO_Weapon.AttackChain WeapAtkChain, SO_AttackFX sO_AttackFX, PolygonCollider2D atkFXCol) {
        // References from the SO_AttackFX.
        float thisColStart = sO_AttackFX.colStart;
        float thisColEnd = sO_AttackFX.colEnd;
        // Set chosen pool attack FX collider off in case the attack FX dosn't use a collider.
        atkFXCol.enabled = false;
        SO_ImpactFX sO_ImpactFX = sO_AttackFX.soImpactFX;
        // Apply the collision shape and offset position to the chosen pool collider.
        atkFXCol.points = sO_AttackFX.collider.points;
        atkFXCol.offset = sO_AttackFX.collider.offset;
        // Set other variables.
        List<Collider2D> collidersHit = new List<Collider2D>();
        List<Collider2D> collidersDamaged = new List<Collider2D>();
        float timer = 0f;
        bool detectCol = false;
        // Wait a frame to avoid having the collider appear for a frame at its last position, the atkFX's new position HAS to be set before the collider is enabled. (Maybe specifically for atkFX's that have the same colStart time, first sprite time and moveDelay all set to the same value)
        yield return null;
        
        while (timer < thisColEnd) {
            timer += Time.deltaTime;
            if (timer >= thisColStart && !detectCol) {
                yield return null;
                // If the attack has a collider, enable it otherwise leave it off.
                if (sO_AttackFX.collider) {
                    atkFXCol.enabled = true;
                }
                detectCol = true;
                yield return null;
            }
            if (detectCol) {
                // Detect hit collision.
                Physics2D.OverlapCollider(atkFXCol, hitLayers, collidersHit);
                foreach (Collider2D col in collidersHit)
                {
                    if (!collidersDamaged.Contains(col)) {
                        collidersDamaged.Add(col);
                        // Hit impact FX. Apply the correct rotation, position, sprites and layerMask to an impactFX.
                        HitImpact.PlayImpactFX(atkFXCol.transform.position, col.bounds.center, sO_ImpactFX, hitLayers.layerMask, col);
                        // Slow time. Duration to be set by weapon damage, slow to be adjusted (animation curve) by TimeSlow script.
                        TimeSlow.StartTimeSlow(6, 0f);
                        // If this is an enemy, apply damage.
                        if (col.gameObject.CompareTag("Enemy")) {
                            col.GetComponent<Enemy_Health>().ReceiveDamage(WeapAtkChain.DamageRoll);
                        }
                        else if (col.gameObject.CompareTag("Destructible")) {
                            col.GetComponent<Clutter_Health>().ReceiveDamage(WeapAtkChain.DamageRoll);
                        }
                        //Debug.Log("Collider: " + col.gameObject.name + " was hit! Hit hit, hurraay!");
                    }
                }
            }
            yield return null;
        }
        if (collidersHit.Count > 0) {
            collidersHit.Clear();
            collidersDamaged.Clear();
        }
        atkFXCol.enabled = false;
        yield return null;
    }
}

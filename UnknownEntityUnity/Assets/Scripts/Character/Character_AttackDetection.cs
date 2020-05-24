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
        atkFXCol.points = sO_AttackFX.collider.points;
        // Set other variables.
        List<Collider2D> collidersHit = new List<Collider2D>();
        List<Collider2D> collidersDamaged = new List<Collider2D>();
        float timer = 0f;
        bool detectCol = false;

        while (timer < thisColEnd) {
            timer += Time.deltaTime;
            if (timer >= thisColStart && !detectCol) {
                atkFXCol.enabled = true;
                detectCol = true;
            }
            if (detectCol) {
                // Detect hit collision.
                Physics2D.OverlapCollider(atkFXCol, hitLayers, collidersHit);
                foreach (Collider2D col in collidersHit)
                {
                    if (!collidersDamaged.Contains(col)) {
                        collidersDamaged.Add(col);
                        // Get weapon specific hit FX (and sound FX).
                        
                        col.GetComponent<Enemy_Health>().ReceiveDamage(WeapAtkChain.DamageRoll);
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

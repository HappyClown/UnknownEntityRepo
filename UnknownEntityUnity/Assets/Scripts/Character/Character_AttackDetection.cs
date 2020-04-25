using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AttackDetection : MonoBehaviour
{
    //public PolygonCollider2D[] attackColliders;
    
    public bool[] activeAttacks;
    // public PolygonCollider2D attackCollider;
    public ContactFilter2D hitLayers;
    //public List<Collider2D> collidersHit;
    //public List<Collider2D> collidersDamaged;

    public IEnumerator AttackCollider(PolygonCollider2D newCollider, float colStart, float colEnd, int chainNum, PolygonCollider2D atkFXCol) {
        if (activeAttacks[chainNum]) {
            activeAttacks[chainNum] = false;
            yield return null;
        }
        activeAttacks[chainNum] = true;
        int thisChainNum = chainNum;
        float timer = 0f;
        //float thisAttackLength = attackLength;
        float thisColStart = colStart;
        float thisColEnd = colEnd;
        //attackColliders[thisChainNum].enabled = true;
        bool detectCol = false;
        atkFXCol.points = newCollider.points;
        List<Collider2D> collidersHit = new List<Collider2D>();
        List<Collider2D> collidersDamaged = new List<Collider2D>();

        while (activeAttacks[thisChainNum] && timer < thisColEnd) {
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
                        col.GetComponent<Enemy_Health>().ReceiveDamage(5);
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
        activeAttacks[thisChainNum] = false;
        yield return null;
    }
}

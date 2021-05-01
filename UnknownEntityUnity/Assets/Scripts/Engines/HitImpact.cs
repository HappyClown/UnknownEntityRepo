using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitImpact : MonoBehaviour
{
    public ImpactFXPool impactFXPool;
    private static ImpactFXPool impactFXPool_St;
    private static ImpactFX impactFX_St;

    void Start() {
        impactFXPool_St = impactFXPool;
    }

    public static void PlayImpactFX(Collider2D hittingCollider, Vector2 receivingColliderPos, SO_ImpactFX sOImpactFX, LayerMask hitLayerMask, Collider2D receivingCollider) {
        // Get the hitting colldier's postion.
        Vector2 hittingColliderPos = (Vector2)hittingCollider.transform.position;
        // Request an ImpactFX script(attached to a GameObject) from an ImpactFX pool.
        impactFX_St = impactFXPool_St.RequestImpactFX();
        // Calculate the direction from the hitting colldier to the receiving collider.
        Vector2 dirToEnemy = (Vector2)receivingCollider.bounds.center - hittingColliderPos;
        // Does this impact keep the attack/projectile's direction.
        if (sOImpactFX.keepDirection) {
            impactFX_St.transform.up = hittingCollider.transform.up;
        }
        else {
            // Calculate and apply the direction from the hittingCollider to the receivingCollider.
            impactFX_St.transform.up = dirToEnemy;
        }
        // Fire a raycast from the hittingCollider to the receivingCollider to get a "hit" position, in order to place the impactFX close the the receiver.
        Debug.DrawRay(hittingColliderPos, dirToEnemy, Color.white, 1f);
        foreach(RaycastHit2D hit in Physics2D.RaycastAll(hittingColliderPos, dirToEnemy, dirToEnemy.magnitude, hitLayerMask)) {
            if (hit.collider == receivingCollider) {
                impactFX_St.transform.position = new Vector3(hit.point.x, hit.point.y, impactFX_St.transform.position.z);
                break;
            }
        }
        // Start the impactFX sprite animation.
        impactFX_St.StartImpactFX(sOImpactFX);
    }
}
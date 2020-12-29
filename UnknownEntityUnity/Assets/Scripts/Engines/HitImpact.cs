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

    public static void PlayImpactFX(Vector2 hittingColliderPos, Vector2 receivingColliderPos, SO_ImpactFX sOImpactFX, LayerMask hitLayerMask, Collider2D receivingCollider) {

        // Request an ImpactFX script(attached to a GameObject) from an ImpactFX pool.
        impactFX_St = impactFXPool_St.RequestImpactFX();
        // Calculate and apply the direction from the hittingCollider to the receivingCollider.
        Vector2 dirToEnemy = (Vector2)receivingCollider.bounds.center - hittingColliderPos;
        impactFX_St.transform.up = dirToEnemy;
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
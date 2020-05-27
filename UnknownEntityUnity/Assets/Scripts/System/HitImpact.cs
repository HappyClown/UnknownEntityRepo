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
        Vector2 dirToEnemy = receivingColliderPos - hittingColliderPos;
        impactFX_St.transform.up = dirToEnemy;
        // Fire a raycast from the hittingCollider to the receivingCollider to get a "hit" position, in order to place the impactFX close the the receiver.
//
        //receivingCollider.Raycast(dirToEnemy,);
        // Vector3 testhitcolpos = new Vector3(hittingColliderPos.x, hittingColliderPos.y, receivingCollider.gameObject.transform.position.z);
        // Vector3 testdirtoenemy = new Vector3(dirToEnemy.x, dirToEnemy.y, receivingCollider.gameObject.transform.position.z);
        // Ray ray = new Ray(testhitcolpos, testdirtoenemy);
        // float dist;
        // if (receivingCollider.bounds.IntersectRay(ray, out dist)) {
        //     print (dist);
        // }
        // Vector2 impactPoint = dirToEnemy.normalized * dist;
//
        // RaycastHit2D[] hits = Physics2D.RaycastAll(hittingColliderPos, dirToEnemy, dirToEnemy.magnitude, hitLayerMask);
        // print(hits.Length);
        foreach(RaycastHit2D hit in Physics2D.RaycastAll(hittingColliderPos, dirToEnemy, dirToEnemy.magnitude, hitLayerMask)) {
            if (hit.collider == receivingCollider) {
                impactFX_St.transform.position = new Vector3(hit.point.x, hit.point.y, impactFX_St.transform.position.z);
                break;
            }
        }
//
        //Vector2 impactPoint = Physics2D.Raycast(hittingColliderPos, dirToEnemy, dirToEnemy.magnitude, hitLayerMask).point;
        //impactFX_St.transform.position = new Vector3(impactPoint.x, impactPoint.y, impactFX_St.transform.position.z);
//
        // Start the impactFX sprite animation.
        impactFX_St.StartImpactFX(sOImpactFX);
    }
}

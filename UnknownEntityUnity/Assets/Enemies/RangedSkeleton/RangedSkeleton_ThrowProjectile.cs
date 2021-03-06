using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSkeleton_ThrowProjectile : MonoBehaviour
{
    public Enemy_Refs eRefs;
    public RangedSkeleton_Bow rsBow;
    public LayerMask blockLOSLayers;
    public Transform attackDirPoint;
    float sqrAtkRange;
    [Header("Cooldown")]
    public float cooldown;
    float cdTimer;
    public bool inProjThrow;
    public bool throwProjReady = true;

    void Start() {
        sqrAtkRange = eRefs.eSO.atkRange * eRefs.eSO.atkRange;
    }

    public bool CheckThrowProj() {
        if (!inProjThrow && throwProjReady) {
            // Check if the target is within attack range.
            if (eRefs.SqrDistToTarget(eRefs.PlayerCenterPos, this.transform.position) <= sqrAtkRange) {
            // Check to see if there are obstacles in the way. // Maybe switch to circle cas to make sure there is space to fire the projectile.
                if (!Physics2D.Raycast(attackDirPoint.position, eRefs.PlayerCenterPos - attackDirPoint.position, eRefs.DistToTarget(attackDirPoint.position, eRefs.PlayerCenterPos), blockLOSLayers)) {
                    return true;
                }
            }
        }
        return false;
    }

    public void StartProjectileThrow() {
        inProjThrow = true;
        throwProjReady = false;
        rsBow.rotateBow = true;
        rsBow.bowSpriteAnim.Play(rsBow.bowAnimClip);
        rsBow.StartCoroutine(rsBow.RotatingBow());
        //rsBow.StartCoroutine(rsBow.BowAnimation());
    }

    public void ProjectileAttackDone(bool allowPathUpdate = true) {
        StartCoroutine(Cooldown());
        inProjThrow = false;
        // Request movement type (run away, chase, idle if in atk range, etc), in this case it should be walking away from the player.
        if (allowPathUpdate) eRefs.eFollowPath.allowPathUpdate = true;
    }

    IEnumerator Cooldown() {
        cdTimer = 0f;
        yield return null;
        while (cdTimer < cooldown) {
            cdTimer += Time.deltaTime;
            yield return null;
        }
        throwProjReady = true;
    }
}

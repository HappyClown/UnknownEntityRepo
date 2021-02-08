using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSkeleton_ThrowProjectile : MonoBehaviour
{
    public Enemy_Refs eRefs;
    // public RangedSkeleton_Actions rsActions;
    public RangedSkeleton_Bow rsBow;
    public LayerMask blockLOSLayers;
    public Transform attackDirPoint;
    // public SO_Projectile projSO;
    // public ProjectilePool projPool;
    // Projectile proj;
    // [Header("Throw Projectile Attack")]
    // public Transform projOriginTrans;
    float sqrAtkRange;
    // Vector2 projDirNorm;
    [Header("Cooldown")]
    public float cooldown;
    float cdTimer;
    // [Header("Animation")]
    // public float totalDuration;
    // public Sprite[] sprites;
    // public float[] spriteChanges;
    // public float[] events;
    // public SpriteRenderer spriteR;
    // float distToTargetSqr;
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
        //StartCoroutine(ThrowProjectileAction()); // replace with the bow animation coroutine
        rsBow.StartCoroutine(rsBow.BowAnimation());
    }

    // IEnumerator ThrowProjectileAction() { // take out as it is now in the bow anim script
    //     // Could turn this into a scriptable object, to hold values for different types of projectiles.
    //     float timer = 0f;
    //     int spriteStep = 0;
    //     int eventStep = 0;
    //     projDirNorm = eRefs.NormDirToTargetV2(projOriginTrans.position, eRefs.PlayerPos);
    //     while (timer < totalDuration) {
    //         timer += Time.deltaTime;
    //         if (spriteStep < spriteChanges.Length && timer > spriteChanges[spriteStep]) {
    //             spriteR.sprite = sprites[spriteStep];
    //             spriteStep++;
    //         }
    //         if (eventStep < events.Length && timer > events[eventStep]) {
    //             if (eventStep == 0) {
    //                 SetupProjectile();
    //             }
    //             eventStep++;
    //         }
    //         yield return null;
    //     }
    //     // StartCoroutine(Cooldown());
    //     // inProjThrow = false;
    //     // // Request movement type (run away, chase, idle if in atk range, etc), in this case it should be walking away from the player.
    //     // //rsActions.RunAway();
    //     // eRefs.eFollowPath.allowPathUpdate = true;
    //     // //eRefs.unit.allowPathUpdate = true;
    //     yield return null;
    // }    

    public void ProjectileAttackDone(bool allowPathUpdate = true) {
        StartCoroutine(Cooldown());
        inProjThrow = false;
        // Request movement type (run away, chase, idle if in atk range, etc), in this case it should be walking away from the player.
        //rsActions.RunAway();
        if (allowPathUpdate) eRefs.eFollowPath.allowPathUpdate = true;
        //eRefs.unit.allowPathUpdate = true;
    }

    // public void SetupProjectile() { // transfer into the bow anim script
    //     proj = projPool.RequestProjectile();
    //     proj.LaunchProjectile(projSO, projDirNorm, projOriginTrans.position);
    // }

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

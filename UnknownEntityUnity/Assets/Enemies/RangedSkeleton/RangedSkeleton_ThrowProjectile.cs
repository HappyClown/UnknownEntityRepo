using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSkeleton_ThrowProjectile : MonoBehaviour
{
    public Enemy_Refs eRefs;
    public LayerMask blockLOSLayers;
    public SO_Projectile projSO;
    public ProjectilePool projPool;
    Projectile proj;
    [Header("Throw Projectile Attack")]
    public Transform projOriginTrans;
    float sqrAtkRange;
    Vector2 projDirNorm;
    [Header("Cooldown")]
    public float cooldown;
    float cdTimer;
    [Header("Animation")]
    public float totalDuration;
    public Sprite[] sprites;
    public float[] spriteChanges;
    public float[] events;
    public SpriteRenderer spriteR;

    void Start() {
        sqrAtkRange = eRefs.eSO.atkRange * eRefs.eSO.atkRange;
    }

    public void StartCheck() {
        StartCoroutine(CheckDistance());
    }

    IEnumerator CheckDistance() {
    // Could make it check every X frames / seconds.
        while (true) {
            // Check if the target is within attack range.
            float distToTargetSqr = (eRefs.plyrTrans.position - this.transform.position).sqrMagnitude;
            if (distToTargetSqr <= sqrAtkRange) {
                // Check to see if there are obstacles in the way.
                if (!Physics2D.Raycast(this.transform.position, eRefs.plyrTrans.position - this.transform.position, eRefs.eSO.atkRange, blockLOSLayers)) {
                    StartCoroutine(ThrowProjectileAction());
                    eRefs.unit.StopFollowPathCoroutine();
                    break;
                }
            }
            yield return null;
        }
    }

    IEnumerator ThrowProjectileAction() {
        // Could turn this into a scriptable object, to hold values for different types of projectiles.
        float timer = 0f;
        int spriteStep = 0;
        int eventStep = 0;
        projDirNorm = eRefs.NormDirToPlayerV2(projOriginTrans.position);
        while (timer < totalDuration) {
            timer += Time.deltaTime;
            if (spriteStep < spriteChanges.Length && timer > spriteChanges[spriteStep]) {
                spriteR.sprite = sprites[spriteStep];
                spriteStep++;
            }
            if (eventStep < events.Length && timer > events[eventStep]) {
                if (eventStep == 0) {
                    SetupProjectile();
                }
                eventStep++;
            }
            yield return null;
        }
        StartCoroutine(Cooldown());
        eRefs.unit.allowPathUpdate = true;
        yield return null;
    }    

    public void SetupProjectile() {
        proj = projPool.RequestProjectile();
        proj.LaunchProjectile(projSO, projDirNorm, projOriginTrans.position);
    }

    IEnumerator Cooldown() {
        cdTimer = 0f;
        yield return null;
        while (cdTimer < cooldown) {
            cdTimer += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(CheckDistance());
    }
}

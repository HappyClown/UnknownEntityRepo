using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSkeleton : Enemy_Specific
{
    public Enemy_Refs eRefs;
    [Header("ShieldBash Attack")]
    public SpriteRenderer atkSpriteR;
    public Collider2D atkCol;
    public Transform atkTrans, shieldTrans;
    public float atkSpawnDist = 0.5f, atkDur = 0.25f, atkMoveDist = 0.85f;
    public float moveDist, moveDur;
    public Vector2 atkDirNorm;
    Vector2 startPos, endPos;
    Vector2 startMovePos, endMovePos;
    Vector2 atkXY;
    [Header("ShieldUp Special")]
    public Sprite shieldUpSprite;
    [Range(0,1)]
    public float shieldUpSpeedMod = 0.5f;
    [Range(0,1)]
    public float shieldUpDamageMod = 0.5f;
    public float shieldUpRange = 2.5f, minShieldUpTime = 1f, shieldUpPlrRangeCheck = 0.5f;
    float shieldUpRangeSqr;
    bool shieldIsUp;
    //bool forceShieldDown = false;

    void Start() {
        shieldUpRangeSqr = shieldUpRange * shieldUpRange;
    }

    // void Update() {
    //     // Check if the target is within ShieldUp range.
    //     if (eRefs.SqrDistToPlayer(this.transform.position) <= shieldUpRangeSqr && !shieldIsUp && !forceShieldDown) {
    //         ShieldUp();
    //     }
    //     if (eRefs.eAtk.attacking && !forceShieldDown) {
    //         forceShieldDown = true;
    //         ShieldDown();
    //     }
    //     else if (!eRefs.eAtk.attacking && forceShieldDown) {
    //         forceShieldDown = false;
    //     }
    // }

    void ShieldUp() {
        // Slow down movement speed, reduce damage taken, change walking anim.
        shieldIsUp = true;
        eRefs.walkingSprite = shieldUpSprite;
        eRefs.eFollowPath.speedModifier *= shieldUpSpeedMod;
        eRefs.eHealth.damageModifier *= shieldUpDamageMod;
        StartCoroutine(Shielded());
    }
    void ShieldDown() {
        // Reverse movement slow down, reverse damage reduction, change walking anim.
        shieldIsUp = false;
        eRefs.walkingSprite = eRefs.eSO.walkingSprite;
        eRefs.eFollowPath.speedModifier /= shieldUpSpeedMod;
        eRefs.eHealth.damageModifier /= shieldUpDamageMod;
    }
    IEnumerator Shielded() {
        float timer = 0f;
        while (shieldIsUp) {
            timer += Time.deltaTime;
            // Minimum amount of time spent with shield up.
            if (timer > minShieldUpTime) {
                // Check if the target is within ShieldUp range.
                if (eRefs.SqrDistToTarget(this.transform.position, eRefs.PlayerPos) > shieldUpRangeSqr) {
                    ShieldDown();
                    break;
                }
                yield return new WaitForSeconds(shieldUpPlrRangeCheck);
            }
            yield return null;
        }
        yield return null;
    }

    public void ShieldBash() {
        // Get the attack's start and target positions based on the attack direction, spawn distance and move distance.
        EnemyMotion();
        //atkDirNorm = eRefs.eAtk.attackDir.normalized;
        startPos = (Vector2)shieldTrans.position + atkDirNorm;
        endPos = startPos + atkDirNorm * atkMoveDist;
        atkSpriteR.enabled = true;
        atkCol.enabled = true;
        StartCoroutine(ShieldBashSwing());
    }
    public void EnemyMotion() {
        //atkDirNorm = eRefs.eAtk.attackDir.normalized;
        startMovePos = this.transform.position;
        endMovePos = (Vector2)this.transform.position + atkDirNorm*moveDist;
        eRefs.eFollowPath.flip.PredictFlip(startMovePos, endMovePos);
        StartCoroutine(ShieldBashMovement());
    }
    IEnumerator ShieldBashSwing() {
        float timer = 0f;
        while (timer < 1) {
            timer += Time.deltaTime / atkDur;
            atkXY = Vector2.Lerp(startPos, endPos, timer);
            atkTrans.position = new Vector3(atkXY.x, atkXY.y, atkTrans.position.z);
            yield return null;
        }
        atkSpriteR.enabled = false;
        atkCol.enabled = false;
    }
    IEnumerator ShieldBashMovement() {
        float timer = 0f;
        Vector2 moveLerp = Vector2.zero;
        while (timer < 1) {
            timer += Time.deltaTime/moveDur;
            moveLerp = Vector2.Lerp(startMovePos, endMovePos, timer);
            this.transform.position = new Vector3 (moveLerp.x, moveLerp.y, this.transform.position.z);
            yield return null;
        }
    }

    public new void StopAttacks() {
        this.StopAllCoroutines();
        atkSpriteR.enabled = false;
        atkCol.enabled = false;
    }
}
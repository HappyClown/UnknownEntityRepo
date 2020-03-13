using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShieldSkeleton_ShieldBash : MonoBehaviour
{
    public Enemy_Refs eRefs;
    public LayerMask blockLOSLayers;
    public ShieldSkeleton_ShieldUp ssShieldUp;
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
    float sqrAtkRange;
    Vector2 attackDir;
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
        while (true) {
            // Check if the target is within attack range.
            float distToTargetSqr = (eRefs.plyrTrans.position - this.transform.position).sqrMagnitude;
            if (distToTargetSqr <= sqrAtkRange) {
                // Check to see if there are obstacles in the way.
                if (!Physics2D.Raycast(this.transform.position, eRefs.plyrTrans.position - this.transform.position, eRefs.eSO.atkRange, blockLOSLayers)) {
                    StartCoroutine(ShieldBash());
                    eRefs.unit.StopFollowPathCoroutine();
                    break;
                }
            }
            yield return null;
        }
    }

    IEnumerator ShieldBash() {
        float timer = 0f;
        int spriteStep = 0;
        int eventStep = 0;
        attackDir = eRefs.NormDirToPlayerV2(shieldTrans.position);
        while (timer < totalDuration) {
            timer += Time.deltaTime;
            if (spriteStep < spriteChanges.Length && timer > spriteChanges[spriteStep]) {
                spriteR.sprite = sprites[spriteStep];
                spriteStep++;
            }
            if (eventStep < events.Length && timer > events[eventStep]) {
                if (eventStep == 0) {
                    SetupEnemyMovement();
                    SetupBashProjectile();
                    ssShieldUp.ForceShieldDown();
                }
                eventStep++;
            }
            yield return null;
        }
        StartCoroutine(Cooldown());
        ssShieldUp.AllowShieldUp();
        eRefs.unit.allowPathUpdate = true;
    }

    public void SetupEnemyMovement() {
        atkDirNorm = attackDir.normalized;
        startMovePos = this.transform.position;
        endMovePos = (Vector2)this.transform.position + atkDirNorm*moveDist;
        eRefs.unit.flip.PredictFlip(startMovePos, endMovePos);
        StartCoroutine(Movement());
    }

    public void SetupBashProjectile() {
        // Get the attack's start and target positions based on the attack direction, spawn distance and move distance.
        atkDirNorm = attackDir.normalized;
        startPos = (Vector2)shieldTrans.position;
        endPos = startPos + atkDirNorm * atkMoveDist;
        atkSpriteR.enabled = true;
        atkCol.enabled = true;
        StartCoroutine(Projectile());
    }

    IEnumerator Movement() {
        float timer = 0f;
        Vector2 moveLerp = Vector2.zero;
        while (timer < 1) {
            timer += Time.deltaTime/moveDur;
            moveLerp = Vector2.Lerp(startMovePos, endMovePos, timer);
            this.transform.position = new Vector3 (moveLerp.x, moveLerp.y, this.transform.position.z);
            yield return null;
        }
    }

    IEnumerator Projectile() {
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

    IEnumerator Cooldown() {
        cdTimer = 0f;
        yield return null;
        while (cdTimer < cooldown) {
            cdTimer += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(CheckDistance());
    }

    public void StopAction() {
        this.StopAllCoroutines();
        atkSpriteR.enabled = false;
        atkCol.enabled = false;
        this.enabled = false;
    }
}

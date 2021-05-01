using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShieldSkeleton_ShieldBash : MonoBehaviour
{
    public Enemy_Refs eRefs;
    public ShieldSkeleton_ShieldUp ssShieldUp;
    [Header("ShieldBash Attack")]
    //public SpriteRenderer atkSpriteR;
    public Collider2D atkCol;
    List <Collider2D> atkCollisionsHit = new List<Collider2D>();
    List <Transform> hitTransforms = new List<Transform>();
    public ContactFilter2D atkContactFilter;
    //public Transform atkTrans, shieldTrans;
    //public float /* atkSpawnDist = 0.5f, */ atkDur = 0.25f, atkMoveDist = 0.85f;
    public float moveDist, moveDur;
    public float minAtkDamage, maxAtkDamage;
    public SO_ImpactFX sO_ImpactFX;
    public Transform attackDirPoint;
    Vector2 atkDirNorm;
    //Vector2 startPos, endPos;
    Vector2 startMovePos, endMovePos;
    //Vector2 atkXY;
    float sqrAtkRange;
    Vector2 attackDir;
    [Header("Cooldown")]
    public bool inBash = false;
    public float cooldown;
    float cdTimer;
    [Header("Animation")]
    public float totalDuration;
    public Sprite[] sprites;
    public float[] spriteChanges;
    public float[] events;
    public SpriteRenderer spriteR;
    Vector3 oldPos, newPos;
    public Sprite[] loopingBashSprites;
    public float loopSpriteDuration;
    public Sprite shieldDownSprite;
    private int loopTicks;
    private float loopTimer;
    public float beforeShieldDownDuration;
    float moveSpeed;
    Coroutine movementCoro, shieldBashCoro;

    float Damage {
        get {
            return Random.Range(minAtkDamage, maxAtkDamage);
        }
    }

    void Start() {
        sqrAtkRange = eRefs.eSO.atkRange * eRefs.eSO.atkRange;
        cdTimer = cooldown;
    }

    public bool CheckToBash() {
        // Check if im currently attacking and if its off cooldown.
        if (!inBash && cdTimer >= cooldown) {
            // Check if the target is within attack range.
            if (eRefs.SqrDistToTarget(eRefs.PlayerCenterPos, attackDirPoint.position) <= sqrAtkRange) {
                // Check to see if there are obstacles in the way.
                if (!Physics2D.Raycast(attackDirPoint.position, (Vector2)eRefs.PlayerCenterPos - (Vector2)attackDirPoint.position, eRefs.DistToTarget(attackDirPoint.position, eRefs.PlayerCenterPos), eRefs.losLayerMask)) {
                    return true;
                }
            }
        }
        return false;
    }

    public void StartShieldBash() {
        inBash = true;
        shieldBashCoro = StartCoroutine(ShieldBash());
    }

    // Enemy sprite changes for the ShieldBash attack, decides when does movement start, start cooldown.
    IEnumerator ShieldBash() {
        float timer = 0f;
        int spriteStep = 0;
        int eventStep = 0;
        bool flipEnemy = true;
        // eRefs.eFollowPath.flip.PredictFlip(this.transform.position, eRefs.PlayerShadowPos);
        //attackDir = eRefs.NormDirToTargetV2(attackDirPoint.position, eRefs.PlayerCenterPos);
        while (timer < totalDuration) {
            timer += Time.deltaTime;
            if (spriteStep < spriteChanges.Length && timer > spriteChanges[spriteStep]) {
                spriteR.sprite = sprites[spriteStep];
                spriteStep++;
            }
            if (eventStep < events.Length && timer > events[eventStep]) {
                if (eventStep == 0) {
                    // Stop flipping the enemy to look at the player and grab the direction for the charge attack.
                    flipEnemy = false;
                    attackDir = eRefs.NormDirToTargetV2(attackDirPoint.position, eRefs.PlayerCenterPos);
                }
                else if (eventStep == 1) {
                    SetupEnemyMovement();
                }
                eventStep++;
            }
            if (flipEnemy) {
                eRefs.eFollowPath.flip.PredictFlip(this.transform.position, eRefs.PlayerShadowPos);
            }
            yield return null;
        }
        StartCoroutine(Cooldown());
        //eRefs.eFollowPath.allowPathUpdate = true;
        inBash = false;
    }

    public void SetupEnemyMovement() {
        atkDirNorm = attackDir.normalized;
        startMovePos = this.transform.position;
        endMovePos = (Vector2)this.transform.position + atkDirNorm*moveDist;
        eRefs.eFollowPath.flip.PredictFlip(startMovePos, endMovePos);
        this.transform.forward = Vector2.up;
        movementCoro = StartCoroutine(Movement());
    }
    
    IEnumerator Movement() {
        float timer = 0f;
        Vector2 moveLerp = Vector2.zero;
        moveSpeed = moveDist/moveDur;
        oldPos = this.transform.position;
        atkCol.enabled = true;
        while (timer < 1) {
            timer += Time.deltaTime/moveDur;
            // Go through a sprite loop during the bash movement.
            loopTimer += Time.deltaTime;
            if (loopTimer > loopSpriteDuration) {
                spriteR.sprite = loopingBashSprites[loopTicks];
                loopTicks++;
                loopTimer = 0f;
                if (loopTicks >= loopingBashSprites.Length) {
                    loopTicks = 0;
                }
            }
            // Check attack collisions.
            if (atkCol.OverlapCollider(atkContactFilter, atkCollisionsHit) > 0) {
                CheckColliders();
            }
            // Check collision movement.
            oldPos = this.transform.position;
            newPos = oldPos + (Vector3)atkDirNorm * moveSpeed * Time.deltaTime;
            eRefs.eCol.boxCol.gameObject.transform.position = this.transform.position;
            this.transform.position = eRefs.eCol.CollisionCheck(atkDirNorm, newPos, oldPos);
            yield return null;
        }
        atkCol.enabled = false;
        EmptyCollisionLists();
        // At the end of the movement, the rest of the TotalDuration - (event[0] + movementduration), play sprite with shield down.
        yield return new WaitForSeconds(beforeShieldDownDuration);
        spriteR.sprite = shieldDownSprite;
        // Become vulnerable to damage again at the end of the bash movement.
        ssShieldUp.ForceShieldDown();
    }

    void CheckColliders() {
        if (atkCollisionsHit.Count > 0 && atkCollisionsHit[0] != null) {
            foreach(Collider2D col in atkCollisionsHit) {
                // Check if the hit collider's transform has already been hit, if so, move on.
                foreach(Transform hitTrans in hitTransforms) {
                    if (col.transform == hitTrans) {
                        return;
                    }
                }
                if (col.CompareTag("Player")) { // Change for variable reference to Tag to allow being used by player?
                    // Hit impact FX. Apply the correct rotation, position, sprites and layerMask to an impactFX.
                    HitImpact.PlayImpactFX(atkCol, col.transform.position, sO_ImpactFX, atkContactFilter.layerMask, col);
                    // Apply damage to the player, also give it the direction from the hittingCollider to the playerCollider.
                    col.GetComponent<Character_Health>().TakeDamage(Damage, eRefs.NormDirToTargetV2(atkCol.transform.position, col.transform.position));
                    hitTransforms.Add(col.transform);
                }
                else if (col.CompareTag("Destructible")) {
                    col.GetComponent<Clutter_Health>().ReceiveDamage(Damage, atkCol.transform.position, col.bounds.center);
                    hitTransforms.Add(col.transform);
                    // Get destructible script and apply damage to the object.
                }
            }
        }
    }

    void EmptyCollisionLists() {
        atkCollisionsHit.Clear();
        hitTransforms.Clear();
    }

    public IEnumerator Cooldown() {
        cdTimer = 0f;
        yield return null;
        while (cdTimer < cooldown) {
            cdTimer += Time.deltaTime;
            yield return null;
        }
    }

    public void StopAction() {
        // Dont stop the cooldown.
        //StopCoroutine(movementCoro);
        StopCoroutine(shieldBashCoro);
        
        inBash = false;
        //atkSpriteR.enabled = false;
        atkCol.enabled = false;
        EmptyCollisionLists();
        //this.enabled = false;
    }
}

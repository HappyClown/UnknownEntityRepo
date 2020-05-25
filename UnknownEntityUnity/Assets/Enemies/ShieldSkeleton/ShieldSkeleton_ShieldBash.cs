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
            //print("inbash and cooldown all good.");
            // Check if the target is within attack range.
            if (eRefs.SqrDistToTarget(eRefs.PlayerPos, this.transform.position) <= sqrAtkRange) {
            //print("distance is good.");
                // Check to see if there are obstacles in the way.
                if (!Physics2D.Raycast(this.transform.position, (Vector2)eRefs.PlayerPos - (Vector2)this.transform.position, eRefs.DistToTarget(this.transform.position, eRefs.PlayerPos), eRefs.losLayerMask)) {
                    //print("nothing in the way");
                    return true;
                }
            }
        }
        return false;
    }

    public void StartShieldBash() {
        inBash = true;
        StartCoroutine(ShieldBash());
    }
    // Enemy sprite changes for the ShieldBash attack, when does movement start, start cooldown.
    IEnumerator ShieldBash() {
        float timer = 0f;
        int spriteStep = 0;
        int eventStep = 0;
        eRefs.eFollowPath.flip.PredictFlip(this.transform.position, eRefs.PlayerPos);
        attackDir = eRefs.NormDirToTargetV2(this.transform.position, eRefs.PlayerPos);
        while (timer < totalDuration) {
            timer += Time.deltaTime;
            if (spriteStep < spriteChanges.Length && timer > spriteChanges[spriteStep]) {
                spriteR.sprite = sprites[spriteStep];
                spriteStep++;
            }
            if (eventStep < events.Length && timer > events[eventStep]) {
                if (eventStep == 0) {
                    SetupEnemyMovement();
                    //SetupBashProjectile();
                    //ssShieldUp.ForceShieldDown();
                }
                eventStep++;
            }
            yield return null;
        }
        StartCoroutine(Cooldown());
        //ssShieldUp.AllowShieldUp();
        eRefs.eFollowPath.allowPathUpdate = true;
        inBash = false;
    }

    public void SetupEnemyMovement() {
        atkDirNorm = attackDir.normalized;
        startMovePos = this.transform.position;
        endMovePos = (Vector2)this.transform.position + atkDirNorm*moveDist;
        eRefs.eFollowPath.flip.PredictFlip(startMovePos, endMovePos);
        this.transform.forward = Vector2.up;
        StartCoroutine(Movement());
    }

    // public void SetupBashProjectile() {
    //     // Get the attack's start and target positions based on the attack direction, spawn distance and move distance.
    //     atkDirNorm = attackDir.normalized;
    //     startPos = (Vector2)shieldTrans.position;
    //     endPos = startPos + atkDirNorm * atkMoveDist;
    //     atkSpriteR.enabled = true;
    //     atkCol.enabled = true;
    //     // Instead of a projectile, activate a collider or check collision with the enemy's collider.
    //     StartCoroutine(Projectile());
    // }
    // Shield bash movement - Change from lerp to translate or Transform+Movement*Time.deltaTime*Speed, this would change it from going to X position over X time to moving at X speed for X time in X direction, the speed can be deduced from the previous X position, X current postion and X time basically changing the movement method without having to get new variables just adding some calculations. So the X position calculated from the X move would only be a means to get the distance, which can probably be taken straight from the X move making the X position calculation superfluus.

    // Use X move and X duration to calculate an X speed, make the enemy translate or transform in X direction at X speed for X duration. The purpose of setting an X move instead of directly setting an X speed is to know exactly the distance the attack movement will cover if there are no obstacles.
    
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
                //print("Should check colliders");
                CheckColliders();
            }
            // Check collision movement.
            oldPos = this.transform.position;
            newPos = oldPos + (Vector3)atkDirNorm * moveSpeed * Time.deltaTime;
            //float printMag = (newPos - oldPos).magnitude;
            //float printX = newPos.x - oldPos.x;
            //float printY = newPos.y - oldPos.y;
            eRefs.eCol.boxCol.gameObject.transform.position = this.transform.position;
            //print("DELTA TIME: "+Time.deltaTime);
            //print ("NEW-OLD MAGNITUDE: "+printMag+", X: "+printX+", Y: "+printY);
            this.transform.position = eRefs.eCol.CollisionCheck(atkDirNorm, newPos, oldPos);
            //print("FINAL POSITION: "+"("+transform.position.x+", "+transform.position.y+", "+transform.position.z+")");
            //print("-------------------------");
            yield return null;
            // The final problem is indeed the box collider position sometimes not being updated in time despite the objects position being updated. This causes issue because the raycast position bases itself on the collider's bounds. The result is that it occasionally casts the rays from the previous frame's position again and applies the same but now erroneous values.
            // The solution is to calculate where the bounds are based on the collider's size:
            // curPos.x + (boxCol.Size.x / 2) = boxCol.bounds.max.x;
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
                if (col.CompareTag("Player")) { // Change for variable reference to Tag to allow being used by player???
                    // Calculate hit direction, partially for the Hit FX.
                    Vector2 hitDirToPlyr = eRefs.NormDirToTargetV2(this.transform.position, eRefs.plyrTrans.position);
                    Vector2 hitPos = Vector2.zero;
                    RaycastHit2D hit = Physics2D.Raycast(atkCol.transform.position, eRefs.NormDirToTargetV2(atkCol.transform.position, col.transform.position), eRefs.DistToTarget(atkCol.transform.position, col.transform.position), atkContactFilter.layerMask);
                    if (hit) {
                        hitPos = hit.point;
                    }
                    // print("HIT DIR TO PLYR: "+hitDirToPlyr+"  &&  HIT POS: "+hitPos);
                    // Apply damage to the player.
                    col.GetComponent<Character_Health>().TakeDamage(Damage, hitDirToPlyr, hitPos);
                    hitTransforms.Add(col.transform);
                    //print("Shield bash hit the Player");
                }
                else if (col.CompareTag("Destructible")) {
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

    // IEnumerator Projectile() {
    //     float timer = 0f;
    //     while (timer < 1) {
    //         timer += Time.deltaTime / atkDur;
    //         atkXY = Vector2.Lerp(startPos, endPos, timer);
    //         atkTrans.position = new Vector3(atkXY.x, atkXY.y, atkTrans.position.z);
    //         yield return null;
    //     }
    //     atkSpriteR.enabled = false;
    //     atkCol.enabled = false;
    // }

    IEnumerator Cooldown() {
        cdTimer = 0f;
        yield return null;
        while (cdTimer < cooldown) {
            cdTimer += Time.deltaTime;
            yield return null;
        }
    }

    public void StopAction() {
        this.StopAllCoroutines();
        //atkSpriteR.enabled = false;
        atkCol.enabled = false;
        EmptyCollisionLists();
        this.enabled = false;
    }
}

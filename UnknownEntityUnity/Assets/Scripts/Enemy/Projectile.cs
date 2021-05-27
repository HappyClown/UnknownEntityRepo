using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerTools;

public class Projectile : MonoBehaviour
{
    public bool inUse;
    float projDurationTimer, projDuration;
    SO_Projectile projSO;
    public SpriteRenderer mySpriteR;
    public PolygonCollider2D myCol;
    public SpriteAnim mySpriteAnim;
    [Header("Projectile Animation")]
    float animTotalDuration;
    Sprite[] animSprites;
    float[] animTimings;
    //ContactFilter2D contactFilter;
    List<Collider2D> collidedList = new List<Collider2D>(1);
    List<Collider2D> collidersDamaged = new List<Collider2D>();
    bool colStarted, colEnded;
    AnimationCurve speedCurve;
    float curveTimer;
    float curveTimerAdjustment;
    float curProjSpeed;

    // Particle System To Turn Off
    ParticleSystem partSys;
    Transform partSysParent;
    //Vector2 direction;

    public void LaunchProjectile(SO_Projectile _projSO, Vector2 _direction, Vector2 startPos, float flipXDirection = 0f/* , ParticleSystem _partSys = null, Transform _partSysParent = null */) {
        inUse = true;
        mySpriteR.sortingOrder = _projSO.sortingOrder;
        projDurationTimer = 0f;
        //
        if (flipXDirection < 0f) {
            mySpriteR.flipX = true;
        }
        // partSys = _partSys;
        // partSysParent = _partSysParent;
        projSO = _projSO;
        projDuration = projSO.duration;
        curProjSpeed = projSO.maxSpeed;
        // If projectile uses a speed curve.
        if (projSO.useSpeedCurve) {
            curveTimer = 0f;
            curveTimerAdjustment = 1 / projDuration;
            speedCurve = projSO.speedCurve;
        }
        //contactFilter = projSO.contactFilter;
        //direction = _direction;
        this.transform.position = new Vector3(startPos.x, startPos.y, this.transform.position.z);
        this.transform.up = _direction;
        this.transform.position += (Vector3)_direction.normalized * projSO.spawnDistance;
        // Activating the game object here in case the animation coroutine needs to be, 
        this.gameObject.SetActive(true);
        // Check if the sprite is animated, if yes start animation coroutine? Else just take the single sprite.
        if (!projSO.animated) {
            mySpriteR.sprite = projSO.sprite; 
        }
        else {
            // Setup animation.
            // animSprites = projSO.animSprites;
            // animTimings = projSO.animTimings;
            // animTotalDuration = projSO.animTotalDuration;
            // mySpriteR.sprite = animSprites[0];
            // StartCoroutine(AnimateProjectile());
            // With PowerSpriteAnimator.
            if (projDuration < projSO.animClip.length) {
                Debug.Log("Hello, be careful, a projectile that was just started has a duration that is shorter then its animation clip's lenght(duration). This means that the animation clip will not fully play once.");
            }
            mySpriteAnim.Play(projSO.animClip);
        }
        // Assign collider.
        myCol.points = projSO.col.points;
        //myCol = projSO.col;
        StartCoroutine(MoveProjectile());
    }

    IEnumerator MoveProjectile() {
        while (projDurationTimer < projDuration) {
            projDurationTimer += Time.deltaTime;
            // Check when to activate/deactivate the collider.
            if (!colStarted && projDurationTimer > projSO.colStartTime) {
                myCol.enabled = true;
                colStarted = true;
            }
            if (!colEnded && projDurationTimer > projSO.colEndTime) {
                myCol.enabled = false;
                colEnded = true;
            }
            // If projectile uses a speed curve.
            if (projSO.useSpeedCurve) {
                curveTimer+=Time.deltaTime*curveTimerAdjustment;
                curProjSpeed = speedCurve.Evaluate(curveTimer) * projSO.maxSpeed;
            }
            // Move the projectile.
            transform.Translate(Vector2.up * curProjSpeed * Time.deltaTime);
            // Check collisions.
            if (myCol.OverlapCollider(projSO.contactFilter, collidedList) > 0) {
                //print("Should check colliders");
                CheckColliders();
            }
            yield return null;
        }
        DeactivateProjectile();
        yield return null;
    }

    IEnumerator AnimateProjectile() {
        int thisSpriteIndex = 0;
        float timer = 0f;
        while (projDurationTimer < projDuration) {
            timer += Time.deltaTime;
            if (timer >= animTimings[thisSpriteIndex]) {
                mySpriteR.sprite = animSprites[thisSpriteIndex];
                if (thisSpriteIndex < animTimings.Length -1) {
                    thisSpriteIndex++;
                }
            }
            if (timer >= animTotalDuration) {
                thisSpriteIndex = 0;
                timer = 0f;
            }
            yield return null;
        }
        yield return null;
    }

    void CheckColliders() {
        if (collidedList.Count > 0 && collidedList[0] != null) {
            foreach(Collider2D col in collidedList) {
                if (!collidersDamaged.Contains(col)) {
                    collidersDamaged.Add(col);
                    if (col.CompareTag("Player")) { // Change for variable reference to Tag to allow being used by player???
                        Character_Health charHealth = col.GetComponent<Character_Health>();
                        // Hit impact FX. Apply the correct rotation, position, sprites and layerMask to an impactFX.
                        //HitImpact.PlayImpactFX(myCol.transform.position, col.transform.position, projSO.sO_ImpactFX, projSO.contactFilter.layerMask, col);
                        // Apply damage to the player.
                        col.GetComponent<Character_Health>().TakeDamage(projSO.Damage, ((Vector2)charHealth.transform.position - (Vector2)this.transform.position).normalized);
                        //print("Projectile hit the Player");
                    }
                    else if (col.CompareTag("Destructible")) {
                        // Get destructible script and apply damage to the object.
                        col.GetComponent<Clutter_Health>().ReceiveDamage(projSO.Damage, myCol.transform.position, col.bounds.center);
                        //print("ARO HIT DA BOXES YA.");
                    }
                    else {
                        // This should be the obstacles that just destroy the projectile nothing else.
                        //print("Projectile hit the a Wall most likely.");
                    }
                    // Turn off the projectile with collision.
                    if (projSO.destroyOnContact) {
                        HitImpact.PlayImpactFX(myCol, col.transform.position, projSO.sO_ImpactFX, projSO.contactFilter.layerMask, col);
                        DeactivateProjectile();
                    }
                }
            }
        }
    }

    void DeactivateProjectile() {
        inUse = false;
        colStarted = false;
        colEnded = false;
        collidersDamaged.Clear();
        myCol.enabled = false;
        mySpriteAnim.Stop();
        mySpriteR.sortingOrder = 0;
        mySpriteR.flipX = false;
        //HitImpact.PlayImpactFX()
        // if (partSys != null) { 
        //     partSys.Stop(); 
        // }
        // if (partSysParent != null) {
        //     partSys.transform.parent = partSysParent; 
        // }
        // partSys = null;
        // partSysParent = null;
        this.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSkeleton_ShieldUp : MonoBehaviour
{    
    public Enemy_Refs eRefs;
    [Header("ShieldUp Special")]
    public Sprite shieldUpSprite;
    public SpriteRenderer shieldSpriteR;
    [Range(0,1)]
    public float shieldUpSpeedMod = 0.5f;
    [Range(0,1)]
    public float shieldUpDamageMod = 0.5f;
    public float shieldUpRange = 2.5f, minShieldUpTime = 1f, shieldUpPlrRangeCheck = 0.5f;
    public float shieldUpCooldown;
    float shieldUpRangeSqr;
    public bool shieldIsUp;
    bool onCooldown;
    public bool forceShieldDown = false;
    public Sprite[] shieldUpWalkCycle;

    void Start() {
        shieldUpRangeSqr = shieldUpRange * shieldUpRange;
    }

    void Update() { // This could be in a Coroutine.
        // Check if the target is within ShieldUp range.
        if (!onCooldown && !shieldIsUp && !forceShieldDown && eRefs.SqrDistToTarget(this.transform.position, eRefs.PlayerPos) <= shieldUpRangeSqr ) {
            ShieldUp();
        }
    }

    void ShieldUp() {
        // Slow down movement speed, reduce damage taken, change walking anim.
        shieldIsUp = true;
        eRefs.eHealth.canBeStunned = false;
        //eRefs.walkingSprite = shieldUpSprite;
        shieldSpriteR.enabled = true;
        eRefs.eFollowPath.speedModifier *= shieldUpSpeedMod;
        eRefs.eHealth.damageModifier *= shieldUpDamageMod;
        // Change the walking sprite array to the shield up sprites.
        eRefs.eWalkAnim.ChangeCycleAnimation(shieldUpWalkCycle);
        StartCoroutine(Shielded());
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
    void ShieldDown() {
        // Reverse movement slow down, reverse damage reduction, change walking anim.
        shieldIsUp = false;
        eRefs.eHealth.canBeStunned = true;
        //eRefs.walkingSprite = eRefs.eSO.walkingSprite;
        shieldSpriteR.enabled = false;
        eRefs.eFollowPath.speedModifier /= shieldUpSpeedMod;
        eRefs.eHealth.damageModifier /= shieldUpDamageMod;
        // Set the walk cycle back to the shield down walk cycle.
        eRefs.eWalkAnim.ChangeCycleAnimation(eRefs.eSO.walkingSprites);
        SetShieldUpCooldown();
    }
    public void ForceShieldDown() {
        forceShieldDown = true;
        if (shieldIsUp) {
            ShieldDown();
        }
    }    
    public void AllowShieldUp() {
        forceShieldDown = false;
    }
    public void SetShieldUpCooldown() {
        // If the shield cooldown is triggered and its already on cooldown, stop that cooldown routine and start a new one.
        if (onCooldown) {
            StopCoroutine(ShieldUpCooldownCR());
        }
        StartCoroutine(ShieldUpCooldownCR());
    }
    IEnumerator ShieldUpCooldownCR() {
        float timer = 0f;
        onCooldown = true;
        while(timer < shieldUpCooldown) {
            timer += Time.deltaTime;
            yield return null;
        }
        yield return null;
        onCooldown = false;
    }
}

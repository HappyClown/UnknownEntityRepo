using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSkeleton_ShieldUp : MonoBehaviour
{    
    public Enemy_Refs eRefs;
    [Header("ShieldUp Special")]
    public Sprite shieldUpSprite;
    [Range(0,1)]
    public float shieldUpSpeedMod = 0.5f;
    [Range(0,1)]
    public float shieldUpDamageMod = 0.5f;
    public float shieldUpRange = 2.5f, minShieldUpTime = 1f, shieldUpPlrRangeCheck = 0.5f;
    float shieldUpRangeSqr;
    bool shieldIsUp;
    bool forceShieldDown = false;

    void Start() {
        shieldUpRangeSqr = shieldUpRange * shieldUpRange;
    }

    void Update() { // This could be in a Coroutine.
        // Check if the target is within ShieldUp range.
        if (eRefs.SqrDistToPlayer(this.transform.position) <= shieldUpRangeSqr && !shieldIsUp && !forceShieldDown) {
            ShieldUp();
        }
    }

    void ShieldUp() {
        // Slow down movement speed, reduce damage taken, change walking anim.
        shieldIsUp = true;
        eRefs.walkingSprite = shieldUpSprite;
        eRefs.unit.speedModifier *= shieldUpSpeedMod;
        eRefs.eHealth.damageModifier *= shieldUpDamageMod;
        StartCoroutine(Shielded());
    }
    void ShieldDown() {
        // Reverse movement slow down, reverse damage reduction, change walking anim.
        shieldIsUp = false;
        eRefs.walkingSprite = eRefs.eSO.walkingSprite;
        eRefs.unit.speedModifier /= shieldUpSpeedMod;
        eRefs.eHealth.damageModifier /= shieldUpDamageMod;
    }
    IEnumerator Shielded() {
        float timer = 0f;
        while (shieldIsUp) {
            timer += Time.deltaTime;
            // Minimum amount of time spent with shield up.
            if (timer > minShieldUpTime) {
                // Check if the target is within ShieldUp range.
                if (eRefs.SqrDistToPlayer(this.transform.position) > shieldUpRangeSqr) {
                    ShieldDown();
                    break;
                }
                yield return new WaitForSeconds(shieldUpPlrRangeCheck);
            }
            yield return null;
        }
        yield return null;
    }

    public void ForceShieldDown() {
        forceShieldDown = true;
        ShieldDown();
    }    
    public void AllowShieldUp() {
        forceShieldDown = false;
    }
}

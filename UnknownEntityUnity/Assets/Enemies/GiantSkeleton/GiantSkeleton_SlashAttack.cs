using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantSkeleton_SlashAttack : MonoBehaviour
{
    public Enemy_Refs eRefs;
    public bool inAtk;
    public bool inCooldown;
    public SO_AnimationValues aV;
    public SO_Projectile pV;
    public Transform attackDirPoint;
    public ProjectilePool projPool;

    public bool CheckToSlash() {
        // Check the distance to the player, slash
        if (Vector2.Distance(eRefs.PlayerCenterPos, attackDirPoint.position) < eRefs.eSO.atkRange) {
            return true;
        }
        return false;
    }

    public void StartSlashAttack() {
        // attack direction
        inAtk = true;
        StartCoroutine(SlashAttack());
    }

    IEnumerator SlashAttack() {
        float timer = 0f;
        int spriteStep = 0;
        int eventStep = 0;
        eRefs.eFollowPath.flip.PredictFlip(this.transform.position, eRefs.PlayerShadowPos);
        // attack direction can be taken at any point during the animation with an event
        Vector2 attackDir = eRefs.NormDirToTargetV2(attackDirPoint.position, eRefs.PlayerCenterPos);
        while (timer < aV.totalDuration) {
            timer += Time.deltaTime;
            if (spriteStep < aV.changeSprites.Length && timer > aV.changeSprites[spriteStep]) {
                eRefs.eSpriteR.sprite = aV.sprites[spriteStep];
                spriteStep++;
            }
            if (eventStep < aV.eventTriggers.Length && timer > aV.eventTriggers[eventStep]) {
                if (eventStep == 0) {
                    // Launch slash projectile in the appropriate direction.
                    projPool.RequestProjectile().LaunchProjectile(pV, attackDir, attackDirPoint.position);
                    // Projectile proj = projPool.RequestProjectile();
                    // proj.LaunchProjectile(pV, attackDir, attackDirPoint.position);
                }
                eventStep++;
            }
            yield return null;
        }
        inAtk = false;
        inCooldown = true;
        yield return new WaitForSeconds(eRefs.eSO.attackCooldown);
        inCooldown = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GiantSkeleton_SlashAttack : MonoBehaviour
{
    public Enemy_Refs eRefs;
    public bool inAtk;
    public bool inCooldown;
    public SO_AnimationValues aV;
    public SO_Projectile pV;
    public Transform attackDirPoint;
    public ProjectilePool projPool;
    private Vector2 slashProjDirection;

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
        eRefs.mySpriteAnim.Play(eRefs.animClips[1]);
        //eRefs.mySpriteAnim.SetSpeed(2);
        //StartCoroutine(SlashAttack());
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
            if (spriteStep < aV.spriteTimings.Length && timer > aV.spriteTimings[spriteStep]) {
                eRefs.eSpriteR.sprite = aV.sprites[spriteStep];
                spriteStep++;
            }
            if (eventStep < aV.eventTriggers.Length && timer > aV.eventTriggers[eventStep]) {
                this.SendMessageUpwards(aV.eventMessages[eventStep]);
                //if (eventStep == 0) {
                    // aV.unityEvents[0].Invoke();
                    // Launch slash projectile in the appropriate direction.
                    //projPool.RequestProjectile().LaunchProjectile(pV, attackDir, attackDirPoint.position);
                    // Projectile proj = projPool.RequestProjectile();
                    // proj.LaunchProjectile(pV, attackDir, attackDirPoint.position);
                //}
                eventStep++;
            }
            yield return null;
        }
        inAtk = false;
        inCooldown = true;
        yield return new WaitForSeconds(eRefs.eSO.attackCooldown);
        inCooldown = false;
    }

    public void AnimLaunchSlashProjectile() {
        projPool.RequestProjectile().LaunchProjectile(pV, slashProjDirection, attackDirPoint.position);
    }

    public void AnimStartSlashCooldown() {
        eRefs.mySpriteAnim.Stop();
        StartCoroutine(SlashCooldown());
    }

    public void AnimFlipTowardsPlayer() {
        eRefs.flipX.FlipTowards(attackDirPoint.position, eRefs.PlayerShadowPos);
    }

    public void AnimGetDirToPlayer() {
        slashProjDirection = eRefs.NormDirToTargetV2(attackDirPoint.position, eRefs.PlayerCenterPos);
    }

    IEnumerator SlashCooldown() {
        inAtk = false;
        inCooldown = true;
        yield return new WaitForSeconds(eRefs.eSO.attackCooldown);
        inCooldown = false;
    }

    public void StopSlashAttack() {
        // One of the next two to stop the anim clip if it is the slash one playing now.
        // AnimationClip curClip = eRefs.mySpriteAnim.GetCurrentAnimation();
        // if (curClip = eRefs.animClips[1]) {
        //     eRefs.mySpriteAnim.Stop();
        // }
        if (eRefs.mySpriteAnim.Clip == eRefs.animClips[1]) {
            eRefs.mySpriteAnim.Stop();
        }
        //this.StopAllCoroutines(); // NOT NEEDED ATM SINCE the animation and the events are HANDLED by the animation and not by coroutines.
        StartCoroutine(SlashCooldown());

    }
}

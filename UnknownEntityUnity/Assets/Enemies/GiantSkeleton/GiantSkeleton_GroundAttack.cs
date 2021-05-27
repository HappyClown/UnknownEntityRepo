using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantSkeleton_GroundAttack : MonoBehaviour
{
    // if player is within X range for X seconds while chase player, trigger ground attack.

    // ground attack spikes; take direction to target have a marx range for the first one but spawn it closer if the player is closer.

    // If too easy, create function to predict player position based on current position+(direction*speed)
    public bool inAtk;
    public bool inCooldown;
    public Enemy_Refs eRefs;
    public GiantSkeleton_SlashAttack giantSkel_SlashAtk;
    public ProjectilePool projPool;
    public SO_Projectile projValues;
    public SO_Projectile lastProjValues;
    public Transform groundAttackOrigin;
    public Transform attackDirPoint;
    public float triggerRadius;
    public float triggerThreshold;
    private float triggerValue;
    public float firstSpikeMaxDist;
    // TEMP
    [Header("Spike Values")]
    public float spikeSpawnDist;
    public float delayBetweenSpikes;
    public int spikeAmount = 1;
    private Vector2 normDirToPlayer;
    public bool CheckToAttack()
    {
        if (inAtk || inCooldown || giantSkel_SlashAtk.inAtk || giantSkel_SlashAtk.inCooldown) {
            return false;
        }
        // When the target is within a certain distance for a certain amount of time, trigger ground attack.
        if (Vector2.Distance(eRefs.PlayerCenterPos, attackDirPoint.position) < triggerRadius) {
            triggerValue += Time.deltaTime;
        }
        if (triggerValue > triggerThreshold) {
            return true;
        }
        return false;
    }

    public void StartGroundAttack()
    {
        inAtk = true;
        eRefs.mySpriteAnim.Play(eRefs.animClips[3]);
        triggerValue = 0f;
    }

    IEnumerator GroundAttackCooldown() {
        inAtk = false;
        inCooldown = true;
        yield return new WaitForSeconds(eRefs.eSO.attackCooldown);
        inCooldown = false;
    }

    public void StopGroundAttack() {
        if (eRefs.mySpriteAnim.Clip == eRefs.animClips[3]) {
            eRefs.mySpriteAnim.Stop();
        }
        StartCoroutine(GroundAttackCooldown());
    }

    public void AnimSpawnGroundSpikes() {
        StartCoroutine(SpawnGroundSpikes());
    }

    IEnumerator SpawnGroundSpikes() {
        // Decide on first spike's spawn position, for now in direction of player, at a certain distance from self.
        float timer = 0f;
        int curSpike = 1;
        normDirToPlayer= (eRefs.playerShadow.position - groundAttackOrigin.position).normalized;
        float firstSpikeDist = ((Vector2)groundAttackOrigin.position-(Vector2)eRefs.PlayerShadowPos).magnitude;
        if (firstSpikeDist > firstSpikeMaxDist) {
            firstSpikeDist = firstSpikeMaxDist;
        }
        Vector2 spikePos = (Vector2)groundAttackOrigin.position+(normDirToPlayer*(firstSpikeDist));
        // 
        projPool.RequestProjectile().LaunchProjectile(projValues, Vector2.up, spikePos, normDirToPlayer.x);
        while (curSpike < spikeAmount) {
            timer += Time.deltaTime;
            if (timer >= delayBetweenSpikes) {
                curSpike++;
                timer = 0f;
                spikePos = (Vector2)groundAttackOrigin.position+(normDirToPlayer*(firstSpikeDist+(spikeSpawnDist*(curSpike-1))));
                if (curSpike == spikeAmount) { // Spawn the last spike (different one).
                    projPool.RequestProjectile().LaunchProjectile(lastProjValues, Vector2.up, spikePos, normDirToPlayer.x);
                }
                else {
                    projPool.RequestProjectile().LaunchProjectile(projValues, Vector2.up, spikePos, normDirToPlayer.x);
                }
            }
            yield return null;
        }
    }
    public void AnimStartGroundAttackCooldown() {
        eRefs.mySpriteAnim.Stop();
        StartCoroutine(GroundAttackCooldown());
    }

    // NEED TO DO:
    // Have the spike slightly curve towards the player by adjusting their direction in the towards the player.*
    // Flip the projectiles based on the direction they go in.
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    public Enemy_Refs eRefs;
    //public Enemy_ColorFlash eColorFlash;
    public HealthBar healthBar;
    public float maxHealth;
    public float curHealth;
    public float damageModifier = 1f;
    public float maxPoise;
    public float curPoise;
    [Header("Hit Reaction")]
    public float hitTotalDuration;
    public Sprite[] hitSprites;
    public float[] hitTimings;
    bool inHitReaction, alreadyDead;
    Coroutine hitReactionCoroutine;
    [Header("Read Only")]
    public bool canBeStunned = true;
    //public GameObject[] objectsToDisable;

    private void Start() {
        // Relagate this to a startup function.
        maxHealth = eRefs.eSO.maximumLife;
        curHealth = maxHealth;
        maxPoise = eRefs.eSO.maximumPoise;
        curPoise = maxPoise;
    }
    
    public void ReceiveDamage(float healthDamage, float hitPoiseDamage, Vector2 hittingColliderPos, Vector2 receivingColliderPos) {
        if (!alreadyDead) {
            curHealth -= healthDamage * damageModifier;
            eRefs.spriteColorFlash.PlayColorFlash(eRefs.eSpriteR);
            healthBar.AdjustHealthBar(maxHealth, curHealth);
            Vector2 hitDir = ((Vector2)receivingColliderPos - hittingColliderPos).normalized;
            // Apply poise damage from the hit to the enemy which can lead to the enemy being stunned.
            PoiseDamage(hitPoiseDamage, hitDir);
            // If I die from this hit and im currently in a hit reaction, stop the hit reaction and die.
            if (curHealth <= 0f) {
                if (eRefs.mySpriteAnim != null) {
                    eRefs.mySpriteAnim.Stop();
                }
                alreadyDead = true;
                if (inHitReaction) {
                    StopCoroutine(hitReactionCoroutine);
                }
                eRefs.eDeath.DeathSequence(hitDir);
                return;
            }
        }
    }

    public void PoiseDamage(float hitPoiseDamage, Vector2 hitDir) {
        curPoise -= hitPoiseDamage;
        // Apply a 'stun' meaning, stop walking, interrupt attacks and play a hit reaction animation.
        if (curPoise <= 0f && canBeStunned) {
            Stunned();
            // If you get hit while already in a hit reaction, stop the current one and start the new one.
            if (inHitReaction) {
                StopCoroutine(hitReactionCoroutine);
                hitReactionCoroutine = StartCoroutine(HitReaction(hitDir));
            }
            else {
                hitReactionCoroutine = StartCoroutine(HitReaction(hitDir));
            }
            curPoise = maxPoise;
        }
    }

    public void Stunned () {
        // To stop all path movement.
        eRefs.eFollowPath.StopAllMovementCoroutines();
        // To stop enemy specific actions ie shoot projectile, shield bash.
        eRefs.eAction.StopActions();
        // Pause state updating.
        eRefs.eAction.StopStateUpdates();
        // to resume movement - eRefs.eFollowPath.allowPathUpdate = true;
    }
    
    IEnumerator HitReaction(Vector2 hitDir) {
        inHitReaction = true;
        // Arbitrary list of objects to disable during hit reaction, Ex: bow for Ranged Skeletons
        //foreach(GameObject obj in objectsToDisable) { obj.SetActive(false); }
        float timer = 0f;
        int thisSpriteIndex = 0;
        while (timer < hitTotalDuration) {
            timer += Time.deltaTime;
        // Animation
            if (thisSpriteIndex < hitSprites.Length && timer > hitTimings[thisSpriteIndex]) {
                eRefs.eSpriteR.sprite = hitSprites[thisSpriteIndex];
                thisSpriteIndex++;
            }
        // Movement
            yield return null;
        }
        //foreach(GameObject obj in objectsToDisable) { obj.SetActive(true); }
        // Am I dead or do I resume my states.
        if (curHealth <= 0f) {
            eRefs.eDeath.DeathSequence(hitDir);
        }
        else {
            eRefs.eAction.ResumeStateUpdates();
        }
        inHitReaction = false;
        hitReactionCoroutine = null;
        yield return null;
    }
}

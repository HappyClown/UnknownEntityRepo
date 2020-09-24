using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    public Enemy_Refs eRefs;
    public HealthBar healthBar;
    public float maxHealth;
    public float curHealth;
    public float damageModifier = 1f;
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
    }
    
    public void ReceiveDamage(float damage) {
        if (!alreadyDead) {
            curHealth -= damage * damageModifier;
            healthBar.AdjustHealthBar(maxHealth, curHealth);
            // Apply a 'stun' meaning, stop walking, stop attacking.
            if (canBeStunned) {
                Stunned();
                // Get hit reaction. Sprite Animation and movement.
                if (inHitReaction) {
                    // If you get hit while already in a hit reaction, stop the current one and start the new one.
                    StopCoroutine(hitReactionCoroutine);
                    hitReactionCoroutine = StartCoroutine(HitReaction());
                }
                else {
                    hitReactionCoroutine = StartCoroutine(HitReaction());
                }
                // If your health is 0, flag it in order to avoid entering a new hit reaction.
                if (curHealth <= 0) {
                    alreadyDead = true;
                }
            }
            // If you cant be stunned and your health is 0, die immediately.
            else if (curHealth <= 0f) {
                eRefs.eDeath.DeathSequence();
            }
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
    
    IEnumerator HitReaction() {
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
            eRefs.eDeath.DeathSequence();
        }
        else {
            eRefs.eAction.ResumeStateUpdates();
        }
        inHitReaction = false;
        hitReactionCoroutine = null;
        yield return null;
    }
}

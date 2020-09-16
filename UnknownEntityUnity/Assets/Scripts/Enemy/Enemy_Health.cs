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

    private void Start() {
        // Relagate this to a startup function.
        maxHealth = eRefs.eSO.maximumLife;
        curHealth = maxHealth;
    }
    
    public void ReceiveDamage(float damage) {
        curHealth -= damage * damageModifier;
        healthBar.AdjustHealthBar(maxHealth, curHealth);
        // Apply a 'stun' meaning, stop walking, stop attacking.
        Stunned();
        // Get hit reaction. Sprite Animation and movement.
        StartCoroutine(HitReaction());
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

        }
        // Am I dead or do I resume my states.
        if (curHealth <= 0f) {
            eRefs.eDeath.DeathSequence();
        }
        else {
            eRefs.eAction.ResumeStateUpdates();
        }
        yield return null;
    }
}

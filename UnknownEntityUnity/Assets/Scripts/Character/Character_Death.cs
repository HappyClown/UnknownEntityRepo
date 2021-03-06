using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Death : MonoBehaviour
{
    public Character_EquippedWeapons charWeaps;
    public Character_Attack charAtk;
    public Character_Movement charMov;
    public DeathSequence deathSequence;
    public Collider2D hitCol;
    public SpriteRenderer weaponSpriteR;
    public GameObject shadow;
    [Header("Animation")]
    public AnimationClip ClipDeath;
    public Sprite[] deathAnimSprites;
    public float[] deathAnimTimings;
    public SpriteRenderer spriteR;
    float timer;
    int tick;
    float totalDuration;
    int totalTicks;

    public void CharacterDies() {
        // totalDuration = deathAnimTimings[deathAnimTimings.Length-1];
        // totalTicks = deathAnimTimings.Length-1;
        // tick = 0;
        // timer = 0f;
        // Stop all scripts that need to be stopped.
        // Turn off hit collider.
        hitCol.enabled = false;
        // Stop weapon motion.
        charAtk.StopPreviousMotion();
        // Turn off attack script.
        charAtk.enabled = false;
        // Turn off movement script.
        charMov.enabled = false;
        // Turn weapon off.
        weaponSpriteR.sprite = null;
        // Turn weapon swapping script off.
        charWeaps.enabled = false;
        // Turn shadow off.
        shadow.SetActive(false);
        // Stop checking for character flip.
        // Turn off mouse pointer?
        charMov.mySpriteAnim.Play(ClipDeath);
        deathSequence.StartCoroutine(deathSequence.DeathUI());
        Time.timeScale = 0.33f;
        //StartCoroutine(DeathAnimation());
    }

    public IEnumerator DeathAnimation() {
        deathSequence.StartCoroutine(deathSequence.DeathUI());
        Time.timeScale = 0.33f;
        // Stop all scripts that need to be stopped.
        while(tick < totalTicks) {
            timer += Time.deltaTime;
            if (timer > deathAnimTimings[tick]) {
                spriteR.sprite = deathAnimSprites[tick];
                tick++;
            }
            yield return null;
        }
        Time.timeScale = 1f;
    }
}

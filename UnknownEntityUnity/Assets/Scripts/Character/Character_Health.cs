using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Health : MonoBehaviour
{
    public float maximumHealth;
    public  float currentHealth;
    public HealthBar healthBar;
    // Not sure if this is gonna stay here hit sprites.
    public Sprite[] hitSprites;
    public float[] hitSpriteTimings;
    public SpriteRenderer spriteR;
    float timer;
    int spriteNumber;
    public float getHitDuration;
    public Character_Movement charMov;

    public IEnumerator TakeHitAnimation() {
        charMov.StopInputMove();
        timer = 0f;
        spriteNumber = 0;
        spriteR.sprite = hitSprites[0];
        spriteNumber++;
        while (timer < getHitDuration) {
            //print("Should be changing sprite to getting hit in the face sprite.");
            timer += Time.deltaTime/getHitDuration;
            if (spriteNumber < hitSprites.Length && timer > hitSpriteTimings[spriteNumber]) {
                spriteR.sprite = hitSprites[spriteNumber];
                spriteNumber++;
            }
            yield return null;
        }
        charMov.canInputMove = true;
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        healthBar.AdjustHealthBar(maximumHealth, currentHealth);
        StartCoroutine(TakeHitAnimation());
    }
}

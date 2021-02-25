using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_MotionFX : MonoBehaviour
{
    public bool inUse;
    public SpriteRenderer spriteR;   
    float timer;
    //
    Sprite[] sprites;
    float[] spriteTimings;
    int totalTicks;
    int tick;
    SO_MotionFX sO_MotionFX;

    public void StartMotionFX(SO_MotionFX sOMotionFX) {
        inUse = true;
        sO_MotionFX = sOMotionFX;
        sprites = sO_MotionFX.motionFXSprites;
        spriteTimings = sO_MotionFX.motionFXTimings;
        totalTicks = spriteTimings.Length-1;
        tick = 0;
        timer = 0f;
        this.gameObject.SetActive(true);
        StartCoroutine(PlayImpactFX());
    }

    public IEnumerator PlayImpactFX() {
        while (tick < totalTicks) {
            timer += Time.deltaTime;
            if (timer >= spriteTimings[tick]) {
                spriteR.sprite = sprites[tick];
                tick++;
            }
            yield return null;
        }
        spriteR.sprite = null;
        this.gameObject.SetActive(false);
        inUse = false;
    }
}
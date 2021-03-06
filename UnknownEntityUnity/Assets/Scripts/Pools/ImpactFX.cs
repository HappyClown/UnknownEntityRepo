using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactFX : MonoBehaviour
{
    public bool inUse;
    public SpriteRenderer spriteR;
    float timer;
    //
    Sprite[] sprites;
    float[] spriteTimings;
    int totalTicks;
    int tick;
    SO_ImpactFX sO_ImpactFX;

    public void StartImpactFX(SO_ImpactFX sOImpactFX) {
        inUse = true;
        sO_ImpactFX = sOImpactFX;
        sprites = sO_ImpactFX.impactFXSprites;
        spriteTimings = sO_ImpactFX.impactFXTimings;
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

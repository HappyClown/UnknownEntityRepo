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

    public IEnumerator PlayImpactFX(SO_ImpactFX sOImpactFX) {
        tick = 0;
        timer = 0f;
        sprites = sOImpactFX.impactFXSprites;
        spriteTimings = sOImpactFX.impactFXTimings;
        totalTicks = spriteTimings.Length-1;
        while (tick < totalTicks) {
            timer += Time.deltaTime;
            if (timer >= spriteTimings[tick]) {
                spriteR.sprite = sprites[tick];
                tick++;
            }
            yield return null;
        }
        spriteR.sprite = null;
    }
}

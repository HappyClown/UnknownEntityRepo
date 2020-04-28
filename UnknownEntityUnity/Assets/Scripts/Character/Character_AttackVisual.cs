using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AttackVisual : MonoBehaviour
{
    // Variables are created in the coroutine to be allow starting multiple coroutines simultaneously with different variable references.

    // public SO_AttackFX sO_AttackFX;
    // private SpriteRenderer atkSpriteR;
    // private Sprite[] atkSprites;
    // private float[] atkSpriteChangeTimes;
    // private float totalDuration;
    // private float timer = 0f;
    // private int thisSpriteIndex = 0;

    public IEnumerator AttackAnimation(SO_AttackFX sO_AttackFX, Character_AttackFX atkFX) {
        // References from the AttackFX object from the CharacterAttackFX Pool.
        atkFX.inUse = true;
        SpriteRenderer atkSpriteR = atkFX.spriteR;
        atkFX.gameObject.SetActive(true);
        // References from the SO_AttackFX.
        Sprite[] atkSprites = sO_AttackFX.sprites;
        float[] atkSpriteChangeTimes = sO_AttackFX.spriteChangeTiming;
        float totalDuration = sO_AttackFX.totalDuration;
        // Set other variables.
        float timer = 0f;
        int thisSpriteIndex = 0;
        
        while (timer < totalDuration) {
            timer += Time.deltaTime;
            if (timer >= atkSpriteChangeTimes[thisSpriteIndex]) {
                atkSpriteR.sprite = atkSprites[thisSpriteIndex];
                if (thisSpriteIndex < atkSpriteChangeTimes.Length -1) {
                    thisSpriteIndex++;
                }
            }
            yield return null;
        }
        atkSpriteR.sprite = null;
        atkFX.gameObject.SetActive(false);
        atkFX.inUse = false;
        yield return null;
    }
}
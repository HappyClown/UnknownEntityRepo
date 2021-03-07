﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AttackVisual : MonoBehaviour
{
    // Variables are created in the coroutine to allow starting multiple coroutines simultaneously with different variable references.
    public IEnumerator AttackAnimation(SO_AttackFX sO_AttackFX, Character_AttackFX atkFX) {
        // References from the AttackFX object from the CharacterAttackFX Pool.
        atkFX.inUse = true;
        if (sO_AttackFX.stopOnStun) {
            atkFX.stopOnStun = true;
        }
        SpriteRenderer atkSpriteR = atkFX.spriteR;
        atkFX.gameObject.SetActive(true);
        //print ("ATK FX OBJECT set active");
        atkSpriteR.sprite = null;
        // References from the SO_AttackFX.
        Sprite[] atkSprites = sO_AttackFX.sprites;
        float[] atkSpriteChangeTimes = sO_AttackFX.spriteChangeTiming;
        float totalDuration = sO_AttackFX.totalDuration;
        // Set other variables.
        float timer = 0f;
        int thisSpriteIndex = 0;
        // This was put it to avoid having the first sprite appear before the attack movement was set.
        yield return null;
        // Go through all the attack FX sprites based on their change timings.
        while (timer < totalDuration) {
            timer += Time.deltaTime;
            if (timer >= atkSpriteChangeTimes[thisSpriteIndex]) {
                atkSpriteR.sprite = atkSprites[thisSpriteIndex];
                if (thisSpriteIndex < atkSpriteChangeTimes.Length -1) {
                    thisSpriteIndex++;
                }
            }
            // During this time the player will not be able to voluntarily interrupt his attack, for example they will not be able to use their dash skill.
            if (timer > sO_AttackFX.cantInterrupStart && timer < sO_AttackFX.cantInterrupEnd) {
                atkFX.canInterrupt = false;
            }
            else {
                atkFX.canInterrupt = true;
            }
            // Alternative to not have any point uninterruptable during the attack, have a point durring the attack where it will cancel the attack fx (projectile or not), for example if the player dashes before the nimble spear attack 01's projectile is fully formed it will make it disappear. Basically a point in the attack where the attack is either cancelled or continues.
            // Cancel Point, before this point the attack FX (visual, movement, collision) will be cancelled by things like getting hit aka stunned.
            if (timer > sO_AttackFX.cancelPoint) {
                atkFX.involuntaryCancel = false;
            }
            yield return null;
        }
        while (atkFX.holdLastSprite) {
            yield return null;
        }
        atkSpriteR.sprite = null;
        atkFX.gameObject.SetActive(false);
        atkFX.stopOnStun = false;
        atkFX.involuntaryCancel = true;
        atkFX.inUse = false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AttackVisual : MonoBehaviour
{
    // Variables are created in the coroutine to allow starting multiple coroutines simultaneously with different variable references.
    public Character_Attack charAtk;
    public IEnumerator AttackAnimation(SO_AttackFX sO_AttackFX, Character_AttackFX atkFX, bool atkFollowPlayerOrientation = false) {
        // References from the AttackFX object from the CharacterAttackFX Pool.
        atkFX.inUse = true;
        atkFX.exitAttackAnimationFX = false;
        atkFX.loopAnimation = sO_AttackFX.loopAnimation;
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
        float fxXOrientation = atkFX.transform.localScale.x;
        float playerXOrientation = charAtk.playerSpriteTrans.localScale.x;
        bool flipCheck = false;
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
            if (atkFollowPlayerOrientation && timer >= atkSpriteChangeTimes[0] && !flipCheck) {
                flipCheck = true;
                if (charAtk.playerSpriteTrans.localScale.x != playerXOrientation) {
                    atkFX.transform.localScale = new Vector3(-fxXOrientation, 1, 1);
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
            // If my Character_AttackFX pool object is set to stopAttackNow, exit the while loop immediately.
            if (atkFX.exitAttackAnimationFX) {
                break;
            }
            // If my Character_AttackFX pool object is set to loop, restart the animation.
            if (timer >= totalDuration) {
                if (atkFX.loopAnimation) {
                    timer = 0f;
                    thisSpriteIndex = 0;
                }
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
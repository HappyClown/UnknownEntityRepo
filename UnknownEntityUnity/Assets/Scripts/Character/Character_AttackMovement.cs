﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AttackMovement : MonoBehaviour
{
    public Transform weapOrigTrans;
    // Weapon height.
    public Transform weapSpriteTrans;

    public IEnumerator AttackMovement(SO_AttackFX sO_AttackFX, Transform atkFXTrans)
    {
        // References from SO_AttackFX.
        float totalDuration = sO_AttackFX.totalDuration;
        float followDuration = sO_AttackFX.followDuration;
        float moveDistance = sO_AttackFX.moveDistance;
        float spawnDistance = sO_AttackFX.spawnDistance;
        float moveDelay = sO_AttackFX.moveDelay;
        AnimationCurve moveAnimCurve = sO_AttackFX.moveAnimCurve;
        // Setup variables.
        Vector3 startPos = Vector3.zero;
        Vector3 targetPos = Vector3.zero;
        Vector3 xyweapOrig = Vector3.zero;
        //
        // Get movement values before or after delay.
        if (sO_AttackFX.setupBeforeDelay || moveDelay <= 0f) {
            // Get the correct orientation for the cur chain attack.
            atkFXTrans.rotation = weapOrigTrans.rotation;
            // Get the weapon's XY position.
            xyweapOrig = new Vector3(weapOrigTrans.position.x, weapOrigTrans.position.y, atkFXTrans.position.z);
            // Put the attack at the correct spawn point, after the move delay.
            atkFXTrans.position = xyweapOrig + (weapOrigTrans.up * spawnDistance);
            //print ("ATK FX has been MOVED to its initital position.");
        }
        if (moveDelay > 0f) {
            yield return new WaitForSeconds(moveDelay);
        }
        if (!sO_AttackFX.setupBeforeDelay) {
            // Get the correct orientation for the cur chain attack.
            atkFXTrans.rotation = weapOrigTrans.rotation;
            // Get the weapon's XY position.
            xyweapOrig = new Vector3(weapOrigTrans.position.x, weapOrigTrans.position.y, atkFXTrans.position.z);
            // Put the attack at the correct spawn point, after the move delay.
            atkFXTrans.position = xyweapOrig + (weapOrigTrans.up * spawnDistance);
            //print ("ATK FX has been MOVED to its initital position.");
        }

        float timer = 0f;
        float moveDuration = totalDuration - moveDelay;

        if (sO_AttackFX.followWeapon) {
            while (timer < followDuration) {
                timer += Time.deltaTime;
                atkFXTrans.position = weapSpriteTrans.position + (weapOrigTrans.up * sO_AttackFX.followWeaponHeight);
                atkFXTrans.rotation = weapOrigTrans.rotation;
                
                if (timer >= totalDuration) {
                    if (sO_AttackFX.loopAnimation) {
                        timer = 0f;
                    }
                }
                yield return null;
            }
        }
        // Move the attack a certain distance over attack length period using a lerp.
        if (followDuration < totalDuration) {
            // Set Lerp values.
            startPos = atkFXTrans.position;
            targetPos = atkFXTrans.position + (weapOrigTrans.up * moveDistance);
            while (timer < 1f) {
                timer += Time.deltaTime / moveDuration;
                atkFXTrans.position = Vector3.Lerp(startPos, targetPos, moveAnimCurve.Evaluate(timer));
                yield return null;
            }
        }
        // Reset its position to 999,999 in order to avoid having the collider spawn for a frame at its last used postion, interacting where it should not.
        // atkFXTrans.position = new Vector3(999, 999, atkFXTrans.position.z);
    }
}

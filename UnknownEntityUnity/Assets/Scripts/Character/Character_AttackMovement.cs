using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AttackMovement : MonoBehaviour
{
    public Transform weapOrigTrans;

    public IEnumerator AttackMovement(SO_AttackFX sO_AttackFX, Transform atkFXTrans)
    {
        // References from SO_AttackFX.
        float totalDuration = sO_AttackFX.totalDuration;
        float moveDistance = sO_AttackFX.moveDistance;
        float spawnDistance = sO_AttackFX.spawnDistance;
        float moveDelay = sO_AttackFX.moveDelay;
        AnimationCurve moveAnimCurve = sO_AttackFX.moveAnimCurve;
        // If there is no moveDelay, get the position and rotation on click.
        if (moveDelay <= 0) {
            // Get the correct orientation for the cur chain attack.
            atkFXTrans.rotation = weapOrigTrans.rotation;
            // Get the weapon's XY position.
            Vector3 xyweapOrig = new Vector3(weapOrigTrans.position.x, weapOrigTrans.position.y, atkFXTrans.position.z);
            // Put the attack at the correct spawn point.
            atkFXTrans.position = xyweapOrig + (weapOrigTrans.up * spawnDistance);
        }
        // Set the initial move delay, get the position and rotation right after the delay.
        else {
            yield return new WaitForSeconds(moveDelay);
            // Get the correct orientation for the cur chain attack.
            atkFXTrans.rotation = weapOrigTrans.rotation;
            // Get the weapon's XY position.
            Vector3 xyweapOrig = new Vector3(weapOrigTrans.position.x, weapOrigTrans.position.y, atkFXTrans.position.z);
            // Put the attack at the correct spawn point, after the move delay.
            atkFXTrans.position = xyweapOrig + (weapOrigTrans.up * spawnDistance);
        }
        // Set Lerp values.
        float timer = 0f;
        float moveDuration = totalDuration - moveDelay;
        Vector3 startPos = atkFXTrans.position;
        Vector3 targetPos = atkFXTrans.position + (weapOrigTrans.up * moveDistance);

        // Move the attack a certain distance over attack length period using a lerp.
        while (timer < 1) {
            timer += Time.deltaTime / moveDuration;
            atkFXTrans.position = Vector3.Lerp(startPos, targetPos, moveAnimCurve.Evaluate(timer));
            yield return null;
        }
    }
}

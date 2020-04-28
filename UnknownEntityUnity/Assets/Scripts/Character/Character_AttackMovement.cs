using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AttackMovement : MonoBehaviour
{
    public Transform weapOrigTrans;
    //public Transform playerTrans;

    public IEnumerator AttackMovement(SO_AttackFX sO_AttackFX, /* int chainNum, float spawnDistance, float moveDistance, float moveDelay, float attackLength, AnimationCurve moveCurve, */ Transform atkFXTrans)
    {
        // References from SO_AttackFX.
        float totalDuration = sO_AttackFX.totalDuration;
        float moveDistance = sO_AttackFX.moveDistance;
        float spawnDistance = sO_AttackFX.spawnDistance;
        float moveDelay = sO_AttackFX.moveDelay;
        AnimationCurve moveAnimCurve = sO_AttackFX.moveAnimCurve;
        // Set weapon variables.
        Vector3 xyweapOrig = new Vector3(weapOrigTrans.position.x, weapOrigTrans.position.y, atkFXTrans.position.z);
        // Get the correct orientation for the cur chain attack, 
        atkFXTrans.rotation = weapOrigTrans.rotation;
        // Put the attack at the correct spawn point,
        atkFXTrans.position = xyweapOrig + (weapOrigTrans.up * spawnDistance);
        //atkFXTrans.Translate(Vector3.up * spawnDistance);
        //atkFXTrans.position = xyweapOrig;
        //atkFXTrans.Translate(Vector3.up * moveDistance);
        // Set Lerp values.
        Vector3 startPos = atkFXTrans.position;
        Vector3 targetPos = atkFXTrans.position + (weapOrigTrans.up * moveDistance);
        float timer = 0f;
        float moveDuration = totalDuration - moveDelay;

        // Set the initial move delay.
        yield return new WaitForSeconds(moveDelay);

        // Move the attack a certain distance over attack length period using a lerp.
        while (timer < 1) {
            timer += Time.deltaTime / moveDuration;
            atkFXTrans.position = Vector3.Lerp(startPos, targetPos, moveAnimCurve.Evaluate(timer));
            yield return null;
        }
        yield return null;
    }
}

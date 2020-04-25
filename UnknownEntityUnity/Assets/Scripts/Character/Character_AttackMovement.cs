using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AttackMovement : MonoBehaviour
{
    public Transform weapOrigTrans;
    public Transform playerTrans;

    public IEnumerator AttackMovement(int chainNum, float spawnDistance, float moveDistance, float moveDelay, float attackLength, AnimationCurve moveCurve, Transform atkFXTrans)
    {
        float timer = 0f;
        //float speed = moveDistance / attackLength;
        //Vector3 xyPlayerTrans = new Vector3(playerTrans.position.x, playerTrans.position.y, atkFXTrans.position.z);
        Vector3 xyweapOrig = new Vector3(weapOrigTrans.position.x, weapOrigTrans.position.y, atkFXTrans.position.z);
    // Get the correct orientation for the cur chain attack, 
        atkFXTrans.rotation = weapOrigTrans.rotation;
    // Put the attack at the correct spawn point,
        atkFXTrans.position = xyweapOrig;
        atkFXTrans.Translate(Vector3.up * moveDistance);
        Vector3 targetPos = atkFXTrans.position; // or xyPlayerTrans + Vector3.up*movedistance?

        atkFXTrans.position = xyweapOrig;
        atkFXTrans.Translate(Vector3.up * spawnDistance);
        Vector3 startPos = atkFXTrans.position;
    // Move the attack a certain distance over attack length period using a lerp.
        while (timer < 1) {
            timer += Time.deltaTime / attackLength;
            atkFXTrans.position = Vector3.Lerp(startPos, targetPos, moveCurve.Evaluate(timer));
            yield return null;
        }
        yield return null;
    }
}

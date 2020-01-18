using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AttackMovement : MonoBehaviour
{
    public Transform weapOrigTrans;
    public Transform playerTrans;
    public Transform[] attackTranfs;

    void Start()
    {
        
    }

    public IEnumerator AttackMovement(int chainNum, float spawnDistance, float moveDistance, float moveDelay, float attackLength, AnimationCurve moveCurve)
    {
        float timer = 0f;
        float speed = moveDistance / attackLength;
        Vector3 xyPlayerTrans = new Vector3(playerTrans.position.x, playerTrans.position.y, attackTranfs[chainNum].position.z);
    // Get the correct orientation for the cur chain attack, 
        attackTranfs[chainNum].rotation = weapOrigTrans.rotation;
    // Put the attack at the correct spawn point,
        attackTranfs[chainNum].position = xyPlayerTrans;
        attackTranfs[chainNum].Translate(Vector3.up * moveDistance);
        Vector3 targetPos = attackTranfs[chainNum].position;

        attackTranfs[chainNum].position = xyPlayerTrans;
        attackTranfs[chainNum].Translate(Vector3.up * spawnDistance);
        Vector3 startPos = attackTranfs[chainNum].position;
    // Move the attack a certain distance over attack length period(speed), (anim curve?)
        // Using MoveTowards. 
        // float step = 0f;
        // while (timer < 1f) {
        //     timer += Time.deltaTime / attackLength;
        //     step = speed * Time.deltaTime;
        //     attackTranfs[chainNum].position = Vector3.MoveTowards(attackTranfs[chainNum].position, targetPos, step);
        //     yield return null;
        // }
    // Using Lerp + .
        while (timer < 1) {
            timer += Time.deltaTime / attackLength;
            attackTranfs[chainNum].position = Vector3.Lerp(startPos, targetPos, moveCurve.Evaluate(timer));
            yield return null;
        }
        yield return null;
    }
}

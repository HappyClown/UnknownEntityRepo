using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AttackPlayerMovement : MonoBehaviour
{
    public Character_Movement charMov;
    public Transform playerTrans;
    public Transform weapOrigTrans;

    public void AttackPlayerMovement(float slowDownValue, float slowDownDur, float atkLength, float moveDistance, int chainNum) {
        // Towards the attack direction.
        Vector3 newPosition = playerTrans.position+weapOrigTrans.up*moveDistance;
        Vector3 normMove = (newPosition - playerTrans.position).normalized;
        charMov.MoveThePlayer(normMove, newPosition, playerTrans.position);
        // Slow run speed during attack length.
        StartCoroutine(MoveSlowDown(slowDownValue, slowDownDur));
    }

    IEnumerator MoveSlowDown(float _slowDownValue, float _slowDownDur) {
        charMov.runSpeed *= _slowDownValue;
        yield return new WaitForSeconds(_slowDownDur);
        charMov.runSpeed /= _slowDownValue;
        yield return null;
    }
}

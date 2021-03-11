using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherButtonActions : MonoBehaviour
{
    [Header("Movement Skill")]
    public Character_MovementSkills movementSkill; // change for a generic script that can
    public float moveSkillGraceDuration;
    Coroutine moveGraceCoroutine;
    [Header("Interact")]
    public bool interactPressed;

    public void MoveSkillButtonPressedChecks() {
        if (movementSkill.CanIUseMovementSkill()) {
            // Movement skill is start in its own script if it is true.
            // Cancel grace period for movement skills.
            if (moveGraceCoroutine != null) StopCoroutine(moveGraceCoroutine);
        }
        // Start a grace period coroutine.
        if (moveGraceCoroutine != null) StopCoroutine(moveGraceCoroutine);
        moveGraceCoroutine = StartCoroutine(MoveSkillInGrace());
        
    }

    IEnumerator MoveSkillInGrace() {
        float timer = 0f;
        while (timer < moveSkillGraceDuration) {
            timer += Time.deltaTime;
            if (movementSkill.CanIUseMovementSkill()) {
                timer = moveSkillGraceDuration;
            }
            yield return null;
        }
        moveGraceCoroutine = null;
    }
}

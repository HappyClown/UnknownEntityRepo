﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_MovementSkill_Dash : MonoBehaviour
{
    public MouseInputs moIn;
    public Character_Movement charMove;
    //public Character_MovementSkills chosenMoveSkill;

    public bool dashing;
    
    Vector3 dashDirection;
    float dashSpeed;
    public float dashDuration;
    public float dashDistance;

    float timer;
    float multiplierDuration;
    Vector3 newPos, curPos;

    void Update() {
        if (moIn.movementSkillPressed) {
            //print("Hello.");
            StartMovementSkill();
        }

        if (dashing) {
            timer += Time.deltaTime * multiplierDuration;
            curPos = this.transform.position;
            newPos = curPos + (dashDirection*dashSpeed*Time.deltaTime);
            charMove.MoveThePlayer(dashDirection, newPos, curPos);
            if (timer >= 1f) {
                dashing = false;
                charMove.canInputMove = true;
            }
        }
    }

    public void StartMovementSkill() {
        dashDirection = charMove.normalizedMovement;
        print(dashDirection);
        dashSpeed = dashDistance/dashDuration;
        // This is just to avoid dividing every update. Example: Time.deltaTime/dashDuration -> Time.deltaTime*multiplierDuration. 
        multiplierDuration = 1 / dashDuration;

        timer = 0f;
        charMove.canInputMove = false;
        dashing = true;
    }

    public void StopMovementSkill() {
        dashing = false;
        timer = 0f;
        charMove.canInputMove = true;
    }
}

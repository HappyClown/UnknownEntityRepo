using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AttackPlayerMovement : MonoBehaviour
{
    public Character_Movement charMov;
    public WeaponLookAt weaponLookAt;
    public Transform weapOrigTrans;
    public Transform playerTrans;
    // Arrays from SO
    private float[] durations;
    private float[] distances;
    private bool[] lockInputMovement;
    private bool[] lockCharacterAndWeapon;
    private float[] slowDownRunSpeed;
    // Current motion variables.
    private int curMotion;
    private float curDistance;
    private Vector2 curDirection;
    private float curDuration;
    private float curSpeed;
    private float curSlow;
    private Vector2 curPosition, newPosition;
    // General
    public bool charAtkMotionOn;
    private float moveTimer;

    void Update() {
        if (charAtkMotionOn) {
            moveTimer += Time.deltaTime/curDuration;
            if (moveTimer >= 1f) {
                SetupNextMotion();
            }
            curPosition = this.transform.position;
            newPosition = curPosition + (curDirection*curSpeed*Time.deltaTime);
            charMov.MoveThePlayer(curDirection, newPosition, curPosition);
        }
    }

    void SetupNextMotion() {
        curMotion++;
        if (curMotion == durations.Length) {
            charMov.canInputMove = true;
            charMov.charCanFlip = true;
            charMov.ReduceSpeed(-curSlow);
            curSlow = 0f;
            charMov.FlipSpriteMouseBased();
            weaponLookAt.lookAtEnabled = true;
            curMotion = 0;
            charAtkMotionOn = false;
            moveTimer = 0f;
            return;
        }
        // If there was no movement for the previous motion (distance set to "0"), set the movement direction now.
        if (curDistance == 0) { curDirection = weapOrigTrans.up; }
        // Undo the previous movement slow down.
        charMov.ReduceSpeed(-curSlow);
        // Set the next motions current values.
        curDistance = distances[curMotion];
        curDuration = durations[curMotion];
        curSpeed = Mathf.Abs(curDistance/curDuration);
        curPosition = this.transform.position;
        curSlow = slowDownRunSpeed[curMotion];
        // Set initial locks and slowdown.
        if (lockInputMovement[curMotion]) { charMov.StopInputMove(); }
        else { charMov.canInputMove = true; }
        if (lockCharacterAndWeapon[curMotion]) { charMov.charCanFlip = false; weaponLookAt.lookAtEnabled = false; }
        else { charMov.charCanFlip = true; weaponLookAt.lookAtEnabled = true; }
        // Apply the new slowdown.
        charMov.ReduceSpeed(curSlow);
        moveTimer = 0f;
    }

    public void SetupPlayerAttackMotions(SO_CharAtk_Motion sOCharAtkMotion) {
        if (curSlow != 0f) { charMov.ReduceSpeed(-curSlow); }
        // Get references from the Scriptable Object.
        durations = sOCharAtkMotion.durations;
        distances = sOCharAtkMotion.distances;
        lockInputMovement = sOCharAtkMotion.lockInputMovement;
        lockCharacterAndWeapon = sOCharAtkMotion.lockCharacterAndWeapon;
        slowDownRunSpeed = sOCharAtkMotion.slowDownRunSpeed;
        // Assign the current values for the first motion.
        curMotion = 0;
        curDistance = distances[curMotion];
        curDuration = durations[curMotion];
        curDirection = weapOrigTrans.up;
        curSpeed = Mathf.Abs(curDistance/curDuration);
        curPosition = weapOrigTrans.position;
        curSlow = slowDownRunSpeed[curMotion];
        // Set initial locks and slowdown.
        // Locks the player movement input.
        if (lockInputMovement[curMotion]) { charMov.StopInputMove(); }
        else { charMov.canInputMove = true; }
        // This locks the character sprite flip and weapon rotation. (Can be seperated)
        if (lockCharacterAndWeapon[curMotion]) { charMov.charCanFlip = false; weaponLookAt.lookAtEnabled = false; }
        else { charMov.charCanFlip = true; weaponLookAt.lookAtEnabled = true; }
        // Applies a slow to the player input movement speed.
        charMov.ReduceSpeed(curSlow);

        moveTimer = 0f;
        charAtkMotionOn = true;
    }

    IEnumerator InputMoveSlowDown(float speedReduceOnOne, float slowDuration) {
        charMov.ReduceSpeed(speedReduceOnOne);
        yield return new WaitForSeconds(slowDuration);
        charMov.ReduceSpeed(-speedReduceOnOne);
        yield return null;
    }

    public void StopPlayerMotion() {
        if (charAtkMotionOn) {
            charMov.ReduceSpeed(-curSlow);
            curSlow = 0f;
            charAtkMotionOn = false;
        }
    }
}
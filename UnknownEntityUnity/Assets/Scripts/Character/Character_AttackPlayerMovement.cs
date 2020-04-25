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
            charMov.FlipSprite();
            weaponLookAt.lookAtEnabled = true; 
            curMotion = 0;
            charAtkMotionOn = false;
            moveTimer = 0f;
            return;
        }
        // Set the next motions current values.
        curDistance = distances[curMotion];
        curDuration = durations[curMotion];
        curSpeed = Mathf.Abs(curDistance/curDuration);
        curPosition = this.transform.position;
        // Set initial locks and slowdown.
        if (lockInputMovement[curMotion]) { charMov.StopInputMove(); }
        else { charMov.canInputMove = true; }
        if (lockCharacterAndWeapon[curMotion]) { charMov.charCanFlip = false; weaponLookAt.lookAtEnabled = false; }
        else { charMov.charCanFlip = true; weaponLookAt.lookAtEnabled = true; }
        //
        moveTimer = 0f;
    }

    public void SetupPlayerAttackMotions(SO_CharAtk_Motion sOCharAtkMotion) {
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
        // Set initial locks and slowdown.
        if (lockInputMovement[curMotion]) { charMov.StopInputMove(); }
        else { charMov.canInputMove = true; }
        if (lockCharacterAndWeapon[curMotion]) { charMov.charCanFlip = false; weaponLookAt.lookAtEnabled = false; }
        else { charMov.charCanFlip = true; weaponLookAt.lookAtEnabled = true; }
        //
        moveTimer = 0f;
        charAtkMotionOn = true;
    }

    IEnumerator InputMoveSlowDown(float speedReduceOnOne, float slowDuration) {
        charMov.ReduceSpeed(speedReduceOnOne);
        yield return new WaitForSeconds(slowDuration);
        charMov.ReduceSpeed(-speedReduceOnOne);
        yield return null;
    }
}
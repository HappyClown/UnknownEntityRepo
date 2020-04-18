using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_SlashMotion : Weapon_Motion
{
    [Header("Script References")]
    public Character_Attack charAtk;
    [Header("To-set Variables")]
    public SO_Weapon_Motion_Slash sOWeaponMotionSlash;
    [Header("Read Only")]
    public bool resetWeapRot;
    public bool atkWeapRot;
    public Transform weaponTrans;
    public SpriteRenderer weaponSpriteR;
    private AnimationCurve atkRotAnimCurve;
    private float weaponRotDur;
    private float waitingForResetAngle, restingAngle;
    private float startAngle, curZAngle, rotTimer, endAngle;
    private bool clockwise = true;

    void Update() {
        // Rotate charAtk.weapon back to its reset position.
        if (resetWeapRot) {
            rotTimer += Time.deltaTime / sOWeaponMotionSlash.resetRotDuration;
            curZAngle = Mathf.Lerp(startAngle, endAngle, rotTimer);
            if (rotTimer >= 1f) {
                resetWeapRot = false;
                rotTimer = 0f;
                curZAngle = endAngle;
                weaponSpriteR.color= Color.white;
            }
            weaponTrans.localRotation = Quaternion.Euler(weaponTrans.localEulerAngles.x, weaponTrans.localEulerAngles.y, curZAngle);
        }

        // Rotate charAtk.weapon to the other side simulating an attack.
        if (atkWeapRot) {
            rotTimer += Time.deltaTime / (weaponRotDur/2);
            curZAngle = Mathf.Lerp(startAngle, endAngle, atkRotAnimCurve.Evaluate(rotTimer));
            if (rotTimer >= 1f) {
                charAtk.readyToAtk = true;
                charAtk.atkChain.ready = true;
                atkWeapRot = false;
                rotTimer = 0f;
                curZAngle = endAngle;
                resetWeapRot = true;
                // Set up the angles for the reset charAtk.weapon rotation.
                if (clockwise) { 
                    startAngle = endAngle;
                    endAngle = -restingAngle;
                }
                else {
                    startAngle = endAngle;
                    endAngle = -restingAngle;
                }
                weaponSpriteR.color= Color.gray;
            }
            weaponTrans.localRotation = Quaternion.Euler(weaponTrans.localEulerAngles.x, weaponTrans.localEulerAngles.y, curZAngle);
        }
    }

    // Instantly rotate the charAtk.weapon to the other side.
    public override void WeaponMotionSetup(Character_Attack _charAtk, Transform _weaponTrans, SpriteRenderer _weaponSpriteR) {

        weaponTrans = _weaponTrans;
        weaponSpriteR = _weaponSpriteR;
        charAtk = _charAtk;
        sOWeaponMotionSlash = charAtk.weapon.attackChains[charAtk.atkChain.curChain].sO_Weapon_Motion as SO_Weapon_Motion_Slash;
        atkRotAnimCurve = sOWeaponMotionSlash.attackRotAnimCurve;
        weaponRotDur = sOWeaponMotionSlash.weaponMotionDuration;

        if (clockwise) {
            restingAngle = sOWeaponMotionSlash.restingAngle;
            waitingForResetAngle = -sOWeaponMotionSlash.waitingForResetAngle;
            clockwise = false;
        }
        else {
            restingAngle = -sOWeaponMotionSlash.restingAngle;
            waitingForResetAngle = sOWeaponMotionSlash.waitingForResetAngle;
            clockwise = true;
        }
        resetWeapRot = false;
        rotTimer = 0f;

        weaponSpriteR.color= Color.white;
        startAngle = restingAngle;
        endAngle = waitingForResetAngle;

        atkWeapRot = true;
    }

    //Stop rotations, used for weapon swapping, ...interrupts like stuns?
    public override void StopMotions() {
        resetWeapRot = false;
        atkWeapRot = false;
        rotTimer = 0f;
    }
}

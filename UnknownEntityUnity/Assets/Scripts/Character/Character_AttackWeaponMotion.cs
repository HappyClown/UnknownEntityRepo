using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AttackWeaponMotion : MonoBehaviour
{
    [Header("Script References")]
    public Character_AttackChain atkChain;
    public Character_Attack charAtk;
    //[Header("To set Values")]
    [Header("Read Only")]
    public bool resetWeapRot;
    public Transform weaponTran;
    public SpriteRenderer weaponSpriteR;
    //private bool clockwise = true;
    private float waitingForResetAngle, restingAngle;
    private float startAngle, curZAngle, rotTimer, endAngle;
    private bool atkWeapRot, passByZero;
    private float weapRotDur;
    private AnimationCurve atkRotAnimCurve;

    // void Update() {
    //     // Rotate charAtk.weapon back to its reset position.
    //     if (resetWeapRot) {
    //         rotTimer += Time.deltaTime / charAtk.weapon.resetRotDuration;
    //         curZAngle = Mathf.Lerp(startAngle, endAngle, rotTimer);
    //         if (rotTimer >= 1f) {
    //             resetWeapRot = false;
    //             rotTimer = 0f;
    //             curZAngle = endAngle;
    //             weaponSpriteR.color= Color.white;
    //         }
    //         weaponTran.localRotation = Quaternion.Euler(weaponTran.localEulerAngles.x, weaponTran.localEulerAngles.y, curZAngle);
    //     }

    //     // Rotate charAtk.weapon to the other side simulating an attack.
    //     if (atkWeapRot) {
    //         rotTimer += Time.deltaTime / (weapRotDur/2);
    //         curZAngle = Mathf.Lerp(startAngle, endAngle, atkRotAnimCurve.Evaluate(rotTimer));
    //         if (rotTimer >= 1f) {
    //             charAtk.readyToAtk = true;
    //             atkChain.ready = true;
    //             atkWeapRot = false;
    //             rotTimer = 0f;
    //             curZAngle = endAngle;
    //             // Set up the angles for the reset charAtk.weapon rotation.
    //             if (clockwise) { 
    //                 startAngle = endAngle;
    //                 endAngle = -restingAngle;
    //             }
    //             else {
    //                 startAngle = endAngle;
    //                 endAngle = -restingAngle;
    //             }
    //             weaponSpriteR.color= Color.gray;
    //         }
    //         weaponTran.localRotation = Quaternion.Euler(weaponTran.localEulerAngles.x, weaponTran.localEulerAngles.y, curZAngle);
    //     }
    // }
    // // Instantly rotate the charAtk.weapon to the other side.
    // public void WeaponMotion(float _weapRotDur) {
    //     if (clockwise) {
    //         restingAngle = charAtk.weapon.restingAngle;
    //         waitingForResetAngle = -charAtk.weapon.waitingForResetAngle;
    //         clockwise = false;
    //     }
    //     else {
    //         restingAngle = -charAtk.weapon.restingAngle;
    //         waitingForResetAngle = charAtk.weapon.waitingForResetAngle;
    //         clockwise = true;
    //     }
    //     resetWeapRot = false;
    //     rotTimer = 0f;
    //     weapRotDur = _weapRotDur;
    //     atkRotAnimCurve = charAtk.weapon.attackRotAnimCurve;
    //     weaponSpriteR.color= Color.white;

    //     startAngle = restingAngle;
    //     endAngle = waitingForResetAngle;

    //     //passByZero = true;
    //     atkWeapRot = true;
    // }

    // //Stop rotations, used for weapon swapping, ...interrupts like stuns?
    // public void StopMotions() {
    //     resetWeapRot = false;
    //     atkWeapRot = false;
        
    // }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon_Motion", menuName = "UnknownEntityUnity/SO_Weapon_SlashFunctionsTest", order = 0)]
public class SO_Weapon_SlashFunctionsTest : SO_Weapon_MotionFunctionsTest
{
    [Header("Weapon Motion")]
    // If you want to alternate and control the rotation angle of the attack.
    public float restingAngle;
    public float waitingForResetAngle;
    public float resetRotDuration;
    public AnimationCurve attackRotAnimCurve;
    public float RotationDifference {
        get {
            return Mathf.Abs(restingAngle-waitingForResetAngle);
        }
    }
    //[Header("Script References")]
    //[Header("To set Values")]
    [Header("Read Only")]
    public bool resetWeapRot;
    public Transform weaponTran;
    public SpriteRenderer weaponSpriteR;
    //private bool clockwise = true;
    private float startAngle, curZAngle, rotTimer, endAngle;
    private AnimationCurve atkRotAnimCurve;

    public override void ResetWeaponRotation(Transform weapTrans) {
        // weaponTran = weapTrans;
        // // Rotate charAtk.weapon back to its reset position.
        // curZAngle = Mathf.Lerp(startAngle, endAngle, rotTimer);
        // if (rotTimer >= 1f) {
        //     resetWeapRot = false;
        //     rotTimer = 0f;
        //     curZAngle = endAngle;
        //     weaponSpriteR.color= Color.white;
        // }
        // weaponTran.localRotation = Quaternion.Euler(weaponTran.localEulerAngles.x, weaponTran.localEulerAngles.y, curZAngle);
    }

    public override void AttackWeaponMotion(float curTimer, Transform weapTrans) {
        // weaponTran = weapTrans;
        // // Rotate charAtk.weapon to the other side simulating an attack.
        // curZAngle = Mathf.Lerp(startAngle, endAngle, atkRotAnimCurve.Evaluate(rotTimer));
        // if (rotTimer >= 1f) {
        //     curZAngle = endAngle;
        //     // Set up the angles for the reset charAtk.weapon rotation.
        //     if (clockwise) {
        //         startAngle = endAngle;
        //         endAngle = -restingAngle;
        //     }
        //     else {
        //         startAngle = endAngle;
        //         endAngle = -restingAngle;
        //     }
        //     //weaponSpriteR.color= Color.gray;
        //     weaponTran.localRotation = Quaternion.Euler(weaponTran.localEulerAngles.x, weaponTran.localEulerAngles.y, curZAngle);
        // }
        // weaponTran.localRotation = Quaternion.Euler(weaponTran.localEulerAngles.x, weaponTran.localEulerAngles.y, curZAngle);
    }

    public override void WeaponMotionSetup(float motionDuration) {
        // if (clockwise) {
        //     //restingAngle = charAtk.weapon.restingAngle;
        //     waitingForResetAngle = -waitingForResetAngle;
        //     clockwise = false;
        // }
        // else {
        //     restingAngle = -restingAngle;
        //     //waitingForResetAngle = charAtk.weapon.waitingForResetAngle;
        //     clockwise = true;
        // }
        // //atkRotAnimCurve = charAtk.weapon.attackRotAnimCurve;
        // //weaponSpriteR.color= Color.white;

        // startAngle = restingAngle;
        // endAngle = waitingForResetAngle;
        // //weaponTran = weapTrans;
    }

    //Stop rotations, used for weapon swapping, ...interrupts like stuns?
    public override void StopMotions() {
        // weaponSpriteR.color= Color.white;
    }
}

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
    public bool resetWeapOn;
    public Transform weaponTrans;
    public SpriteRenderer weaponSpriteR;
    // References
    float[] rotations;
    float[] motionDurations;
    AnimationCurve[] animCurves;
    float restingRotation;
    // Current
    int curMotion;
    float curMotionDur;
    float startRot, endRot;
    float curRot;
    AnimationCurve curAnimCurve;
    float moveTimer;
    bool weapMotionOn;
    //

    void Update() {
        // Rotate charAtk.weapon back to its reset position.
        // if (resetWeapRot) {
        //     rotTimer += Time.deltaTime / sOWeaponMotionSlash.resetRotDuration;
        //     curZAngle = Mathf.Lerp(startAngle, endAngle, rotTimer);
        //     if (rotTimer >= 1f) {
        //         resetWeapRot = false;
        //         rotTimer = 0f;
        //         curZAngle = endAngle;
        //         weaponSpriteR.color= Color.white;
        //     }
        //     weaponTrans.localRotation = Quaternion.Euler(weaponTrans.localEulerAngles.x, weaponTrans.localEulerAngles.y, curZAngle);
        // }

        // // Rotate charAtk.weapon to the other side simulating an attack.
        // if (atkWeapRot) {
        //     rotTimer += Time.deltaTime / (weaponRotDur/2);
        //     curZAngle = Mathf.Lerp(startAngle, endAngle, atkRotAnimCurve.Evaluate(rotTimer));
        //     if (rotTimer >= 1f) {
        //         charAtk.readyToAtk = true;
        //         charAtk.atkChain.ready = true;
        //         atkWeapRot = false;
        //         rotTimer = 0f;
        //         curZAngle = endAngle;
        //         resetWeapRot = true;
        //         // Set up the angles for the reset charAtk.weapon rotation.
        //         if (clockwise) { 
        //             startAngle = endAngle;
        //             endAngle = -restingAngle;
        //         }
        //         else {
        //             startAngle = endAngle;
        //             endAngle = -restingAngle;
        //         }
        //         weaponSpriteR.color= Color.gray;
        //     }
        //     weaponTrans.localRotation = Quaternion.Euler(weaponTrans.localEulerAngles.x, weaponTrans.localEulerAngles.y, curZAngle);
        // }        
        // Rotate charAtk.weapon back to its reset position.
        if (resetWeapOn) {
            moveTimer += Time.deltaTime / sOWeaponMotionSlash.resetRotDuration;
            // Lerp.
            curRot = Mathf.Lerp(startRot, endRot, curAnimCurve.Evaluate(moveTimer));
            if (moveTimer >= 1f) {
                resetWeapOn = false;
                moveTimer = 0f;
                curRot = endRot;
            }
            weaponTrans.localRotation = Quaternion.Euler(weaponTrans.localEulerAngles.x, weaponTrans.localEulerAngles.y, curRot);
        }

        if (weapMotionOn) {
            moveTimer += Time.deltaTime / curMotionDur;
            curRot = Mathf.Lerp(startRot, endRot, curAnimCurve.Evaluate(moveTimer));
            if (moveTimer >= 1f) {
                SetupNextMotion();
            }
            weaponTrans.localRotation = Quaternion.Euler(weaponTrans.localEulerAngles.x, weaponTrans.localEulerAngles.y, curRot);
        }
    }

    void SetupNextMotion() {
        curMotion++;
        // If there are no more attack motions.
        if (curMotion == motionDurations.Length) {
            weapMotionOn = false;
            resetWeapOn = true;
            curRot = endRot;
            startRot = curRot;
            endRot = restingRotation*-1;
            moveTimer = 0f;
            charAtk.readyToAtk = true;
            charAtk.atkChain.ready = true;
            // Setup weapon reset here.
            return;
        }
        curMotionDur = motionDurations[curMotion];
        curRot = endRot;
        startRot = curRot;
        endRot = rotations[curMotion];
        curAnimCurve = animCurves[curMotion];
        moveTimer = 0f;
    }

    // Instantly rotate the charAtk.weapon to the other side.
    public override void WeaponMotionSetup(Character_Attack _charAtk, Transform _weaponTrans, SpriteRenderer _weaponSpriteR) {
        // References from the character.
        weaponTrans = _weaponTrans;
        weaponSpriteR = _weaponSpriteR;
        charAtk = _charAtk;
        // References from the motion SO associated with the current attack chain.
        sOWeaponMotionSlash = charAtk.weapon.attackChains[charAtk.atkChain.curChain].sO_Weapon_Motion as SO_Weapon_Motion_Slash;
        motionDurations = sOWeaponMotionSlash.motionDurations;
        // Clone makes the new array a "shallow" reference, meaning making changes to the new array wont change the original one, it just copies the values as opposed to being a reference to the array. 
        rotations = sOWeaponMotionSlash.rotations.Clone() as float[];
        animCurves = sOWeaponMotionSlash.animCurves;
        restingRotation = sOWeaponMotionSlash.restingRotation.z;
        curMotion = 0;
        // First motion setup.
        if (sOWeaponMotionSlash.useDirectionChange) {
            DirectionChange();
        }
        startRot = restingRotation;
        endRot = rotations[curMotion];

        curMotionDur = motionDurations[curMotion];
        curAnimCurve = animCurves[curMotion];
        moveTimer = 0f;
        // Enable motion.
        resetWeapOn = false;
        weapMotionOn = true;
        if (sOWeaponMotionSlash.useDirectionChange) { 
            sOWeaponMotionSlash.clockwise = !sOWeaponMotionSlash.clockwise;
        }
    }

    void DirectionChange() {
        sOWeaponMotionSlash.restingRotation.z *= -1;
        if (sOWeaponMotionSlash.clockwise) {
            for (int i = 0; i < rotations.Length; i++) {
                rotations[i] *= -1;
            }
        }
        // If its clockwise the sprite flipX should be false, because the sprite animations are usually created from left to right (clockwise).
        charAtk.atkDirectionChanges = true;
        charAtk.atkFXFlip = !sOWeaponMotionSlash.clockwise;
    }
    
    void DirectionsChangeForAtkFX() {
        // If its clockwise the sprite flipX should be false, because the sprite animations are usually created from left to right (clockwise).
        charAtk.atkDirectionChanges = true;
        charAtk.atkFXFlip = !sOWeaponMotionSlash.clockwise;
    }

    //Stop rotations, used for weapon swapping, ...interrupts like stuns?
    public override void StopMotions() {
        resetWeapOn = false;
        weapMotionOn = false;
        moveTimer = 0f;
    }
}
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_StabMotion : Weapon_Motion
{
    [Header("Script References")]
    public Character_Attack charAtk;
    [Header("To-set Variables")]
    public SO_Weapon_Motion_Stab sOWeaponMotionStab;
    [Header("Read Only")]
    public bool resetWeapRot;
    public bool weapMotionOn;
    public Transform weaponTrans;
    public SpriteRenderer weaponSpriteR;
    private float moveTimer;
    //
    private float restingY;
    private float curMotionDur;
    private AnimationCurve curAnimCurve;
    private float[] motionDurations;
    private float[] yPositions;
    private AnimationCurve[] animCurves;
    //
    private float curYPos, startYPos, endYPos;
    private int curMotion;

    void Update() {
        // Rotate charAtk.weapon back to its reset position.
        if (resetWeapRot) {
            moveTimer += Time.deltaTime / sOWeaponMotionStab.resetYPosDuration;
            // Lerp.
            curYPos = Mathf.Lerp(startYPos, endYPos, curAnimCurve.Evaluate(moveTimer));
            if (moveTimer >= 1f) {
                resetWeapRot = false;
                moveTimer = 0f;
                curYPos = endYPos;
                //weaponSpriteR.color= Color.white;
            }
            weaponTrans.localPosition = new Vector3(weaponTrans.localPosition.x, curYPos, weaponTrans.localPosition.z);
        }

        if (weapMotionOn) {
            moveTimer += Time.deltaTime / curMotionDur;
            curYPos = Mathf.Lerp(startYPos, endYPos, curAnimCurve.Evaluate(moveTimer));
            if (moveTimer >= 1f) {
                SetupNextMotion();
            }
            weaponTrans.localPosition = new Vector3(weaponTrans.localPosition.x, curYPos, weaponTrans.localPosition.z);
        }
    }

    void SetupNextMotion() {
        curMotion++;
        // If there are no more attack motions.
        if (curMotion == motionDurations.Length) {
            weapMotionOn = false;
            resetWeapRot = true;
            curYPos = endYPos;
            startYPos = curYPos;
            endYPos = restingY;
            moveTimer = 0f;
            charAtk.readyToAtk = true;
            charAtk.atkChain.ready = true;
            // Setup weapon reset here.
            return;
        }
        curMotionDur = motionDurations[curMotion];
        curYPos = endYPos;
        startYPos = curYPos;
        endYPos = yPositions[curMotion];
        curAnimCurve = animCurves[curMotion];
        moveTimer = 0f;
    }

    public override void WeaponMotionSetup(Character_Attack _charAtk, Transform _weaponTrans, SpriteRenderer _weaponSpriteR) {
        // References from the character.
        weaponTrans = _weaponTrans;
        weaponSpriteR = _weaponSpriteR;
        charAtk = _charAtk;
        // References from the motion SO associated with the current attack chain.
        sOWeaponMotionStab = charAtk.weapon.attackChains[charAtk.atkChain.curChain].sO_Weapon_Motion as SO_Weapon_Motion_Stab;
        motionDurations = sOWeaponMotionStab.motionDurations;
        yPositions = sOWeaponMotionStab.yPositions;
        animCurves = sOWeaponMotionStab.animCurves;
        restingY = sOWeaponMotionStab.restingYPosition;
        curMotion = 0;
        // First motion setup.
        curMotionDur = motionDurations[curMotion];
        startYPos = restingY;
        endYPos = yPositions[curMotion];
        curAnimCurve = animCurves[curMotion];
        moveTimer = 0f;
        // Enable motion.
        resetWeapRot = false;
        weapMotionOn = true;
    }

    //Stop rotations, used for weapon swapping, ...interrupts, like stuns?
    public override void StopMotions() {
        resetWeapRot = false;
        weapMotionOn = false;
        moveTimer = 0f;
    }
}
*/
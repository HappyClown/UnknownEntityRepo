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
        // Go through all the weapon motions in the current attack.
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
        // At what motion can the player start his next attack and swap his weapon.
        if (curMotion >= sOWeaponMotionSlash.allowAttackAndSwap) {
            charAtk.readyToAtk = true;
            charAtk.atkChain.ready = true;
            charAtk.equippedWeapons.canSwapWeapon = true;
        }
        // If there are no more attack motions.
        if (curMotion == motionDurations.Length) {
            weapMotionOn = false;
            resetWeapOn = true;
            curRot = endRot;
            startRot = curRot;
            endRot = restingRotation*-1;
            moveTimer = 0f;
            // charAtk.readyToAtk = true;
            // charAtk.atkChain.ready = true;
            // Setup weapon reset here.
            return;
        }
        // Setup the next weapon motion values.
        curMotionDur = motionDurations[curMotion];
        curRot = endRot;
        startRot = curRot;
        endRot = rotations[curMotion];
        curAnimCurve = animCurves[curMotion];
        moveTimer = 0f;
    }

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
        //
        weaponTrans.localPosition = sOWeaponMotionSlash.restingPosition;
        weaponTrans.localRotation = Quaternion.Euler(sOWeaponMotionSlash.restingRotation);
    }
    // If the weapon and attack FX need to change direction between attacks.
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

    //Stop rotations, used for weapon swapping, ...interrupts like stuns?
    public override void StopMotions() {
        resetWeapOn = false;
        weapMotionOn = false;
        moveTimer = 0f;
        // charAtk.readyToAtk = true;
        // charAtk.atkChain.ready = true;
        // charAtk.equippedWeapons.canSwapWeapon = true;
    }
}
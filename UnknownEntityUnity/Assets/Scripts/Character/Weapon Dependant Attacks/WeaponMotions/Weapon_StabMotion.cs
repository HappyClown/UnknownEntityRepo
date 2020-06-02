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
    //
    private bool camNudged;

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
        // At what motion can the player start his next attack and swap his weapon.
        if (curMotion >= sOWeaponMotionStab.allowAttackAndSwap) {
            charAtk.readyToAtk = true;
            charAtk.atkChain.ready = true;
            charAtk.equippedWeapons.canSwapWeapon = true;
        }
        // At what motion will the camera nudge.
        if (curMotion >= sOWeaponMotionStab.nudgeCamera && !camNudged) {
            CameraFollow.CameraNudge_St((charAtk.moIn.mousePosWorld2D - (Vector2)charAtk.transform.position).normalized, sOWeaponMotionStab.nudgeDistance);
            camNudged = true;
        }
        // If there are no more attack motions.
        if (curMotion >= motionDurations.Length) {
            weapMotionOn = false;
            resetWeapRot = true;
            curYPos = endYPos;
            startYPos = curYPos;
            endYPos = restingY;
            moveTimer = 0f;
            //charAtk.readyToAtk = true;
            //charAtk.atkChain.ready = true;
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
        restingY = sOWeaponMotionStab.restingPosition.y;
        curMotion = 0;
        // First motion setup.
        curMotionDur = motionDurations[curMotion];
        startYPos = restingY;
        endYPos = yPositions[curMotion];
        curAnimCurve = animCurves[curMotion];
        moveTimer = 0f;
        camNudged = false;
        // Enable motion.
        resetWeapRot = false;
        weapMotionOn = true;
        // 
        weaponTrans.localPosition = sOWeaponMotionStab.restingPosition;
        weaponTrans.localRotation = Quaternion.Euler(sOWeaponMotionStab.restingRotation);
    }

    //Stop rotations, used for weapon swapping, ...interrupts, like stuns?
    public override void StopMotions() {
        resetWeapRot = false;
        weapMotionOn = false;
        moveTimer = 0f;
        weaponTrans.localPosition = sOWeaponMotionStab.restingPosition;
        weaponTrans.localRotation = Quaternion.Euler(sOWeaponMotionStab.restingRotation);
        // charAtk.readyToAtk = true;
        // charAtk.atkChain.ready = true;
        // charAtk.equippedWeapons.canSwapWeapon = true;
    }
}

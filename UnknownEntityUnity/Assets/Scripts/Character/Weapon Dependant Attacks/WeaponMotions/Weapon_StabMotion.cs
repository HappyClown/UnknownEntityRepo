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
    public bool atkWeapRot;
    public Transform weaponTrans;
    public SpriteRenderer weaponSpriteR;
    private AnimationCurve atkAnimCurve;
    private float moveTimer;
    private float weaponMoveDur;
    private float curYPos, startYPos, endYPos;
    //private float waitingForResetAngle, restingAngle;
    //private float startAngle, curZAngle, moveTimer, endAngle;
    //private bool clockwise = true;

    void Update() {
        // Rotate charAtk.weapon back to its reset position.
        if (resetWeapRot) {
            moveTimer += Time.deltaTime / sOWeaponMotionStab.resetYPosDuration;
            // Lerp.
            curYPos = Mathf.Lerp(startYPos, endYPos, atkAnimCurve.Evaluate(moveTimer));
            if (moveTimer >= 1f) {
                resetWeapRot = false;
                moveTimer = 0f;
                curYPos = endYPos;
                weaponSpriteR.color= Color.white;
            }
            weaponTrans.localPosition = new Vector3(weaponTrans.localPosition.x, curYPos, weaponTrans.localPosition.z);
        }

        // Rotate charAtk.weapon to the other side simulating an attack.
        if (atkWeapRot) {
            moveTimer += Time.deltaTime / weaponMoveDur;
            // Lerp.
            curYPos = Mathf.Lerp(startYPos, endYPos, atkAnimCurve.Evaluate(moveTimer));
            if (moveTimer >= 1f) {
                charAtk.readyToAtk = true;
                charAtk.atkChain.ready = true;
                atkWeapRot = false;
                moveTimer = 0f;
                resetWeapRot = true;
                curYPos = endYPos;
                endYPos = startYPos;
                startYPos = curYPos;
                weaponSpriteR.color= Color.gray;
            }
            weaponTrans.localPosition = new Vector3(weaponTrans.localPosition.x, curYPos, weaponTrans.localPosition.z);
        }
    }

    // Instantly rotate the charAtk.weapon to the other side.
    public override void WeaponMotionSetup(Character_Attack _charAtk, Transform _weaponTrans, SpriteRenderer _weaponSpriteR) {
        weaponTrans = _weaponTrans;
        weaponSpriteR = _weaponSpriteR;
        charAtk = _charAtk;
        sOWeaponMotionStab = charAtk.weapon.attackChains[charAtk.atkChain.curChain].sO_Weapon_Motion as SO_Weapon_Motion_Stab;
        atkAnimCurve = sOWeaponMotionStab.attackYPosAnimCurve;
        weaponMoveDur = sOWeaponMotionStab.weaponMotionDuration;

        resetWeapRot = false;
        moveTimer = 0f;

        weaponSpriteR.color= Color.white;

        atkWeapRot = true;
        startYPos = 0;
        endYPos = startYPos+0.3f;
    }

    //Stop rotations, used for weapon swapping, ...interrupts like stuns?
    public override void StopMotions() {
        resetWeapRot = false;
        atkWeapRot = false;
        moveTimer = 0f;
    }
}

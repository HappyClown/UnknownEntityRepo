using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AttackWeaponMotion : MonoBehaviour
{
    public bool resetWeapRot;
    public Transform weaponTran;
    public SpriteRenderer weaponSpriteR;
    private bool weaponFlip = false;
    private float rotAngle, baseRotAngle;
    private float tarZEuler, iniZEuler, diffZEuler, curZEuler, rotTimer, finalAtkAngle;
    private bool atkWeapRot, passByZero;
    private float weapRotDur;
    [Header("Scripts")]
    public SO_WeaponBase weapon;
    public Character_AttackChain atkChain;
    public Character_Attack charAtk;

    void Start()
    {
        weaponTran.localEulerAngles = new Vector3(weaponTran.localEulerAngles.x, weaponTran.localEulerAngles.y, weapon.baseAngle);
        
    }

    void Update()
    {
        // Rotate weapon back to its reset position.
        if (resetWeapRot) {
            rotTimer += Time.deltaTime / weapon.rotationDuration;
            curZEuler = Mathf.LerpAngle(iniZEuler, tarZEuler, rotTimer);
            if (rotTimer >= 1f) {
                resetWeapRot = false;
                rotTimer = 0f;
                curZEuler = tarZEuler;
                weaponSpriteR.color= Color.white;
            }
            weaponTran.localEulerAngles = new Vector3(weaponTran.localEulerAngles.x, weaponTran.localEulerAngles.y, curZEuler);
        }
        // Rotate weapon to the other side simulating an attack.
        if (atkWeapRot) {
            rotTimer += Time.deltaTime / (weapRotDur/2);
            curZEuler = Mathf.LerpAngle(iniZEuler, tarZEuler, rotTimer);
            if (rotTimer >= 1f) {
                if (passByZero) {
                    passByZero = false;
                    rotTimer -= 1;
                    tarZEuler = finalAtkAngle;
                    iniZEuler = weaponTran.localEulerAngles.z;
                }
                else {
                    charAtk.readyToAtk = true;
                    atkChain.ready = true;
                    atkWeapRot = false;
                    rotTimer = 0f;
                    curZEuler = tarZEuler;
                    iniZEuler = baseRotAngle + rotAngle;
                    tarZEuler = baseRotAngle + rotAngle + diffZEuler;
                    weaponSpriteR.color= Color.gray;
                }
            }
            weaponTran.localEulerAngles = new Vector3(weaponTran.localEulerAngles.x, weaponTran.localEulerAngles.y, curZEuler);
            //Debug.Log("My local weapon rotation is: " + curZEuler);
        }
    }
    // Instatly rotate the weapon to the other side.
    public void WeaponMotion(float _weapRotDur) {
        if (weaponFlip) {
            rotAngle = weapon.rotationAngle;
            diffZEuler = -weapon.rotationDifference;
            baseRotAngle = -weapon.baseAngle;
            weaponFlip = false;
        }
        else {
            rotAngle = -weapon.rotationAngle;
            diffZEuler = weapon.rotationDifference;
            baseRotAngle = weapon.baseAngle;
            weaponFlip = true;
        }
        resetWeapRot = false;
        rotTimer = 0f;
        weapRotDur = _weapRotDur;
        atkWeapRot = true;
        weaponSpriteR.color= Color.white;
        iniZEuler = weaponTran.localEulerAngles.z;
        finalAtkAngle = baseRotAngle + rotAngle;
        tarZEuler = -180f;
        passByZero = true;
    }
}

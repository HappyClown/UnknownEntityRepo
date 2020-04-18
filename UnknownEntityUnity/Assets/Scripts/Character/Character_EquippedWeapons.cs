﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_EquippedWeapons : MonoBehaviour
{
    [Header("Script References")]
    public Character_Attack charAtk;
    [Header("To-set Variables")]
    public SO_Weapon firstWeap;
    public SpriteRenderer weaponSpriteR;
    [Header("Read Only")]
    public SO_Weapon secondWeap;

    void Start() {
        Changes();
    }

    void Update() {
        // Weapon swap if two weapons are equipped.
        if (Input.GetKeyDown("q")) {
            if (firstWeap != null && secondWeap != null) {
                SO_Weapon tempSecondWeap = secondWeap;
                secondWeap = firstWeap;
                firstWeap = tempSecondWeap;
                Changes();
            }
        }
    }

    public void Changes() {
        // Stop its rotations.
        if (charAtk.curWeaponMotion) {
            charAtk.curWeaponMotion.StopMotions();
        }
        //Assign the weapon motion script.

        for (int i = 0; i < firstWeap.attackChains.Length; i++) {
            if (charAtk.weaponMotions.Count < i+1) {
                charAtk.weaponMotions.Add(charAtk.weaponMotionController.CheckMotionList(firstWeap.attackChains[i].sO_Weapon_Motion.weapon_Motion));
                //charAtk.weaponMotions[i] = 
            }
            else {
                charAtk.weaponMotions[i] = charAtk.weaponMotionController.CheckMotionList(firstWeap.attackChains[i].sO_Weapon_Motion.weapon_Motion);
            }
        }
        charAtk.weaponMotion = charAtk.weaponMotions[0];

        // Assign the Scriptable Object weapon in the Character_Attack script.
        charAtk.weapon = firstWeap;
        // Assign its resting angle.
        charAtk.weaponTrans.localEulerAngles = firstWeap.restingRotation;
        // Assign its resting position.
        charAtk.weaponTrans.localPosition = firstWeap.restingPosition;
        // Change the weapon's appearance.
        weaponSpriteR.sprite = firstWeap.weaponSprite;
        // Find somewhere to insert the rest of the previous weapon's attack duration. 
        weaponSpriteR.color = Color.white;
        // Go back to the first chain.
        charAtk.atkChain.OnWeaponSwap();
    }
}
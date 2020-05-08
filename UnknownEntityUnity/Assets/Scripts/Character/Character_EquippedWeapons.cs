using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_EquippedWeapons : MonoBehaviour
{
    [Header("Script References")]
    public Character_Attack charAtk;
    public MouseInputs moIn;
    [Header("To-set Variables")]
    public SO_Weapon firstWeap;
    public SpriteRenderer weaponSpriteR;
    [Header("Read Only")]
    public SO_Weapon secondWeap;
    public bool canSwapWeapon = true;

    void Start() {
        Changes();
    }

    void Update() {
        // Weapon swap if two weapons are equipped.
        if (canSwapWeapon && moIn.weaponSwapPressed) {
            moIn.weaponSwapPressed = false;
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
        // Change the weapon's appearance.
        weaponSpriteR.sprite = firstWeap.weaponSprite;
        // Find somewhere to insert the rest of the previous weapon's attack duration. 
        weaponSpriteR.color = Color.white;
        // Go back to the first chain.
        charAtk.atkChain.OnWeaponSwap(firstWeap, secondWeap);
        // Assign its resting angle.
        charAtk.weaponTrans.localEulerAngles = charAtk.WeapAtkChain.sO_Weapon_Motion.restingRotation;
        // Assign its resting position.
        charAtk.weaponTrans.localPosition = charAtk.WeapAtkChain.sO_Weapon_Motion.restingPosition;
    }
}
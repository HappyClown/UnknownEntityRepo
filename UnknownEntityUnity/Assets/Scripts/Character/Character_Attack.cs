using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Attack : MonoBehaviour
{
    [Header("Script References")]
    public MouseInputs moIn;
    public Character_AttackChain atkChain;
    public Character_AttackDetection atkDetection;
    //public Character_AttackWeaponMotion atkWeaMotion;
    public Character_AttackFXPool atkFXPool;
    public Character_AttackVisual atkVisual;
    public Character_AttackMovement atkMovement;
    public Character_AttackPlayerMovement atkPlyrMove;
    public Weapon_MotionController weaponMotionController;
    [Header("To-set Variables")]
    public Transform weaponTrans;
    public SpriteRenderer weaponSpriteR;
    [Header("Read Only")]
    public bool readyToAtk;
    // The initial weapon is set inside the Character_EquippedWeapon script.
    public SO_Weapon weapon;
    public Weapon_Motion weaponMotion;
    public Weapon_Motion curWeaponMotion;
    public List<Weapon_Motion> weaponMotions = new List<Weapon_Motion>();
    public Character_AttackFX atkFX;

    void Update() {
        // If I clicked and can attack.
        if (moIn.mouseLeftClicked && atkChain.ready) {
            // Checks and adjusts the current attack chain. (Chaining attacks)
            atkChain.ChainAttacks();
            // Stop the previous motion if needed.
            StopPreviousMotion();
            // Set the weapon back to its resting position and rotation. Usually the first attack chain's resting values.
            ResetWeaponLocalValues();
            // If I want to check the weapon motion every attack. (attack chains can have different motions)
            //curWeaponMotion = weaponMotionController.CheckMotionList(weaponMotion);
            curWeaponMotion = weaponMotions[atkChain.curChain];
            // Start the attack.
            curWeaponMotion.WeaponMotionSetup(this, weaponTrans, weaponSpriteR);
            // Request an attack FX from the attack FX pool, the attack FX contains a Sprite Renderer and a PolygonalCollider2D.
            atkFX = atkFXPool.RequestAttackFX();
            // Handles moving the player, slowing him down, etc., during the attack. (Player motion)
            atkPlyrMove.SetupPlayerAttackMotions(WeapAtkChain.sO_CharAtk_Motion);
            // Enables and changes the attack effect over the course of the attack and dictates if the atkFX pool object is inUse then not.
            atkVisual.StartCoroutine(atkVisual.AttackAnimation(WeapAtkChain.sO_AttackFX, atkFX));
            // Enables and disables the attack's collider during the "animation" and detects colliders it can hit, once.
            atkDetection.StartCoroutine(atkDetection.AttackCollider(WeapAtkChain, WeapAtkChain.sO_AttackFX, atkFX.col));
            // Moves the attack effect over the course of the attack.
            atkMovement.StartCoroutine(atkMovement.AttackMovement(WeapAtkChain.sO_AttackFX, atkFX.transform));
        }
    }
    // If I want to check only when a new weapon is equipped, call this from the Character_EquippedWeapons.Change(). (every attack chain with the same motion)
    public void AdjustMotionList() {
        curWeaponMotion = weaponMotionController.CheckMotionList(weaponMotion);
    }

    public void ResetWeaponLocalValues() {
        weaponTrans.localPosition = weapon.restingPosition;
        weaponTrans.localEulerAngles = weapon.restingRotation;
    }
    public void StopPreviousMotion() {
        if (curWeaponMotion != null) {
            curWeaponMotion.StopMotions();
        }
    }

    // // Shorter reference to the current attack chain's weapon rotation duration.
    // public float WeaponMotionDuration {
    //     get {
    //         return WeapAtkChain.weaponMotionDuration;
    //     }
    // }
    // Shorter reference to the weapon's scriptable object attack chain class.
    public SO_Weapon.AttackChain WeapAtkChain {
        get {
            return weapon.attackChains[atkChain.curChain];
        }
    }
}

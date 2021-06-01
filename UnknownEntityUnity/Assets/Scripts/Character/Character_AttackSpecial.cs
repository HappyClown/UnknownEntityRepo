using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AttackSpecial : MonoBehaviour
{
    [Header("Script References")]
    public MouseInputs moIn;
    public Character_Attack charAtk;
    [Header("Attack FXs")]
    public SO_Weapon mainSpecialSO;
    public SO_CharAtk_Motion SO_WindupCharMo; // Character motion
    public SO_Weapon_Motion sOWeapoMo; // Weapon Motion
    public List<SO_AttackFX> SO_WindupFX, SO_HoldFX, SO_ReleaseFX; // FXs
    public Weapon_Motion weaponMotion;
    public List<Character_AttackFX> windupFX, holdFX, releaseFX;
    //public Coroutine windupCoroutine, holdCoroutine, releaseCoroutine;
    //[Header("Player Motion")]
    //public Character_AttackFX windupFX, holdFX, releaseFX;
    //public Coroutine windupCoroutine, holdCoroutine, releaseCoroutine;
    [Header("General Variables")]
    public bool specAtkButtonDown;
    public bool inWindup, inHold, inRelease;
    private Coroutine specialAtkCoroutine;

    public void SpecialAttack() {
        charAtk.ReadyToAttack(false);
        mainSpecialSO = charAtk.weapon;

        SO_WindupFX[0] = mainSpecialSO.specialAttack.sO_AttackFXWindup[0];
        SO_HoldFX[0] = mainSpecialSO.specialAttack.sO_AttackFXHold[0];
        //print(mainSpecialSO.specialAttack.sO_AttackFXRelease.Length);
        SO_ReleaseFX.Clear();
        for (int i = 0; i < mainSpecialSO.specialAttack.sO_AttackFXRelease.Length; i++) {
            //print("HELLO");
            if (i >= SO_ReleaseFX.Count) {
                SO_ReleaseFX.Add(mainSpecialSO.specialAttack.sO_AttackFXRelease[i]);
            }
            else {
                SO_ReleaseFX[i] = mainSpecialSO.specialAttack.sO_AttackFXRelease[i];
            }
        }
        
        SO_WindupCharMo = mainSpecialSO.specialAttack.sO_CharAtk_Motion;
        sOWeapoMo = mainSpecialSO.specialAttack.sO_Weapon_Motion;

        // Perform the special attack.
        // If the attack is triggered, make sure that the grace period is set back to false since it is used.
        moIn.attackButtonActions.attackButtonTapInGrace = false;
        // Disallow weapon swapping.
        charAtk.equippedWeapons.canSwapWeapon = false;
        // Stop the previous motion if needed.
        charAtk.StopPreviousMotion();
        // Set the weapon back to its resting position and rotation. Usually the first attack chain's resting values.
        charAtk.ResetWeaponLocalValues();
        // Allow character flip and the weapon to "look at" the mouse.
        charAtk.ForceCharFlipAndWeaponLookAt();
        weaponMotion = charAtk.weaponMotionController.CheckMotionList(sOWeapoMo.weapon_Motion);
        //windupFX = null;
        //holdFX = null;
        //releaseFX = null;
        releaseFX.Clear();
        windupFX[0] = charAtk.atkFXPool.RequestAttackFX();
        windupFX[0].inUse = true;
        holdFX[0] = charAtk.atkFXPool.RequestAttackFX();
        holdFX[0].inUse = true;
        for (int i = 0; i < SO_ReleaseFX.Count; i++) {
            if (i >= releaseFX.Count) {
                releaseFX.Add(charAtk.atkFXPool.RequestAttackFX());
            }
            else {
                releaseFX[i] = charAtk.atkFXPool.RequestAttackFX();
            }
            releaseFX[i].inUse = true;
        }
        
        if (specialAtkCoroutine != null) { 
            specialAtkCoroutine = null;
        }
        specialAtkCoroutine = StartCoroutine(InSpecialAttack());
    }

    public void SpecialAttackButtonReleased(bool releasedByPlayer) {
        specAtkButtonDown = false;
        // Can the player use their movement skill?
        if (inWindup) { 
            //CANCELATTACK;
            weaponMotion.StopMotions();
            windupFX[0].StopAllCoroutines();
            windupFX[0].spriteR.sprite = null;
            windupFX[0].col.enabled = false;
            windupFX[0].gameObject.SetActive(false);
            windupFX[0].stopOnStun = false;
            windupFX[0].inUse = false;
            holdFX[0].inUse = false;
            foreach(Character_AttackFX charAtkFX in releaseFX) {
                charAtkFX.inUse = false;
            }
            charAtk.atkPlyrMove.StopPlayerMotion();
            charAtk.ReadyToAttack(true);
            StopCoroutine(specialAtkCoroutine);
            specialAtkCoroutine = null;
            inWindup = false;
            charAtk.equippedWeapons.canSwapWeapon = true;
        }
        if (inHold) { 
            if (releasedByPlayer) {
                holdFX[0].exitAttackAnimationFX = true;
                charAtk.atkPlyrMove.exitPlayerMotion = true;
            } 
            else {
                //CANCELATTACK;
            }
        }
    }

    IEnumerator InSpecialAttack() {
        // General Variables
        float timer = 0f;
        // Windup
        inWindup = true;
        windupFX[0].gameObject.SetActive(true);
        windupFX[0].StartCoroutine(charAtk.atkVisual.AttackAnimation(SO_WindupFX[0], windupFX[0]));
        windupFX[0].StartCoroutine(charAtk.atkMovement.AttackMovement(SO_WindupFX[0], windupFX[0].transform));
        charAtk.atkPlyrMove.SetupPlayerAttackMotions(SO_WindupCharMo);
        weaponMotion.WeaponMotionSetup(charAtk, charAtk.weaponTrans, charAtk.weaponSpriteR, sOWeapoMo);

        while (timer < SO_WindupFX[0].totalDuration) {
            timer += Time.deltaTime;
            yield return null;
        }
        inWindup = false;
        timer = 0f;
        // Hold
        inHold = true;
        holdFX[0].gameObject.SetActive(true);
        //holdFX.loopAnimation = true;
        holdFX[0].StartCoroutine(charAtk.atkVisual.AttackAnimation(SO_HoldFX[0], holdFX[0]));
        holdFX[0].StartCoroutine(charAtk.atkMovement.AttackMovement(SO_HoldFX[0], holdFX[0].transform));
        //charAtk.atkPlyrMove.SetupPlayerAttackMotions(SO_HoldCharMo);
        while (specAtkButtonDown) {
            yield return null;
        }
        holdFX[0].exitAttackAnimationFX = true;
        inHold = false;
        // Release
        inRelease = true;
        // foreach(Character_AttackFX releaseAtkFX in releaseFX) {
        //     releaseFX[0].gameObject.SetActive(true);
        //     releaseFX[0].StartCoroutine(charAtk.atkVisual.AttackAnimation(SO_ReleaseFX[0], releaseFX[0]));
        //     releaseFX[0].StartCoroutine(charAtk.atkMovement.AttackMovement(SO_ReleaseFX[0], releaseFX[0].transform));
        // }
        for (int i = 0; i < releaseFX.Count; i++) {
            releaseFX[i].gameObject.SetActive(true);
            releaseFX[i].StartCoroutine(charAtk.atkVisual.AttackAnimation(SO_ReleaseFX[i], releaseFX[i]));
            releaseFX[i].StartCoroutine(charAtk.atkMovement.AttackMovement(SO_ReleaseFX[i], releaseFX[i].transform));
        }
        if (SO_ReleaseFX[0].collider != null) {
            charAtk.atkDetection.StartCoroutine(charAtk.atkDetection.AttackCollider(charAtk.equippedWeapons.weaponOneIsActive, charAtk.weapon, charAtk.WeapAtkChain, SO_ReleaseFX[0], releaseFX[0].col));
        }
        weaponMotion.StopHoldingMotion();
        while (timer < SO_ReleaseFX[0].totalDuration) {
            timer += Time.deltaTime;
            yield return null;
        }
        inRelease = false;
        timer = 0f;
        charAtk.equippedWeapons.canSwapWeapon = true;
        charAtk.ReadyToAttack(true);
    }

    // Hold rightclick > weaponmotion+atkFX+playermotion+etc.(windup) > btn held state where effects stay active and loop (hold) > weapmotion+atkFX+etc.(release).

    // need to detect at all times the state of the special attack button (right click)

    // cancel point for the attack if it is released before the windup ends

    // early releases with different and incresed effect upon release


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Attack : MonoBehaviour
{
    //public float attackSpeed;
    [Header("Scripts")]
    public MouseInputs moIn;
    public SO_WeaponBase weapon;
    public Character_AttackChain atkChain;
    public Character_AttackDetection atkDetection;
    public Character_AttackWeaponMotion atkWeaMotion;
    public Character_AttackVisual atkVisual;
    public Character_AttackMovement atkMovement;
    public bool readyToAtk;

    void Update() {
        if (moIn.mouseLeftClicked && atkChain.ready) {
            atkWeaMotion.WeaponMotion(AttackSpeed);
            Debug.Log(AttackSpeed);
            atkChain.ChainAttacks();
            atkDetection.StartCoroutine(atkDetection.AttackCollider(WeapAtkChain.collider, WeapAtkChain.collisionStart, WeapAtkChain.collisionEnd, atkChain.curChain));
            atkVisual.StartCoroutine(atkVisual.AttackAnimation(WeapAtkChain.attackSprites, WeapAtkChain.attackSpriteChanges, WeapAtkChain.attackLength,  atkChain.curChain));
            atkMovement.StartCoroutine(atkMovement.AttackMovement(atkChain.curChain, WeapAtkChain.spawnDistance, WeapAtkChain.moveDistance, WeapAtkChain.moveDelay, WeapAtkChain.attackLength, WeapAtkChain.moveCurve));
        }
    }

    public float AttackSpeed {
        get {
            return WeapAtkChain.attackSpeed;
        }
    }

    public SO_WeaponBase.AttackChain WeapAtkChain {
        get {
            return weapon.attackChains[atkChain.curChain];
        }
    }
}

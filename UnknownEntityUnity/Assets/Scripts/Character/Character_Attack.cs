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
    public Character_AttackPlayerMovement atkPlyrMove;
    public bool readyToAtk;

    void Update() {
        if (moIn.mouseLeftClicked && atkChain.ready) {
            atkChain.ChainAttacks();
            atkPlyrMove.AttackPlayerMovement(WeapAtkChain.plyrSlowDown, WeapAtkChain.plyrSlowDownDur, WeapAtkChain.attackLength, WeapAtkChain.plyrMoveDist, atkChain.curChain);
            atkWeaMotion.WeaponMotion(WeapRotDur);
            atkDetection.StartCoroutine(atkDetection.AttackCollider(WeapAtkChain.collider, WeapAtkChain.collisionStart, WeapAtkChain.collisionEnd, atkChain.curChain));
            atkVisual.StartCoroutine(atkVisual.AttackAnimation(WeapAtkChain.attackSprites, WeapAtkChain.attackSpriteChanges, WeapAtkChain.attackLength,  atkChain.curChain));
            atkMovement.StartCoroutine(atkMovement.AttackMovement(atkChain.curChain, WeapAtkChain.spawnDistance, WeapAtkChain.moveDistance, WeapAtkChain.moveDelay, WeapAtkChain.attackLength, WeapAtkChain.moveCurve));
        }
    }

    public float WeapRotDur {
        get {
            return WeapAtkChain.weapRotDur;
        }
    }

    public SO_WeaponBase.AttackChain WeapAtkChain {
        get {
            return weapon.attackChains[atkChain.curChain];
        }
    }
}

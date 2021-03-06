﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class SO_WeaponBase : ScriptableObject
{
    public string weaponName;
    public Sprite weaponSprite;
    public float chainResetDelay;
    public AttackChain[] attackChains;
    [System.Serializable]
    public class AttackChain {
         public float minDamage, maxDamage;
        public float plyrSlowDown, plyrSlowDownDur, plyrMoveDist;
        public float weapRotDur;
        public float attackLength;
        public float collisionStart, collisionEnd;
        public Sprite[] attackSprites;
        public float[] attackSpriteChanges;
        // This would be to normalize the attackSpriteChanges timings from 0 to 1 instead of from 0 to attackLength.
        // public float spriteChange {
        //     get {
        //         return spriteChange * attackLength;
        //     }
        // }
        public float spawnDistance;
        public float moveDistance, moveDelay;
        public AnimationCurve moveCurve;
        public PolygonCollider2D collider;
    }
    
    [Header("Weapon Motion")]
    public Weapon_Motion weapon_Motion;
    // If you want to alternate and control the rotation angle of the attack.
    public float restingAngle;
    public float waitingForResetAngle;
    public float resetRotDuration;
    public AnimationCurve attackRotAnimCurve;
    public float RotationDifference {
        get {
            return Mathf.Abs(restingAngle-waitingForResetAngle);
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Weapon", menuName = "SOWeapons/SO_Weapon", order = 0)]
public class SO_Weapon : ScriptableObject
{
    public string weaponName;
    public Sprite weaponSprite;
    public float chainResetDelay;
    public Vector3 restingRotation, restingPosition;
    public AttackChain[] attackChains;
    [System.Serializable]
    public class AttackChain {
        public SO_Weapon_Motion sO_Weapon_Motion;
        // /////////////////////////////////////////////////////////////////////
        public SO_CharAtk_Motion sO_CharAtk_Motion;
        // /////////////////////////////////////////////////////////////////////
         public float minDamage, maxDamage;
        //public float plyrSlowDown, plyrSlowDownDur, plyrMoveDist;
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
}

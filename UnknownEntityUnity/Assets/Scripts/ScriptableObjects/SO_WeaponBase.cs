using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class SO_WeaponBase : ScriptableObject
{
    public string weaponName;
    public float chainResetDelay;
    public Sprite weaponSprite;
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
    public float rotationDuration, rotationAngle, rotationDifference;
    public float baseAngle;


}

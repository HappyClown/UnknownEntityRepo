using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Weapon_Main", menuName = "SO_WeaponStuff/Weapon_Main", order = 0)]
public class SO_Weapon : ScriptableObject
{
    public string weaponName;
    public Sprite weaponSprite;
    public float chainResetDelay;
    public Vector3 restingRotation, restingPosition;
    public float durabilityDamage = 1f;
    public AttackChain[] attackChains;
    [System.Serializable]
    public class AttackChain {
        public SO_Weapon_Motion sO_Weapon_Motion;
        // /////////////////////////////////////////////////////////////////////
        public SO_CharAtk_Motion sO_CharAtk_Motion;
        // /////////////////////////////////////////////////////////////////////
        public SO_AttackFX[] sO_AttackFXs;
        // /////////////////////////////////////////////////////////////////////
         public float minDamage, maxDamage;
         public float DamageRoll {
             get{ return Random.Range(minDamage, maxDamage); }
         }
         // ////////////////////////////////////////////////////////////////////
         public bool chargeable;
         public SO_ChargeAttack sO_ChargeAttack;
    }
}

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
    public bool durabilityDamageOnHit = true; // Whether the durability damage is applied when an attack hits a target or just when an attack is done. (past the attack cancelation point) 
    public float durabilityDamage = 1f;
    public float poiseDamage = 1f;
    public SO_ObjectDestructionSpawner SO_brokenPiecesSpawner;
    public AttackChain[] attackChains;
    public SpecialAttack specialAttack;
    [System.Serializable]
    public class AttackChain {
        public SO_Weapon_Motion sO_Weapon_Motion;
        // /////////////////////////////////////////////////////////////////////
        public SO_CharAtk_Motion sO_CharAtk_Motion;
        // /////////////////////////////////////////////////////////////////////
        public SO_AttackFX[] sO_AttackFXs;
        // /////////////////////////////////////////////////////////////////////
        public float minDamage, maxDamage;
        [Tooltip("The full duration of the attack starts when the attack is triggered and ends after this duration, used to track when certain actions can be done during an attack, IE when can the attack be interrupted, cancelled, etc.")]
        public float fullAttackDuration;
        [Tooltip("After how long during an attack can the player swap weapon, only works if it is smaller then the fullAttackDuration.")]
        public float allowWeaponSwap;
        [Tooltip("After how long during an attack can the player start the next attack, only works if the current attack is not the last one in this weapon's attack chains.")]
        public float allowNextChainAttack;
        [Tooltip("After the full attack duration the cooldown before another attack can be done starts.")]
        public float canAttackCooldown;
        //[Tooltip("Time frame in which the player can't interrupt his attack. Start and End times.")]
        //public float cantInterrupStart, cantInterrupEnd;
        //public float cancelPoint;
         public float DamageRoll {
             get{ return Random.Range(minDamage, maxDamage); }
         }
         // ////////////////////////////////////////////////////////////////////
         public bool chargeable;
         public SO_ChargeAttack sO_ChargeAttack;
    }
    [System.Serializable]
    public class SpecialAttack {
        public SO_Weapon_Motion sO_Weapon_Motion;
        // /////////////////////////////////////////////////////////////////////
        public SO_CharAtk_Motion sO_CharAtk_Motion;
        // /////////////////////////////////////////////////////////////////////
        public SO_AttackFX[] sO_AttackFXWindup, sO_AttackFXHold, sO_AttackFXRelease;
        public float minDamage, maxDamage;
        [Tooltip("The full duration of the attack starts when the attack is triggered and ends after this duration, used to track when certain actions can be done during an attack, IE when can the attack be interrupted, cancelled, etc.")]
        public float fullAttackDuration;
        [Tooltip("After the full attack duration the cooldown before another attack can be done starts.")]
        public float canAttackCooldown;
    }
}

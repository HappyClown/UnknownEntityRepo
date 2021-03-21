using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AttackChain : MonoBehaviour
{
    [Header("Script References")]
    //public MouseInputs moIn;
    public Character_Attack charAtk;
    //public Character_AttackWeaponMotion atkWeaMotion;
    [Header("To set variables")]
    public float chainResetDelay;
    [Header("Read Only")]
    //public bool ready = true;
    public int curChain;
    private int nextChain;
    private float chainResetTimer;

    void Start() {
        chainResetTimer = chainResetDelay;
    }

    void Update() {
        // Chain reset timer.
        if (chainResetTimer < chainResetDelay) {
            chainResetTimer += Time.deltaTime;
            if (chainResetTimer >= chainResetDelay) {
                nextChain = 0;
                curChain = 0;
                //atkWeaMotion.resetWeapRot = true;
            }
        }
    }

    public void ChainAttacks() {
        charAtk.ReadyToAttack(false);
        //charAtk.readyToAtk = false;
        //ready = false;
        // This attack is the last in the chain. Code based on that can be added here.
        if (nextChain >= charAtk.weapon.attackChains.Length) {
            nextChain = 0;
        }
        curChain = nextChain;
        // Turn this chain reset timer to 0 after the attack is complete.
        chainResetTimer = 0f;
        nextChain++;
    }

    // On weapon swap.
    public void OnWeaponSwap(SO_Weapon activeWeapon, SO_Weapon inactiveWeapon) {
        chainResetTimer = chainResetDelay;
        // If the next chain was higher then the max attack chains of the weapon that was just swapped, meaning that its last attack was done.
        if (nextChain >= inactiveWeapon.attackChains.Length) {
            nextChain = 0;
            curChain = 0;
        }
        // This is to keep the current chain, if its over the new first weapon's max chain count: set it to the last chain.
        if (nextChain >= activeWeapon.attackChains.Length) {
            nextChain = activeWeapon.attackChains.Length-1;
            curChain = 0;
        }
        // Same as above but if its above the new first weapon's max chain count, set it to first chain.
        // This is to reset the chain.
        // nextChain = 0;
        // curChain = 0;
        charAtk.ReadyToAttack(true);
        charAtk.readyToAtk = true;
        //ready = true;
    }
}
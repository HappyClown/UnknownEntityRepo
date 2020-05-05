using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AttackChain : MonoBehaviour
{
    [Header("Script References")]
    public MouseInputs moIn;
    public Character_Attack charAtk;
    public Character_AttackWeaponMotion atkWeaMotion;
    [Header("Read Only")]
    public bool ready = true;
    public int curChain;
    private int nextChain;
    private float chainResetTimer;

    void Start() {
        chainResetTimer = charAtk.weapon.chainResetDelay;
    }

    void Update() {
        // Chain reset timer.
        if (chainResetTimer < charAtk.weapon.chainResetDelay) {
            chainResetTimer += Time.deltaTime;
            if (chainResetTimer >= charAtk.weapon.chainResetDelay) {
                nextChain = 0;
                curChain = 0;
                //atkWeaMotion.resetWeapRot = true;
            }
        }
    }

    public void ChainAttacks() {
        charAtk.readyToAtk = false;
        ready = false;
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
    public void OnWeaponSwap(SO_Weapon firstWeap, SO_Weapon secondWeap) {
        chainResetTimer = charAtk.weapon.chainResetDelay;
        // This is to keep the current chain, if its over the new first weapon's max chain count: set it to the last chain.
        if (nextChain >= firstWeap.attackChains.Length) {
            nextChain = firstWeap.attackChains.Length-1;
            curChain = 0;
        }
        // Same as above but if its above the new first weapon's max chain count, set it to first chain.
        // This is to reset the chain.
        //nextChain = 0;
        //curChain = 0;
        charAtk.readyToAtk = true;
        ready = true;
    }
}
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
    private int nextChain = 0;
    private float chainResetTimer;
    private Coroutine atkDurCoroutine, atkCooldownCoroutine; 

    void Start() {
        chainResetTimer = chainResetDelay;
    }

    void Update() {
        // Full attack duration timer.

        // Cooldown during which player cannot attack.
        // Chain reset timer once the player can attack again.
        if (chainResetTimer < chainResetDelay) {
            chainResetTimer += Time.deltaTime;
            if (chainResetTimer >= chainResetDelay) {
                nextChain = 0;
                curChain = 0;
            }
        }
    }

    public void ChainAttacks() {
        // This attack is the last in the chain. Code based on that can be added here.
        if (nextChain >= charAtk.weapon.attackChains.Length) {
            nextChain = 0;
        }
        curChain = nextChain;
        // Turn this chain reset timer to 0 after the attack is complete.
        chainResetTimer = 0f;
        
        charAtk.ReadyToAttack(false);
        charAtk.equippedWeapons.canSwapWeapon = false;
        if (atkDurCoroutine != null) StopCoroutine(atkDurCoroutine);
        if (atkCooldownCoroutine != null) StopCoroutine(atkCooldownCoroutine);
        atkDurCoroutine = StartCoroutine(FullAttackDuration());
        nextChain++;
        
    }
    // public bool NotInAnAttack() {
    //     if (atkDurCoroutine != null) {
    //         return false;
    //     }
    //     return true;
    // }

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
    }

    IEnumerator FullAttackDuration() {
        float timer = 0f;
        float fullattackDuration = charAtk.weapon.attackChains[curChain].fullAttackDuration;
        //print("Full Attack Duration: "+fullattackDuration + "  AND  This attack chain: " + curChain);
        int currentAttackChain = curChain;
        while (timer < fullattackDuration) {
            timer += Time.deltaTime;
            // After a certain amount of time in the attack, allow weapon to be swapped.
            if (timer > charAtk.weapon.attackChains[curChain].allowWeaponSwap) {
                charAtk.equippedWeapons.canSwapWeapon = true;
            }
            // If this isn't the last attack in the weapon's attack chain allow another attack to be started after allowNextChainAttack time.
            if (nextChain < charAtk.weapon.attackChains.Length && timer > charAtk.weapon.attackChains[curChain].allowNextChainAttack) {
                charAtk.ReadyToAttack(true);
            }
            // Can check here when the player would be able to swap his weapon or start another attack if it can be done during an attack. Could have that the weapon swap can be done during the cooldown but not starting a new attack.
            yield return null;
        }
        // Start cooldown timer before enabling ReadyToAttack.
        atkCooldownCoroutine = StartCoroutine(CanAttackCooldown(currentAttackChain));
        chainResetTimer = 0f;
        atkDurCoroutine = null;
    }
    IEnumerator CanAttackCooldown(int currentAttackChain) {
        float timer = 0f;
        float canAttackCooldown = charAtk.weapon.attackChains[currentAttackChain].canAttackCooldown;
        while (timer < canAttackCooldown) {
            timer += Time.deltaTime;
            yield return null;
        }
        charAtk.ReadyToAttack(true);
        charAtk.equippedWeapons.canSwapWeapon = true;
        atkCooldownCoroutine = null;
    }
}
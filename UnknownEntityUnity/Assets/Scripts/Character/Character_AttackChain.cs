using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AttackChain : MonoBehaviour
{
    public int curChain;
    private int nextChain;
    private float chainResetTimer;
    public bool ready = true;
    [Header("Scripts")]
    public MouseInputs moIn;
    public SO_WeaponBase weapon;
    public Character_Attack charAtk;
    public Character_AttackWeaponMotion atkWeaMotion;

    void Start() {
        chainResetTimer = weapon.chainResetDelay;
    }

    void Update() {
        // Chain reset timer.
        if (chainResetTimer < weapon.chainResetDelay) {
            chainResetTimer += Time.deltaTime;
            if (chainResetTimer >= weapon.chainResetDelay) {
                nextChain = 0;
                atkWeaMotion.resetWeapRot = true;
            }
        }
    }

    public void ChainAttacks() {
        charAtk.readyToAtk = false;
        curChain = nextChain;
        chainResetTimer = 0f;
        nextChain++;
        // Debug.Log("Attacking! Chain #" + curChain);
        if (nextChain >= weapon.attackChains.Length) {
            nextChain = 0;
        }
    }
}
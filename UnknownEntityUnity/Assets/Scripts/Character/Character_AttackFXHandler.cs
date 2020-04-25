using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AttackFXHandler : MonoBehaviour
{
    public Character_Attack charAtk;
    public Character_AttackFXPool attackPool;
    public Character_AttackFX atkFX;

    public void GetAttackFX() {
        atkFX = attackPool.RequestAttackFX();
        charAtk.atkFX = atkFX;
    }
}

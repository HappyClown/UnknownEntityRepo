using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon_Motion : MonoBehaviour
{
    // public abstract void ResetWeaponRotation(Transform weapTrans);
    // public abstract void AttackWeaponMotion(float curTimer, Transform weapTrans);
    public abstract void WeaponMotionSetup(Character_Attack _charAtk, Transform _weaponTrans, SpriteRenderer _weaponSpriteR);
    public abstract void StopMotions();
    public abstract void SetupNextMotion();
}

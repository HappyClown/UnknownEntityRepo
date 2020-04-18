using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_MotionController : MonoBehaviour
{
    public List<Weapon_Motion> weaponMotions = new List<Weapon_Motion>();

    public Weapon_Motion CheckMotionList(Weapon_Motion curWeaponMotion) {
        foreach(Weapon_Motion weaponMotion in weaponMotions) {
            if (weaponMotion.GetType() == curWeaponMotion.GetType()) {
                return weaponMotion;
            }
        }
        Weapon_Motion newWeaponMotion = this.gameObject.AddComponent(curWeaponMotion.GetType()) as Weapon_Motion;
        weaponMotions.Add(newWeaponMotion);
        return newWeaponMotion;
    }
}

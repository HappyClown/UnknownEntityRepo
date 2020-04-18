using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon_Motion", menuName = "UnknownEntityUnity/SO_Weapon_MotionFunctionsTest", order = 0)]
public abstract class SO_Weapon_MotionFunctionsTest : ScriptableObject
{
    public abstract void ResetWeaponRotation(Transform weapTrans);
    public abstract void AttackWeaponMotion(float curTimer, Transform weapTrans);
    public abstract void WeaponMotionSetup(float motionDuration);
    public abstract void StopMotions();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Weapon_Motion_Slash", menuName = "SOWeaponMotions/SO_Weapon_Motion_Slash", order = 0)]
public class SO_Weapon_Motion_Slash : SO_Weapon_Motion
{
    public float restingAngle;
    public float waitingForResetAngle;
    public float resetRotDuration;
    public AnimationCurve attackRotAnimCurve;
    public float weaponMotionDuration;
    public float RotationDifference {
        get {
            return Mathf.Abs(restingAngle-waitingForResetAngle);
        }
    }
}

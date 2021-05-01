using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Weapon_Motion_Stab", menuName = "SO_WeaponStuff/SO_WeaponMotions/Weapon_Motion_Stab", order = 0)]
public class SO_Weapon_Motion_Stab : SO_Weapon_Motion
{
    //public float restingYPosition;
    //public float waitingForResetYPos;
    public float resetYPosDuration;
    //public AnimationCurve attackYPosAnimCurve;
    [Header("Weapon motions; Windup, Action, Winddown")]
    public float weaponMotionDuration;
    public float[] motionDurations;
    public float[] yPositions;
    public AnimationCurve[] animCurves;
    public bool[] holdMotions;
    // public float PositionDifference {
    //     get {
    //         return Mathf.Abs(restingYPosition-waitingForResetYPos);
    //     }
    // }
}
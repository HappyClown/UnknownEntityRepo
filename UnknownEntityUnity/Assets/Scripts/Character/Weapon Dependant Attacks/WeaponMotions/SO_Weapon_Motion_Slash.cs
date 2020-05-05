using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Weapon_Motion_Slash", menuName = "SOWeaponMotions/SO_Weapon_Motion_Slash", order = 0)]
public class SO_Weapon_Motion_Slash : SO_Weapon_Motion
{
    //public float restingRotation;
    //public float waitingForResetAngle;
    public float resetRotDuration;
    [Header("Weapon motions; Windup, Action, Winddown")]
    public float[] motionDurations;
    public float[] rotations;
    public AnimationCurve[] animCurves;
    [Header("Direction change")]
    public bool useDirectionChange;
    public bool clockwise;
    // public float RotationDifference {
    //     get {
    //         return Mathf.Abs(restingAngle-waitingForResetAngle);
    //     }
    // }

    //public float restingYPosition;
    //public float waitingForResetYPos;
    //public float resetYPosDuration;
    //public AnimationCurve attackYPosAnimCurve;
    //[Header("Weapon motions; Windup, Action, Winddown")]
    //public float weaponMotionDuration;
    //public float[] motionDurations;
    //public float[] yPositions;
    //public AnimationCurve[] animCurves;
}

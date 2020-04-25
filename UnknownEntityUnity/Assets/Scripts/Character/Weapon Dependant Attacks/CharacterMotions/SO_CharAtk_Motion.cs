﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_CharAtk_Motion", menuName = "SO_CharAtk_Motion", order = 0)]
public class SO_CharAtk_Motion : ScriptableObject
{
    public float[] durations;
    public float[] distances;
    public bool[] lockInputMovement;
    public bool[] lockCharacterAndWeapon;
    public float[] slowDownRunSpeed;
    // Would require some changes but is possible.
    //public Vector2[] postions;
    //public AnimationCurve[] animCurves;
}

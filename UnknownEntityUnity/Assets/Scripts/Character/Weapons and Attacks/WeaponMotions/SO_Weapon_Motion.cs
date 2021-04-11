using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SO_Weapon_Motion : ScriptableObject
{
    public Weapon_Motion weapon_Motion;
    public Vector3 restingPosition;
    public Vector3 restingRotation;
    public int allowAttackAndSwap;
    // At what motion will the camera be nudged.
    public int nudgeCamera;
    public float nudgeDistance;
}

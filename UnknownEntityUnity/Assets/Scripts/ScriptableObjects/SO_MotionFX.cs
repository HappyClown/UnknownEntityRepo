using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_MotionFX", menuName = "SO_FXs/MotionFX", order = 0)]
public class SO_MotionFX : ScriptableObject
{
    public Sprite[] motionFXSprites;
    public float[] motionFXTimings;
}

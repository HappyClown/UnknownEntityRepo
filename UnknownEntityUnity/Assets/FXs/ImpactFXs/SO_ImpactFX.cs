using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_ImpactFX", menuName = "SO_FXs/ImpactFX", order = 0)]
public class SO_ImpactFX : ScriptableObject
{
    public Sprite[] impactFXSprites;
    public float[] impactFXTimings;
}

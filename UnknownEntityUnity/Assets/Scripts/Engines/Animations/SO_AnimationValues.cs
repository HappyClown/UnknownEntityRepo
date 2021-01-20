using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimValues", menuName = "AnimValues")]
public class SO_AnimationValues : ScriptableObject
{
    public float totalDuration;
    public Sprite[] sprites;
    public float[] changeSprites;
    public bool loop;
    public int loopAmnt;
}
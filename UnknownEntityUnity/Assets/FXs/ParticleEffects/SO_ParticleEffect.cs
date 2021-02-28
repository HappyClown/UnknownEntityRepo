using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_ParticleEffect", menuName = "SO_FXs/ParticleEffect", order = 0)]
public class SO_ParticleEffect : ScriptableObject
{
    public ParticleSystem particleSystem;
    public float inUseDuration;
    public Sprite sprite;

}

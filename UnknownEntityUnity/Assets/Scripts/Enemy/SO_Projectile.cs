using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Projectile", menuName = "Projectile")]
public class SO_Projectile : ScriptableObject
{
    public float minDamage, maxDamage;
    public float Damage {
        get {
            return Random.Range(minDamage, maxDamage);
        }
    }    
    public float duration;
    public float speed;
    [Header("Projectile Animation")]
    public bool animated;
    public float animTotalDuration;
    public Sprite[] animSprites;
    public float[] animTimings;
    public AnimationClip animClip; 

    public Sprite sprite;
    public Collider2D col;
    public ContactFilter2D contactFilter;
    public SO_ImpactFX sO_ImpactFX;

}

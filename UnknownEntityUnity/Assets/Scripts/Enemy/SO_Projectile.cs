using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Projectile", menuName = "SO_EnemyStuff/Projectile")]
public class SO_Projectile : ScriptableObject
{
    public float minDamage, maxDamage;
    public float Damage {
        get {
            return Random.Range(minDamage, maxDamage);
        }
    }
    public float duration;
    public float maxSpeed;
    public float spawnDistance;
    public bool destroyOnContact;
    public bool useSpeedCurve;
    public AnimationCurve speedCurve;
    [Header("Projectile Animation")]
    public bool animated;
    public float animTotalDuration;
    public Sprite[] animSprites;
    public float[] animTimings;
    public AnimationClip animClip; 

    [Header("Collider")]
    //public bool activeOnStart;
    public float colStartTime = 0f; 
    public float colEndTime = 99f;
    public Sprite sprite;
    public PolygonCollider2D col;
    public ContactFilter2D contactFilter;
    public SO_ImpactFX sO_ImpactFX;

}

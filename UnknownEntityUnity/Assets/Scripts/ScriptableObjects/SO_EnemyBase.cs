using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class SO_EnemyBase : ScriptableObject
{
    public new string name;
    public float maximumLife;
    public float minDamage, maxDamage;
    public float attackCooldown;
    public Sprite sprite;
    public Sprite spriteIdle;
    public float atkRange;
    public float aggroRange;
    [Header("Movement")]
    public Sprite walkingSprite;
    public Sprite[] walkingSprites;
    public bool lerpTurnRotations;
    public float moveSpeed;
    public float turnSpeed = 3f;
    public float turnDst = 0.5f;
    public bool slowDown = false;
    public float slowDownDist = 10f;
    public float intelligence;
    [Header ("Animation")]
    public Sprite[] animSprites;
    public float[] animEvents;
    public PolygonCollider2D atkCol;
    public int colTimingIndex;
    public UnityEvent[] events;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class SO_EnemyBase : ScriptableObject
{
    public new string name;
    public float maximumLife;
    public float minDamage, maxDamage;
    public float attackCooldown;
    public Sprite sprite;
    public float atkRange;
    public float aggroRange;
    [Header("Movement")]
    public float moveSpeed;
    public float turnSpeed = 3f;
    public float turnDst = 0.5f;
    public float slowDownDist = 10f;
    public bool slowDown = false;
    public float intelligence;
    [Header ("Animation")]
    public Sprite[] animSprites;
    public float[] animTimings;
    public PolygonCollider2D atkCol;
    public int colTimingIndex;
}
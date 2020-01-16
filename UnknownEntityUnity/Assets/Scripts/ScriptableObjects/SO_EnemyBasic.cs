using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class SO_EnemyBasic : ScriptableObject
{
    public new string name;
    public float maximumLife;
    public float attackDamage;
    public float attackCooldown;
    public Sprite sprite;
}
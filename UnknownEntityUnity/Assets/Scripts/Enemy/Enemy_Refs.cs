using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Refs : MonoBehaviour
{
    [Header("Variables States")]
    public Sprite walkingSprite;
    [Header("General Enemy Refs")]
    public Transform plyrTrans;
    public SpriteRenderer eSpriteR;
    [Header("Enemy Scripts")]
    public Unit unit;
    public Enemy_WalkAnim eWalkAnim;
    public Enemy_Aggro eAggro;
    public Enemy_Actions eAction;
    public Enemy_Health eHealth;
    public Enemy_Death eDeath;
    public SO_EnemyBase eSO;

    // Squared distance to player.
    public float SqrDistToPlayer(Vector3 origin) {
        return (plyrTrans.position - origin).sqrMagnitude;
    }
    // Direction normalized to player from given position.
    public Vector2 NormDirToPlayerV2(Vector3 origin) {
        return ((Vector2)plyrTrans.position - (Vector2)origin).normalized;
    }


}

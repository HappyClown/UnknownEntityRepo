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
    public LayerMask losLayerMask;
    [Header("Enemy Scripts")]
    public Enemy_FollowPath eFollowPath;
    public Enemy_WalkAnim eWalkAnim;
    public Enemy_Aggro eAggro;
    public Enemy_Actions eAction;
    public Enemy_Health eHealth;
    public Enemy_Death eDeath;
    public SO_EnemyBase eSO;
    public Vector3 PlayerPos {
        get{
            return plyrTrans.position;
        }
    }

    // Check circle cast line of sight to target or up to distance if specified.
    public RaycastHit2D CircleCastLOSToTarget(Vector3 origin, Vector3 target, float radius, float distance=0) {
        float dist;
        if (distance > 0) {
            dist = distance;
        }
        else {
            dist = DistToTarget(origin, target);
        }
        RaycastHit2D hit = Physics2D.CircleCast(origin, radius, NormDirToTargetV2(origin, target), dist, losLayerMask);
        return hit;
    }
    // Squared distance to target.
    public float SqrDistToTarget(Vector3 origin, Vector3 target) {
        return (target - origin).sqrMagnitude;
    }    
    // Distance to target.
    public float DistToTarget(Vector3 origin, Vector3 target) {
        return (target - origin).magnitude;
    }
    // Direction normalized to player from given position.
    public Vector2 NormDirToTargetV2(Vector3 origin, Vector3 target) {
        return ((Vector2)target - (Vector2)origin).normalized;
    }


}

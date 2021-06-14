using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerTools;

public class Enemy_Refs : MonoBehaviour
{
    [Header("Variables States")]
    public Sprite walkingSprite;
    public Sprite alternateIdleSprite;
    [Header("General Enemy Refs")]
    public Transform playerShadow;
    public Transform playerCenter;
    public SpriteRenderer eSpriteR;
    public LayerMask losLayerMask;
    public SpriteAnim mySpriteAnim;
    public AnimationClip[] animClips;
    public Character_Health charHealth;
    [Header("Enemy Scripts")]
    public Enemy_FollowPath eFollowPath;
    public Enemy_WalkAnim eWalkAnim;
    public Enemy_Aggro eAggro;
    public Enemy_Actions eAction;
    public Enemy_CollisionDetection eCol;
    public Enemy_Health eHealth;
    public Enemy_Death eDeath;
    public Enemy_Drops eDrops;
    public SO_EnemyBase eSO;
    public FlipObjectX flipX;
    public SpriteColorFlash spriteColorFlash;
    public Vector3 PlayerShadowPos {
        get{
            return playerShadow.position;
        }
    }
    public Vector3 PlayerCenterPos {
        get{
            return playerCenter.position;
        }
    }

    // Check circle cast line of sight to target or up to distance if specified.
    public RaycastHit2D CircleCastLOSToTarget(Vector2 origin, Vector2 target, float radius, float distance=0) {
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
    public float SqrDistToTarget(Vector2 origin, Vector2 target) {
        return (target - origin).sqrMagnitude;
    }    
    // Distance to target.
    public float DistToTarget(Vector2 origin, Vector2 target) {
        return (target - origin).magnitude;
    }
    // Direction normalized to player from given position.
    public Vector2 NormDirToTargetV2(Vector2 origin, Vector2 target) {
        return (target - origin).normalized;
    }
}

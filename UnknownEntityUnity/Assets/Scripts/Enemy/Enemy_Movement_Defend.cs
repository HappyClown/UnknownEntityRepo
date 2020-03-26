using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement_Defend : MonoBehaviour
{
    public Enemy_Refs eRefs;
    public List<Enemy_Actions> enemiesToDefend;
    public Enemy_Actions defendee;
    Transform defendeeTrans;
    public Transform defendTokenTrans;
    public float distFromDefendee = 1f;
    public float updateDistDelta = 0.5f, updateFrequency = 0.25f;
    Vector3 newTokenPos;

    // Pick the first undefended enemy in the list and set it as this enemy's defendee.
    void PickEnemyToDefend() {
        defendee = null;
        foreach(Enemy_Actions enemyToDefend in enemiesToDefend) {
            if (!enemyToDefend.defended) {
                enemyToDefend.defended = true;
                defendee = enemyToDefend;
                defendeeTrans = defendee.transform;
                break;
            }
        }
    }

    void AssignTokenToPathTarget() {
        eRefs.eFollowPath.target = defendTokenTrans;
    }

    public void StartDefendingDefendee() {
        if (!defendee) {
            PickEnemyToDefend();
            AssignTokenToPathTarget();
        }
        StartCoroutine(UpdateDefenderTargetPosition());
    }

    IEnumerator UpdateDefenderTargetPosition() {
        //defendTokenTrans.position = GetDistPositionFromPath();
        defendTokenTrans.position = GetDefensePositionCircleCast();
        float updateDistDeltaSqr = updateDistDelta*updateDistDelta;
        Vector3 plyrOldPos = eRefs.PlayerPos;
        Vector3 defendeeOldPos = defendeeTrans.position;
        yield return null;
        while(true) {
            if (eRefs.SqrDistToTarget(plyrOldPos, eRefs.PlayerPos) + (defendeeTrans.position - defendeeOldPos).sqrMagnitude > updateDistDeltaSqr) {
                //defendTokenTrans.position = GetDistPositionFromPath();
                defendTokenTrans.position = GetDefensePositionCircleCast();
                plyrOldPos = eRefs.PlayerPos;
                defendeeOldPos = defendeeTrans.position;
                yield return null;
            }
            yield return new WaitForSeconds(updateFrequency);
        }
    }

    // CircleCast from the defended towards the player with a distance of the defense tightness. If it hits an obstacle adjust the position to x distance from the obstacle hit.
    Vector3 GetDefensePositionCircleCast() {
        Vector2 dirToPlyr = eRefs.NormDirToTargetV2(defendeeTrans.position, eRefs.PlayerPos);
        RaycastHit2D hit = Physics2D.CircleCast(defendeeTrans.position, 0.25f, dirToPlyr, distFromDefendee, eRefs.losLayerMask);
        if (hit) {
            // Decide what the defender does if there is something blocking the way. Try to get the position from path?
            return GetDistPositionFromPath();
        }
        else {
            return (Vector2)defendeeTrans.position + dirToPlyr * distFromDefendee;
        }
    }
    
    // Another way could be to request a simple path(not going through all the adjustments) from the defended enemy to the player and set the defense position to x distance along the path. This would be more accurate because the position would take into account pathing thorugh obstacles and be in the way of the shortest distance a lot better.
    Vector3 GetDistPositionFromPath() {
        return Pathfinding.GetDistAlongPath(defendeeTrans.position, eRefs.PlayerPos, distFromDefendee, eRefs.eSO.intelligence);
    }

    void GoToDefensePosition() {
        // Request a path to the position of the distance along a path from the defendee to the player.
        newTokenPos = Pathfinding.GetDistAlongPath(defendeeTrans.position, eRefs.PlayerPos, distFromDefendee, eRefs.eSO.intelligence);
        eRefs.eFollowPath.RequestPathToTarget(newTokenPos);
    }
}

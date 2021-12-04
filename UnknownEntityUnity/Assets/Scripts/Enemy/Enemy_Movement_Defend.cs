using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement_Defend : MonoBehaviour
{
    [Header("Script References")]
    public Enemy_Refs eRefs;
    public Enemy_Defender eDefender;
    [Header("To-set Variables")]
    public List<Enemy_AllyToDefend> alliesToDefend;
    public Transform defensePosTokenTrans;
    public float distFromAlly = 3f;
    public float updateDistDelta = 0.1f, updateFrequency = 0.1f;
    [Header("Read Only")]
    public Enemy_AllyToDefend allyToDefend;
    public Transform allyTrans;
    Vector3 newTokenPos;

    // Check to see if Im already defending an ally, if so return true, else if im not defending an ally, try to get one, if I can return true, if I cant return false.
    public bool CheckAlly() {
        if (allyTrans != null) {
            return true;
        }
        else {
            PickAllyToDefend();
            if (allyTrans == null) {
                eDefender.canDefend = false;
                return false;
            }
            else {
                return true;
            }
        }
    } 

    // Assign the defense position transform token to the follow path script.
    public void AssignTokenToPathTarget() {
        if (eRefs.eFollowPath.target != defensePosTokenTrans) {
            eRefs.eFollowPath.target = defensePosTokenTrans;
        }
    }

    // Choose any ally to defend. Then start updating the defense position.
    public void StartDefendingAlly() {
        if (!allyToDefend) {
            PickAllyToDefend();
        }
        StartCoroutine(UpdateDefenderTargetPosition());
    }
    
    // Pick the first undefended enemy in the list and set it as this enemy's allyToDefend.
    void PickAllyToDefend() {
        allyToDefend = null;
        foreach(Enemy_AllyToDefend anAllyToDefend in alliesToDefend) {
            if (!anAllyToDefend.defended) {
                anAllyToDefend.defended = true;
                allyToDefend = anAllyToDefend;
                allyTrans = allyToDefend.transform;
                allyToDefend.defender = eDefender;
                break;
            }
        }
    }

    // If the player is closer to the allyToDefend, start chasing the player directly.
    public bool PlayerCloserToAlly() {
        if (eRefs.SqrDistToTarget(allyTrans.position, eRefs.PlayerShadowPos) < eRefs.SqrDistToTarget(allyTrans.position, this.transform.position)) {
            return true;
        }
        return false;
    }

    // Update the defense position.
    IEnumerator UpdateDefenderTargetPosition() {
        // Get the defense position based on allyToDefend and player positions.
        defensePosTokenTrans.position = GetDefensePositionCircleCast();
        float updateDistDeltaSqr = updateDistDelta*updateDistDelta;
        Vector3 plyrOldPos = eRefs.PlayerShadowPos;
        Vector3 allyOldPos = allyTrans.position;
        yield return null;
        // If the player and the allyToDefend together moved more then the update distance, request a new position.
        while(true) {
            if (eRefs.SqrDistToTarget(plyrOldPos, eRefs.PlayerShadowPos) + (allyTrans.position - allyOldPos).sqrMagnitude > updateDistDeltaSqr) {
                defensePosTokenTrans.position = GetDefensePositionCircleCast();
                //print(defensePosTokenTrans.position);
                // Set the position when a position was last requested.
                plyrOldPos = eRefs.PlayerShadowPos;
                allyOldPos = allyTrans.position;
            }
            yield return new WaitForSeconds(updateFrequency);
        }
    }

    // CircleCast from the allyToDefend towards the player with a distance of the defense tightness. If it hits an obstacle adjust the position to x distance from the obstacle hit.
    Vector3 GetDefensePositionCircleCast() {
        Vector2 dirToPlyr = eRefs.NormDirToTargetV2(allyTrans.position, eRefs.PlayerShadowPos);
        RaycastHit2D hit = Physics2D.CircleCast(allyTrans.position, 0.25f, dirToPlyr, distFromAlly, eRefs.losLayerMask);
        if (hit) {
            // If it hits something get the position from the ally along a pth towards the player.
            return GetDistPositionFromPath();
        }
        else {
            // If it does not hit anything. Get the position at the full defense length from the ally towards the player.
            return (Vector2)allyTrans.position + dirToPlyr * distFromAlly;
        }
    }
    
    // Unused.
    Vector3 GetDistPositionFromPath() {
        // Get the position on the path from the allyToDefend towards the player, at a certain distance from the allyToDefend.
        return Pathfinding.GetDistAlongPath(allyTrans.position, eRefs.PlayerShadowPos, distFromAlly, eRefs.eSO.intelligence);
    }

    void GoToDefensePosition() {
        // Request a path to the position of the distance along a path from the allyToDefend to the player.
        newTokenPos = Pathfinding.GetDistAlongPath(allyTrans.position, eRefs.PlayerShadowPos, distFromAlly, eRefs.eSO.intelligence);
        eRefs.eFollowPath.CustomPathRequest(newTokenPos);
    }
}

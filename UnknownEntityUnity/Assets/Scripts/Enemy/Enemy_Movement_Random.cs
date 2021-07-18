using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement_Random : MonoBehaviour
{
    // Setup a random position to move towards. 
    // Put a token at the desired position and set the follow path script's target to the token
    // Fire a circle cast in a random direction > Set the followpathscript target to the center of that circle cast whether it hits something or not > trigger a free move in the followpathscript > if I reach the destination wait for a certain amount of time
    
    public Enemy_Refs eRefs;
    public Transform movementToken;
    public float minDistance, maxDistance;
    float moveDistance;
    public float radius;
    public float minDuration, maxDuration;
    float duration;
    public bool inRandomMovement;
    Vector2 fromPosition;
    public float tetherDistance = 5f;

    public void AssignRandomMoveTarget() {
        inRandomMovement = true;
        moveDistance = Random.Range(minDistance, maxDistance);
        print("ENEMY MOVEMENT: Random move distance: "+ moveDistance);
        duration = Random.Range(minDuration, maxDuration);
        Vector2 targetPos;
        // Get a random direction.
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        print("ENEMY MOVEMENT: Random circle position normalized: "+ randomDir);
        // If im too far from my spawn position, cast the ray from my spawn position instead of my postion.
        if (Vector2.Distance(this.transform.position, eRefs.mySpawnPosition) > tetherDistance) {
            fromPosition = eRefs.mySpawnPosition;
            print("ENEMY MOVEMENT: Move random from spawn position.");
        }
        else {
            fromPosition = this.transform.position;
            print("ENEMY MOVEMENT: Move random from enemy.");
        }
        print("ENEMY MOVEMENT: Move randomly from this position: "+fromPosition);
        RaycastHit2D hit = Physics2D.CircleCast(fromPosition, radius, randomDir, moveDistance, eRefs.losLayerMask);
        Debug.DrawRay(fromPosition, randomDir*moveDistance, Color.cyan, 5f);
        if (hit) {
            targetPos = hit.centroid;
            print("ENEMY MOVEMENT: CircleCast has hit something.");
        }
        else {
            targetPos = fromPosition + randomDir*moveDistance;
            print("ENEMY MOVEMENT: Nothing was hit, moving full distance.");
        }
        movementToken.position = eRefs.aGrid.NodeFromWorldPoint(targetPos).worldPos;
        print("ENEMY MOVEMENT: Moving to this position: " + movementToken.position);
        print("ENEMY MOVEMENT: Actual distance: "+ Vector2.Distance(movementToken.position, this.transform.position));
        eRefs.eFollowPath.allowPathUpdate = true;
        eRefs.eFollowPath.TriggerFreePath();
        eRefs.eFollowPath.target = movementToken;
        StartCoroutine(RandomMoveStateDuration());
    }

    IEnumerator RandomMoveStateDuration() {
        float timer = 0f;
        while (timer < duration) {
            timer += Time.deltaTime;
            yield return null;
        }
        inRandomMovement = false;
    }
}

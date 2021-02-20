using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement_Random : MonoBehaviour
{
    // Setup a random position to move towards. 
    // Put a token at the desired position and set the follow path script's target to the token
    // Fire a circle cast in a random direction > Set the followpathscript target to th ecenter of that circle cast whether it hits something or not > trigger a free move in the followpathscript > if I reach the destination wait for a certain amount of time
    
    public Enemy_Refs eRefs;
    public Transform movementToken;
    public float minDistance, maxDistance;
    float moveDistance;
    public float radius;
    public float minDuration, maxDuration;
    float duration;
    public bool inRandomMovement;

    public void AssignRandomMoveTarget() {
        inRandomMovement = true;
        moveDistance = Random.Range(minDistance, maxDistance);
        duration = Random.Range(minDuration, maxDuration);
        Vector2 targetPos;
        // Get a random direction.
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        print(randomDir);
        RaycastHit2D hit = Physics2D.CircleCast(this.transform.position, radius, randomDir, moveDistance, eRefs.losLayerMask);
        if (hit) {
            targetPos = hit.centroid;
        }
        else {
            targetPos = randomDir*moveDistance;
        }
        movementToken.position = targetPos;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement_Chase : MonoBehaviour
{
    public Enemy_Refs eRefs;
    bool stateStarted;
    public float minDuration, maxDuration;
    float duration;
    public bool inChase;

    // Set movement target to Player.
    public void AssignChaseTarget() {
        inChase = true;
        duration = Random.Range(minDuration, maxDuration);
        eRefs.eFollowPath.allowPathUpdate = true;
        eRefs.eFollowPath.TriggerFreePath();
        if (eRefs.eFollowPath.target != eRefs.playerShadow) {
            eRefs.eFollowPath.target = eRefs.playerShadow;
        }
        StartCoroutine(ChaseTargetDuration());
    }
    IEnumerator ChaseTargetDuration() {
        float timer = 0f;
        while (timer < duration) {
            timer += Time.deltaTime;
            yield return null;
        }
        inChase = false;
    }
}

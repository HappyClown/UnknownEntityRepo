using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Wait : MonoBehaviour
{
    public Enemy_Refs eRefs;
    public float minDuration, maxDuration;
    float duration;
    public bool inWait;

    public void Wait(){
        inWait = true;
        duration = Random.Range(minDuration, maxDuration);
        eRefs.eFollowPath.StopAllMovementCoroutines();
        eRefs.eSpriteR.sprite = eRefs.eSO.spriteIdle;
        StartCoroutine(Waiting());
    }

    IEnumerator Waiting() {
        float timer = 0f;
        while (timer < duration) {
            timer += Time.deltaTime;
            yield return null;
        }
        inWait = false;
    }
}
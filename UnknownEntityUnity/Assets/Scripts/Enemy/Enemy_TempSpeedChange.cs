using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TempSpeedChange : MonoBehaviour
{
    public Enemy_Refs eRefs;
    public float minusSpeed;
    public float speedMult;
    public float duration;

    public void AnimChangeSpeed() {
        StartCoroutine(InSpeedChange());
    }
    IEnumerator InSpeedChange() {
        eRefs.eFollowPath.speedModifier *= speedMult;
        yield return new WaitForSeconds(duration);
        eRefs.eFollowPath.speedModifier *= (1/speedMult);
    }
}

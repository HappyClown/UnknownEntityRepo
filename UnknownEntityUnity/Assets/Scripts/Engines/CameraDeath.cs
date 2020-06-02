using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDeath : MonoBehaviour
{
    public Transform camTrans;
    public Transform plyrTrans;
    public float lerpDuration;
    public bool lerpCamToPlyr;
    public AnimationCurve animCurve;
    private Vector3 startPos, lerpPos;
    private float time;
    private float multOfLerpDuration;


    void Update() {
        if (lerpCamToPlyr) {
            time += Time.deltaTime * multOfLerpDuration;
            lerpPos = Vector3.Lerp(startPos, plyrTrans.position, animCurve.Evaluate(time));
            camTrans.position = new Vector3(lerpPos.x, lerpPos.y, camTrans.position.z);
            if (time >= 1f) {
                lerpCamToPlyr = false;
                this.enabled = false;
            }
        }
    }

    public void DeathCamLerpSetup() {
        this.enabled = true;
        startPos = camTrans.position;
        multOfLerpDuration = 1 / lerpDuration;
        lerpCamToPlyr = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targetTran;
    public MouseInputs moIn;
    public bool useFrameDelay = false;
    public int everyXFrame = 1;
    private int frameAmnt;
    [Range(0, 1)]
    public float camPlayerToMouse;
    private Vector3 dirVector, dirVectorNorm, adjustedVector;
    private float dirVectorMag;

    void LateUpdate()
    {
        if (useFrameDelay) {
            frameAmnt++;
            if (frameAmnt >= everyXFrame) {
                this.transform.position = new Vector3(targetTran.position.x, targetTran.position.y, this.transform.position.z);
                frameAmnt = 1;
            }
        }

        dirVector = moIn.mousePosWorld2D - new Vector2(targetTran.position.x, targetTran.position.y);
        dirVectorMag = dirVector.magnitude;
        dirVectorNorm = dirVector.normalized;
        adjustedVector = dirVectorNorm * (camPlayerToMouse * dirVectorMag);
        this.transform.position = new Vector3(targetTran.position.x, targetTran.position.y, 0f) + new Vector3(adjustedVector.x, adjustedVector.y, this.transform.position.z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTran;
    public MouseInputs moIn;
    [Range(0, 1)]
    public float camPlayerToMouse;
    private Vector3 dirVector, dirVectorNorm, adjustedVector, targetPos;
    private float dirVectorMag;
    // Bool states
    public bool followingPlayer;
    // Adjustments
    //public float smoothTime;
    //public float followSpeed, minFollowSpeed, maxFollowSpeed;
    private Vector3 refVelocity;
    // Nudge
    public float nudgeDuration;
    public AnimationCurve nudgeAnimCurve;
    private float nudgeForce;
    public bool allowNudging;
    // Static variables.
    public static bool allowNudging_St;
    private static Vector3 cameraNudge_St;
    private static float nudgeForce_St;
    private static Vector2 nudgeDirection_St;
    private static bool nudged_St;
    public bool directlyOnPlayer;
    bool inNudge;

    void Start() {
        allowNudging_St = allowNudging;
    }

    void LateUpdate()
    {
        if (followingPlayer) {
            if (nudged_St) {
                nudged_St = false;
                StartCoroutine(Nudge());
            }
            if (directlyOnPlayer) {
                this.transform.position = new Vector3(playerTran.position.x, playerTran.position.y, this.transform.position.z);
            }
            else {
                // Adjustment between mouse and player.
                dirVector = moIn.mousePosWorld2D - new Vector2(playerTran.position.x, playerTran.position.y);
                dirVectorMag = dirVector.magnitude;
                dirVectorNorm = dirVector.normalized;
                adjustedVector = dirVectorNorm * (camPlayerToMouse * dirVectorMag);
                targetPos = new Vector3(playerTran.position.x, playerTran.position.y, 0f) + new Vector3(adjustedVector.x, adjustedVector.y, this.transform.position.z);

                this.transform.position = targetPos;
                this.transform.position += cameraNudge_St;
            }
        }
    }

    public static void CameraNudge_St(Vector3 directionNorm, float force) {
        if (allowNudging_St) {
            nudgeForce_St = force;
            nudgeDirection_St = directionNorm;
            nudged_St = true;
        }
    }

    IEnumerator Nudge () {
        inNudge = true;
        cameraNudge_St = Vector3.zero;
        float nudgeTimer = 0f;
        float realStartTime = Time.time;
        while (nudgeTimer < 1f) {
            //nudgeTimer += Time.deltaTime / nudgeDuration;
            nudgeTimer += (Time.time-realStartTime) / nudgeDuration;
            nudgeForce = nudgeAnimCurve.Evaluate(nudgeTimer) * nudgeForce_St;
            cameraNudge_St = new Vector3(nudgeDirection_St.x, nudgeDirection_St.y, 0f) * nudgeForce;
            yield return null;
        }
        inNudge = false;
    }
}

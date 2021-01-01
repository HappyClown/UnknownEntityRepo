using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBounce : MonoBehaviour
{
    [Header("To set per object part")]
    public int bounces;
    public int minBounces, maxBounces;
    public float bounceHeight;
    private float minBounceHeight, maxBounceHeight;
    public float heightLossMult;
    public AnimationCurve bounceCurve;
    public float moveDist;
    public float slidePercentOfDist;
    public float startSpeed;
    public float radius;
    public ContactFilter2D contactFilter;
    public float rotation;
    [Header("Comes from outside.")]
    public Transform spriteToMoveTrans, spriteToBounceTrans;
    public Vector2 direction;
    [Header("Nothing For Now")]
    public ScriptableObject sOSpriteBounce;
    public float testReduce = 0.01f;
    private float curSpeed = 1f;

    public void StartBounce() {

    }

    IEnumerator BouncingSpriteLerp () {
        yield return new WaitForSeconds(1f);
        // Movement Variables.
        Vector2 curMainPos = spriteToMoveTrans.position;
        Vector2 startPos = spriteToMoveTrans.position;
        Vector2 endPos = startPos + (direction*moveDist);
        //Raycast variables.
        Vector2 prevDirection = direction;
        Vector2 prevHitPos = startPos;
        float distLeft = moveDist;
        float distUsed = 0f;
        bool checkCollisions = true;
        List<Vector2> hitPositions = new List<Vector2>();
        List<float> hitDistPercentAdded = new List<float>();
        List<float> hitDistPercent = new List<float>();
        // Fire reflecting raycasts to determine the object's trajectory.
        while (checkCollisions) {
            RaycastHit2D rayHit = Physics2D.CircleCast(prevHitPos, radius-testReduce, prevDirection, distLeft, contactFilter.layerMask);
            if (rayHit.collider != null) {
                Debug.DrawLine(prevHitPos, rayHit.centroid, Color.red, 5f);
                float distanceToCol = (rayHit.centroid - prevHitPos).magnitude;
                // Calculate the new maximum moveDist left.
                distLeft -= distanceToCol;
                distUsed += distanceToCol;
                prevHitPos = rayHit.centroid;
                // Add the moveDist traveled so far on 1 to a list, to know when to change direction.
                hitDistPercent.Add(distanceToCol / moveDist);
                hitDistPercentAdded.Add(distUsed / moveDist);
                // Store the hit position to know where to move to next.
                hitPositions.Add(rayHit.centroid);
                // Get the reflected direction and fire a raycast with a length of the moveDist left to see if we hit anything else.
                Vector2 reflectDir = Vector2.Reflect(prevDirection, rayHit.normal).normalized; 
                prevDirection = reflectDir;
                testReduce+=testReduce;
            }
            else {
                checkCollisions = false;
                // If the ray dosnt hit anything calculate the last position and add it to the list.
                hitPositions.Add(prevHitPos+(prevDirection*distLeft));
                hitDistPercent.Add(distLeft/moveDist);
                hitDistPercentAdded.Add(1f);
                Debug.DrawLine(prevHitPos, prevHitPos+(prevDirection*distLeft), Color.red, 5f);
            }
            yield return null;
        }
        // Start moving the object.
        bool movementDone = false;
        int curTarget = 0;
        curSpeed = startSpeed;

        // // Set bounce landings distances.
        // List<float> bounceDistances = new List<float>();
        // float bounceLength = moveDist / bounces;
        // for (int i = 1; i < bounces; i++) {
        //     bounceDistances.Add(bounceLength*1);
        // }
        // Bounce variables.
        int bounceCount = 0;
        float bounceTimer = 0f;
        float slideDist = slidePercentOfDist*moveDist;
        print(slideDist);
        float bounceSegDur = ((moveDist-slideDist)/bounces)/curSpeed;
        float yPos = 0f;
        float baseYPos = spriteToBounceTrans.localPosition.y;
        float startY = 0f;
        float curBounceHeight = bounceHeight;
        float endY = curBounceHeight;
        bool bouncing = true;
        // Move the object.
        while (!movementDone) {
            spriteToMoveTrans.position = Vector2.MoveTowards(spriteToMoveTrans.position, hitPositions[curTarget], curSpeed*Time.deltaTime);
            if (((Vector2)spriteToMoveTrans.position-hitPositions[curTarget]).magnitude <= 0.001f) {
                curTarget++;
                if (curTarget>=hitPositions.Count) {
                    movementDone = true;
                }
            }
            // Bounce the object.
            if (bouncing) {
                bounceTimer+=Time.deltaTime/bounceSegDur;
                yPos = Mathf.Lerp(startY, endY, bounceCurve.Evaluate(bounceTimer));
                //print(yPos);
                if (bounceTimer >= 1f || movementDone) {
                    curBounceHeight*=heightLossMult;
                    endY = curBounceHeight;
                    //print(curBounceHeight);
                    bounceSegDur = ((moveDist-slideDist)/bounces)/curSpeed;
                    bounceCount++;
                    bounceTimer = 0f;
                    yPos = 0f;
                    //print(bounceCount);
                    if (bounceCount >= bounces) {
                        bouncing = false;
                        //print(bouncing);
                    }
                }
                spriteToBounceTrans.localPosition = new Vector3(spriteToBounceTrans.localPosition.x, baseYPos+yPos, spriteToBounceTrans.localPosition.z);
            }
            yield return null;
        }
    }
    #region Complicated unfinished slow movement naturally over multiple lerps.
    // Goes in a coroutine.
    //int reflectCounter = 0;
    //float adjustedDur = duration*hitDistPercent[reflectCounter];
    // float curDur = 0f;
    // float timeCheckStart = 0f;
    // float timeCheckEnd = 0f;
    // timeCheckEnd = FindAnimCurveTimeFromValue(hitDistPercent[reflectCounter]);
    // curDur = timeCheckEnd * duration;
    // while (!movementDone) {
    //     moveTimer += Time.deltaTime / curDur;
    //     print("MoveTimer:"+moveTimer);
    //     //timeCheckStart+(moveTime*curDur)
    //     print("Current time value on anim curve: "+(timeCheckStart+(moveTimer*curDur)));
    //     print("Evalute Curve: "+(moveAnimCurve.Evaluate(timeCheckStart+(moveTimer*curDur)))/* /hitDistPercent[reflectCounter] */);
    //     print("Evalute Curve on ONE: "+(moveAnimCurve.Evaluate(timeCheckStart+(moveTimer*curDur))) );
    //     //+" the on AnimCurve value is: "+moveAnimCurve.Evaluate(moveTimer/timeCheck)+" on a total of: "+hitDistPercentAdded[reflectCounter]);
    //     curMainPos = Vector2.Lerp(startPos, endPos, moveAnimCurve.Evaluate(timeCheckStart+(moveTimer*curDur))/hitDistPercent[reflectCounter]);
    //     if (moveTimer >= 1f) {
    //     //if (moveTimer>moveAnimCurve.Evaluate(moveTimer)) {
    //         startPos = hitPositions[reflectCounter];
    //         curMainPos = startPos;
    //         if (reflectCounter < hitPositions.Count-1) {
    //             reflectCounter++;
    //             //adjustedDur = duration*hitDistPercent[reflectCounter];
    //             endPos = hitPositions[reflectCounter];
    //             moveTimer = 0f;
    //             timeCheckStart = FindAnimCurveTimeFromValue(hitDistPercentAdded[reflectCounter-1]);
    //             timeCheckEnd = FindAnimCurveTimeFromValue(hitDistPercentAdded[reflectCounter]);
    //             curDur = (timeCheckEnd-timeCheckStart) * duration;
    //         }
    //         else {
    //             movementDone = true;
    //         }
    //     }
    //     //print(Time.time);
    //     spriteToMoveTrans.position = curMainPos;
    //     yield return null;
    // }

    // bool CheckIfPastTargetCoord(Vector2 targetPos, Vector2 myPos) {
    //     bool youPassedIt = false;
    //     if (targetPos.x < 0) {
    //         if (myPos.x < targetPos.x) {
    //             youPassedIt = true;
    //         }
    //     }
    //     else if (targetPos.x > 0) {
    //         if (myPos.x > targetPos.x) {
    //             youPassedIt = true;
    //         }
    //     }
    //     if (targetPos.y < 0) {
    //         if (myPos.y < targetPos.y) {
    //             youPassedIt = true;
    //         }
    //     }
    //     else if (targetPos.y > 0) {
    //         if (myPos.y > targetPos.y) {
    //             youPassedIt = true;
    //         }
    //     }
    //     if (youPassedIt) {
    //         return true;
    //     }
    //     else {
    //         return false;
    //     }
    // }

    // float FindAnimCurveTimeFromValue(float value, float accuracy = 0.01f) {
    //     print("Value entered for timecheck:"+value);
    //     float timeCheck = 0f;
    //     bool over=false;
    //     bool lastOver=false;
    //     float timeCheckIncrement = 0.5f;
    //     while (Mathf.Abs(moveAnimCurve.Evaluate(timeCheck)-value) > 0.01f) {
    //         timeCheck += timeCheckIncrement;
    //         if (moveAnimCurve.Evaluate(timeCheck) > value) {
    //             over = true;
    //         }
    //         else if (moveAnimCurve.Evaluate(timeCheck) < value) {
    //             over = false;
    //         }
    //         else {
    //             //print("Right on, very unlikely.");
    //         }
    //         if (over != lastOver) {
    //             timeCheckIncrement/=2;
    //             timeCheckIncrement*=-1;
    //         }
    //         lastOver = over;
    //         //print("Time check: "+moveAnimCurve.Evaluate(timeCheck));
    //         //print("timeCheck - goalPercent: "+(moveAnimCurve.Evaluate(timeCheck)-value));
    //     }
    //     print("TimeCheck:"+timeCheck);
    //     return timeCheck;
    // }
    #endregion
}
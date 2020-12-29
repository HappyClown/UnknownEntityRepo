using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_CollisionDetection : MonoBehaviour
{
    [Header ("To-set Variables")]
    public BoxCollider2D boxCol;
    public float skinWidth = 0.005f;
    public int raysPerSeg = 3;
    public LayerMask colMask;
    [Header ("Read Only")]
    Vector3 gizmosMoveDir = Vector3.zero;
    bool hitDetected;
    Vector2[] rayStartPoints = new Vector2[0];
    float gizmosMoveDist, gizmosMoveDistX, gizmosMoveDistY;
    Vector3 myCurPos, myNewPos;
    float halfColX, halfColY, colOffsetX, colOffsetY;
    bool slideChecked = false;

    public Vector3 CollisionCheck(Vector3 normMoveDir, Vector3 newPos, Vector3 curPos) {
        hitDetected = false;
        gizmosMoveDir = normMoveDir;
        myCurPos = curPos;
        myNewPos = newPos;

        float moveDist = (newPos - curPos).magnitude;
        float newX = newPos.x;
        float newY = newPos.y;
        float adjustedX = 0f;
        float adjustedY = 0f;
        gizmosMoveDist = moveDist;
        (adjustedX, adjustedY) = MovementRaycasts(normMoveDir, moveDist);

        if (hitDetected) {
            print("Hitdetected");
            Vector3 adjustedPos = new Vector3 (curPos.x + adjustedX, curPos.y + adjustedY, curPos.z);
            return adjustedPos;
        } 
        else {
            return newPos;
        }
    }

    (float, float) MovementRaycasts(Vector3 normMoveDir, float moveDist) {
        float shortestHitX = 999f;
        float shortestHitY = 999f;
        Vector2 surfaceNormalX = Vector2.zero;
        Vector2 surfaceNormalY = Vector2.zero;

        bool leftHit = false;
        bool rightHit = false;
        bool topHit = false; 
        bool botHit = false;

        float xMoveDist = myNewPos.x-myCurPos.x;
        gizmosMoveDistX = xMoveDist;
        float yMoveDist = myNewPos.y-myCurPos.y;
        gizmosMoveDistY = yMoveDist;
        
        // To be moved to a setup phase function (something like start but when this enemy is aggroed).
        halfColX = boxCol.size.x/2;
        halfColY = boxCol.size.y/2;
        colOffsetX = boxCol.offset.x;
        colOffsetY = boxCol.offset.y;
        //
        float colBoundMinX = (myCurPos.x + colOffsetX) - halfColX;
        float colBoundMaxX = (myCurPos.x + colOffsetX) + halfColX;
        float colBoundMinY = (myCurPos.y + colOffsetY) - halfColY;
        float colBoundMaxY = (myCurPos.y + colOffsetY) + halfColY;

        RaycastHit2D rayHit;
        if (rayStartPoints.Length != raysPerSeg+2) {
            rayStartPoints = new Vector2[raysPerSeg+2];
        }

        // Fire rays into x and y directions seperately based on moveDir.
        // X rays:
        if (normMoveDir.x != 0) {
            // Rays to the left.
            if (normMoveDir.x < 0) {
                Vector2 colTopLeft = new Vector2(colBoundMinX+skinWidth, colBoundMaxY-skinWidth);
                rayStartPoints[0] = colTopLeft;
                
                Vector2 colBotLeft = new Vector2(colBoundMinX+skinWidth, colBoundMinY+skinWidth);
                rayStartPoints[1] = colBotLeft;

                Vector2 segSize = (colTopLeft-colBotLeft) / (raysPerSeg+1);
                for (int i = 1; i < raysPerSeg+1; i++) {
                    Vector2 newRayPoint = colBotLeft + (segSize*i);
                    rayStartPoints[i+1] = newRayPoint;
                }

                foreach(Vector2 rayStartPoint in rayStartPoints) {
                    rayHit = Physics2D.Raycast(rayStartPoint, normMoveDir, (moveDist+skinWidth), colMask);
                    Debug.DrawRay(rayStartPoint, normMoveDir*(moveDist+skinWidth), Color.white, Time.deltaTime);
                    if (rayHit) {
                        hitDetected = true;
                        leftHit = true;
                        if (Mathf.Abs((rayHit.point - rayStartPoint).x) < Mathf.Abs(shortestHitX)) {
                            shortestHitX = (rayHit.point - rayStartPoint).x;
                            surfaceNormalX = rayHit.normal;
                        }
                    }
                }
            }
            // Rays to the right.
            else if (normMoveDir.x > 0) {
                Vector2 colTopRight = new Vector2(colBoundMaxX-skinWidth, colBoundMaxY-skinWidth);
                rayStartPoints[0] = colTopRight;

                Vector2 colBotRight = new Vector2(colBoundMaxX-skinWidth, colBoundMinY+skinWidth);
                rayStartPoints[1] = colBotRight;

                Vector2 segSize = (colTopRight-colBotRight) / (raysPerSeg+1);
                for (int i = 1; i < raysPerSeg+1; i++) {
                    Vector2 newRayPoint = colBotRight + (segSize*i);
                    rayStartPoints[i+1] = newRayPoint;
                }

                foreach(Vector2 rayStartPoint in rayStartPoints) {
                    rayHit = Physics2D.Raycast(rayStartPoint, normMoveDir, moveDist+skinWidth, colMask);
                    Debug.DrawRay(rayStartPoint, normMoveDir*(moveDist+skinWidth), Color.white, Time.deltaTime);
                    if (rayHit) {
                        hitDetected = true;
                        rightHit = true;
                        if (Mathf.Abs((rayHit.point - rayStartPoint).x) < Mathf.Abs(shortestHitX)) {
                            shortestHitX = (rayHit.point - rayStartPoint).x;
                            surfaceNormalX = rayHit.normal;
                        }
                    }
                }
            }
        }
        // Y rays:
        if (normMoveDir.y != 0) {
            // Rays to the top.
            if (normMoveDir.y > 0) {
                Vector2 colTopLeft = new Vector2(colBoundMinX+skinWidth, colBoundMaxY-skinWidth);
                rayStartPoints[0] = colTopLeft;

                Vector2 colTopRight = new Vector2(colBoundMaxX-skinWidth, colBoundMaxY-skinWidth);
                rayStartPoints[1] = colTopRight;

                Vector2 segSize = (colTopRight-colTopLeft) / (raysPerSeg+1);
                for (int i = 1; i < raysPerSeg+1; i++) {
                    Vector2 newRayPoint = colTopLeft + (segSize*i);
                    rayStartPoints[i+1] = newRayPoint;
                }

                foreach(Vector2 rayStartPoint in rayStartPoints) {
                    rayHit = Physics2D.Raycast(rayStartPoint, normMoveDir, moveDist+skinWidth, colMask);
                    Debug.DrawRay(rayStartPoint, normMoveDir*(moveDist+skinWidth), Color.white, Time.deltaTime);
                    if (rayHit) {
                        hitDetected = true;
                        topHit = true;
                        if (Mathf.Abs((rayHit.point - rayStartPoint).y) < Mathf.Abs(shortestHitY)) {
                            shortestHitY = (rayHit.point - rayStartPoint).y;
                            surfaceNormalY = rayHit.normal;
                        }
                    }
                }
            }
            // Rays to the bottom.
            else if (normMoveDir.y < 0) {
                Vector2 colBotLeft = new Vector2(colBoundMinX+skinWidth, colBoundMinY+skinWidth);
                rayStartPoints[0] = colBotLeft;

                Vector2 colBotRight = new Vector2(colBoundMaxX-skinWidth, colBoundMinY+skinWidth);
                rayStartPoints[1] = colBotRight;

                Vector2 segSize = (colBotRight-colBotLeft) / (raysPerSeg+1);
                for (int i = 1; i < raysPerSeg+1; i++) {
                    Vector2 newRayPoint = colBotLeft + (segSize*i);
                    rayStartPoints[i+1] = newRayPoint;
                }

                foreach(Vector2 rayStartPoint in rayStartPoints) {
                    rayHit = Physics2D.Raycast(rayStartPoint, normMoveDir, (moveDist+skinWidth), colMask);
                    Debug.DrawRay(rayStartPoint, normMoveDir*(moveDist+skinWidth), Color.white, Time.deltaTime);
                    if (rayHit) {
                        hitDetected = true;
                        botHit = true;
                        if (Mathf.Abs((rayHit.point - rayStartPoint).y) < Mathf.Abs(shortestHitY)) {
                            shortestHitY = (rayHit.point - rayStartPoint).y;
                            surfaceNormalY = rayHit.normal;
                        }
                    }
                }
            }
        }

        float modSkinWidthX = skinWidth;
        float modSkinWidthY = skinWidth;
        float finalMoveDistX = 0f;
        float finalMoveDistY = 0f;

        if (leftHit) {
            finalMoveDistX = shortestHitX+modSkinWidthX;
        }
        else if (rightHit) {
            modSkinWidthX *= (-1f);
            finalMoveDistX = shortestHitX+modSkinWidthX;
        }
        else {
            finalMoveDistX = xMoveDist;
        }
        if (topHit) {
            modSkinWidthY *= (-1f);
            finalMoveDistY = shortestHitY+modSkinWidthY;
        }
        else if (botHit) {
            finalMoveDistY = shortestHitY+modSkinWidthY;
        }
        else {
            finalMoveDistY = yMoveDist;
        }
        // SLIDING CHECK //
        float slideMoveDistX = 0f;
        float slideMoveDistY = 0f; 
        print(slideChecked);
        if (hitDetected && !slideChecked) {
            slideChecked = true;
            print("Before slide check final move distances X: "+finalMoveDistX+" and Y: "+finalMoveDistY);
            if (Mathf.Abs(finalMoveDistX) <= 0.0049f || Mathf.Abs(finalMoveDistY) <= 0.0049f) {
                // CALCULATE SLIDE VALUES //
                // This should be done if the returned final value is 0 (or less then 0.0001).
                print("Final Move Dist X: "+finalMoveDistX);
                print("Final Move Dist Y: "+finalMoveDistY);
                // Get the shortest hit's collision surface normal.
                Vector2 surfaceNormal;
                if (shortestHitX <= shortestHitY) {
                    surfaceNormal = surfaceNormalX;
                }
                else {
                    surfaceNormal = surfaceNormalY;
                }
                print("Surface Normal: "+surfaceNormal);
                if (surfaceNormal == ((Vector2)normMoveDir*-1)) {
                    //print("The hit surface normal counters the players movement perfectionado.");
                    // Stop here.
                    return (finalMoveDistX, finalMoveDistY);
                }
                if (surfaceNormal+(Vector2)normMoveDir == Vector2.zero) {
                    //print("The hit surface normal counters the players movement wellionado.");
                    // Stop here.
                    return (finalMoveDistX, finalMoveDistY);
                }
                Vector2 surfaceVectorClockwise;
                Vector2 surfaceVectorCounterClockwise;
                // Clockwise rotation of the surface normal to find the surface vector.
                surfaceVectorClockwise = new Vector2(surfaceNormal.y, surfaceNormal.x*-1);
                print("Surface Vector clockwise: "+ surfaceVectorClockwise);
                // Counter-Clockwise.
                surfaceVectorCounterClockwise = new Vector2(surfaceNormal.y * -1, surfaceNormal.x);
                print("Surface Vector counter-clockwise: "+ surfaceVectorCounterClockwise);

                // Chose the one with the smallest angle between the normMoveDir's reflection and the clockwise or counterclockwise vectors.
                // Reflection vector from the move direction and the surface normal.
                Vector2 surfaceReflect;
                surfaceReflect = Vector2.Reflect(normMoveDir, surfaceNormal);
                print("Surface Reflection: "+surfaceReflect);

                float angleClockwise;
                float angleCounterClockwise;
                angleClockwise = Vector2.Angle(surfaceReflect, surfaceVectorClockwise);
                angleCounterClockwise = Vector2.Angle(surfaceReflect, surfaceVectorCounterClockwise);
                print("Angle between reflection and clockwise: "+angleClockwise);
                print("Angle between reflection and counter-clockwise: "+angleCounterClockwise);
                Vector2 slideDirection;
                if (angleClockwise < angleCounterClockwise) {
                    slideDirection = surfaceVectorClockwise;
                }
                else {
                    slideDirection = surfaceVectorCounterClockwise;
                }
                print("Chosen slide direction vector: "+slideDirection);
                // Calculate the loss of speed based on the angle between the move direction normal and the surface normal on 90.
                float angleMoveDir;
                float speedAdjustment;
                angleMoveDir = Vector2.Angle(normMoveDir*-1, surfaceNormal);
                print("Angle between move direction and surface normal: "+angleMoveDir);
                speedAdjustment = 1-((90-angleMoveDir) / 90);
                print("Speed adjustment multiplier: "+speedAdjustment);

                float slideMoveDist = moveDist*speedAdjustment;
                print("Slide move distance: "+slideMoveDist);
                // Fire the rays again in the new direction to see if sliding is possible.
                Vector2 newSlidePos = (Vector2)myCurPos + (slideDirection*slideMoveDist);
                //CollisionCheck(slideDirection, newSlidePos, myCurPos);
                (slideMoveDistX, slideMoveDistY) = MovementSlideRaycasts(slideDirection, slideMoveDist, newSlidePos, new Vector2(finalMoveDistX, finalMoveDistY));
                print("Slide move XY distances; slideMoveDistX: "+slideMoveDistX+" then; slideMoveDistY: "+slideMoveDistY);
                // RAYS FIRE AGAIN //
            }
        }
        slideChecked = false;
        return (finalMoveDistX+slideMoveDistX, finalMoveDistY+slideMoveDistY);
    }




    (float, float) MovementSlideRaycasts(Vector3 normMoveDir, float moveDist, Vector3 newSlidePos, Vector2 finalMoveDist) {
        Vector2 curPosSlide = (Vector2)myCurPos+finalMoveDist;
        Vector3 newPosSlide = newSlidePos;

        float shortestHitX = 999f;
        float shortestHitY = 999f;
        Vector2 surfaceNormalX = Vector2.zero;
        Vector2 surfaceNormalY = Vector2.zero;

        bool leftHit = false;
        bool rightHit = false;
        bool topHit = false; 
        bool botHit = false;

        float xMoveDist = newPosSlide.x-curPosSlide.x;
        float yMoveDist = newPosSlide.y-curPosSlide.y;
        
        // To be moved to a setup phase function (something like start but when this enemy is aggroed).
        halfColX = boxCol.size.x/2;
        halfColY = boxCol.size.y/2;
        colOffsetX = boxCol.offset.x;
        colOffsetY = boxCol.offset.y;
        //
        float colBoundMinX = (curPosSlide.x + colOffsetX) - halfColX;
        float colBoundMaxX = (curPosSlide.x + colOffsetX) + halfColX;
        float colBoundMinY = (curPosSlide.y + colOffsetY) - halfColY;
        float colBoundMaxY = (curPosSlide.y + colOffsetY) + halfColY;

        RaycastHit2D rayHit;
        if (rayStartPoints.Length != raysPerSeg+2) {
            rayStartPoints = new Vector2[raysPerSeg+2];
        }

        // Fire rays into x and y directions seperately based on moveDir.
        // X rays:
        if (normMoveDir.x != 0) {
            // Rays to the left.
            if (normMoveDir.x < 0) {
                Vector2 colTopLeft = new Vector2(colBoundMinX+skinWidth, colBoundMaxY-skinWidth);
                rayStartPoints[0] = colTopLeft;
                
                Vector2 colBotLeft = new Vector2(colBoundMinX+skinWidth, colBoundMinY+skinWidth);
                rayStartPoints[1] = colBotLeft;

                Vector2 segSize = (colTopLeft-colBotLeft) / (raysPerSeg+1);
                for (int i = 1; i < raysPerSeg+1; i++) {
                    Vector2 newRayPoint = colBotLeft + (segSize*i);
                    rayStartPoints[i+1] = newRayPoint;
                }

                foreach(Vector2 rayStartPoint in rayStartPoints) {
                    rayHit = Physics2D.Raycast(rayStartPoint, normMoveDir, (moveDist+skinWidth), colMask);
                    Debug.DrawRay(rayStartPoint, normMoveDir*(moveDist+skinWidth), Color.white, Time.deltaTime);
                    if (rayHit) {
                        hitDetected = true;
                        leftHit = true;
                        if (Mathf.Abs((rayHit.point - rayStartPoint).x) < Mathf.Abs(shortestHitX)) {
                            shortestHitX = (rayHit.point - rayStartPoint).x;
                            surfaceNormalX = rayHit.normal;
                        }
                    }
                }
            }
            // Rays to the right.
            else if (normMoveDir.x > 0) {
                Vector2 colTopRight = new Vector2(colBoundMaxX-skinWidth, colBoundMaxY-skinWidth);
                rayStartPoints[0] = colTopRight;

                Vector2 colBotRight = new Vector2(colBoundMaxX-skinWidth, colBoundMinY+skinWidth);
                rayStartPoints[1] = colBotRight;

                Vector2 segSize = (colTopRight-colBotRight) / (raysPerSeg+1);
                for (int i = 1; i < raysPerSeg+1; i++) {
                    Vector2 newRayPoint = colBotRight + (segSize*i);
                    rayStartPoints[i+1] = newRayPoint;
                }

                foreach(Vector2 rayStartPoint in rayStartPoints) {
                    rayHit = Physics2D.Raycast(rayStartPoint, normMoveDir, moveDist+skinWidth, colMask);
                    Debug.DrawRay(rayStartPoint, normMoveDir*(moveDist+skinWidth), Color.white, Time.deltaTime);
                    if (rayHit) {
                        hitDetected = true;
                        rightHit = true;
                        if (Mathf.Abs((rayHit.point - rayStartPoint).x) < Mathf.Abs(shortestHitX)) {
                            shortestHitX = (rayHit.point - rayStartPoint).x;
                            surfaceNormalX = rayHit.normal;
                        }
                    }
                }
            }
        }
        // Y rays:
        if (normMoveDir.y != 0) {
            // Rays to the top.
            if (normMoveDir.y > 0) {
                Vector2 colTopLeft = new Vector2(colBoundMinX+skinWidth, colBoundMaxY-skinWidth);
                rayStartPoints[0] = colTopLeft;

                Vector2 colTopRight = new Vector2(colBoundMaxX-skinWidth, colBoundMaxY-skinWidth);
                rayStartPoints[1] = colTopRight;

                Vector2 segSize = (colTopRight-colTopLeft) / (raysPerSeg+1);
                for (int i = 1; i < raysPerSeg+1; i++) {
                    Vector2 newRayPoint = colTopLeft + (segSize*i);
                    rayStartPoints[i+1] = newRayPoint;
                }

                foreach(Vector2 rayStartPoint in rayStartPoints) {
                    rayHit = Physics2D.Raycast(rayStartPoint, normMoveDir, moveDist+skinWidth, colMask);
                    Debug.DrawRay(rayStartPoint, normMoveDir*(moveDist+skinWidth), Color.white, Time.deltaTime);
                    if (rayHit) {
                        hitDetected = true;
                        topHit = true;
                        if (Mathf.Abs((rayHit.point - rayStartPoint).y) < Mathf.Abs(shortestHitY)) {
                            shortestHitY = (rayHit.point - rayStartPoint).y;
                            surfaceNormalY = rayHit.normal;
                        }
                    }
                }
            }
            // Rays to the bottom.
            else if (normMoveDir.y < 0) {
                Vector2 colBotLeft = new Vector2(colBoundMinX+skinWidth, colBoundMinY+skinWidth);
                rayStartPoints[0] = colBotLeft;

                Vector2 colBotRight = new Vector2(colBoundMaxX-skinWidth, colBoundMinY+skinWidth);
                rayStartPoints[1] = colBotRight;

                Vector2 segSize = (colBotRight-colBotLeft) / (raysPerSeg+1);
                for (int i = 1; i < raysPerSeg+1; i++) {
                    Vector2 newRayPoint = colBotLeft + (segSize*i);
                    rayStartPoints[i+1] = newRayPoint;
                }

                foreach(Vector2 rayStartPoint in rayStartPoints) {
                    rayHit = Physics2D.Raycast(rayStartPoint, normMoveDir, (moveDist+skinWidth), colMask);
                    Debug.DrawRay(rayStartPoint, normMoveDir*(moveDist+skinWidth), Color.white, Time.deltaTime);
                    if (rayHit) {
                        hitDetected = true;
                        botHit = true;
                        if (Mathf.Abs((rayHit.point - rayStartPoint).y) < Mathf.Abs(shortestHitY)) {
                            shortestHitY = (rayHit.point - rayStartPoint).y;
                            surfaceNormalY = rayHit.normal;
                        }
                    }
                }
            }
        }

        float modSkinWidthX = skinWidth;
        float modSkinWidthY = skinWidth;
        float finalMoveDistX = 0f;
        float finalMoveDistY = 0f;
        print("In Slide Collision method, Hit Left: "+leftHit+" Hit Right: "+rightHit);

        if (leftHit) {
            finalMoveDistX = shortestHitX+modSkinWidthX;
        }
        else if (rightHit) {
            modSkinWidthX *= (-1f);
            finalMoveDistX = shortestHitX+modSkinWidthX;
        }
        else {
            finalMoveDistX = xMoveDist;
        }
        if (topHit) {
            modSkinWidthY *= (-1f);
            finalMoveDistY = shortestHitY+modSkinWidthY;
        }
        else if (botHit) {
            finalMoveDistY = shortestHitY+modSkinWidthY;
        }
        else {
            finalMoveDistY = yMoveDist;
        }
        print("In Slide Collision method final slide move distances X: "+finalMoveDistX+" and Y: "+finalMoveDistY);
        return (finalMoveDistX, finalMoveDistY);
    }
}
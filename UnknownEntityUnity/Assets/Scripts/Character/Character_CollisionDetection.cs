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

    public Vector3 CollisionCheck(Vector3 normMoveDir, Vector3 newPos, Vector3 curPos) {
        hitDetected = false;
        gizmosMoveDir = normMoveDir;
        myCurPos = curPos;
        myNewPos = newPos;
        //print("BOX COLLIDER POSITION: "+boxCol.transform.position);
        //print("CURRENT POS: "+"("+curPos.x+", "+curPos.y+", "+curPos.z+")");
        float moveDist = (newPos - curPos).magnitude;
        //print(moveDist);
        float newX = newPos.x;
        float newY = newPos.y;
        float adjustedX = 0f;
        float adjustedY = 0f;
        gizmosMoveDist = moveDist;
        (adjustedX, adjustedY) = MovementRaycasts(normMoveDir, moveDist);
        //print ("ADJUSTED X: "+adjustedX+", ADJUSTED Y: "+adjustedY);
        // float finalX = normMoveDir.x * adjustedX;
        // float finalY = normMoveDir.y * adjustedY;
        // print ("FINAL X: "+finalX+", FINAL Y: "+finalY);

        if (hitDetected) {
            print("Hitdetected");
            Vector3 adjustedPos = new Vector3 (curPos.x + adjustedX, curPos.y + adjustedY, curPos.z);
            //print("POS ADJUSTED: "+"("+adjustedPos.x+", "+adjustedPos.y+", "+adjustedPos.z+")");
            return adjustedPos;
        } 
        else {
            //print("POS NEWPOS: "+"("+newPos.x+", "+newPos.y+", "+newPos.z+")");
            return newPos;
        }
    }

    (float, float) MovementRaycasts(Vector3 normMoveDir, float moveDist) {
        float shortestHitX = 999f;
        float shortestHitY = 999f;
        //bool xHit = false;
        //bool yHit = false;
        bool leftHit = false;
        bool rightHit = false;
        bool topHit = false; 
        bool botHit = false;
        // float xMoveDist = Mathf.Abs((normMoveDir * moveDist).x);
        // gizmosMoveDistX = xMoveDist;
        // float yMoveDist = Mathf.Abs((normMoveDir * moveDist).y);
        // gizmosMoveDistY = yMoveDist;
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
                    rayHit = Physics2D.Raycast(rayStartPoint, Vector2.right, xMoveDist+skinWidth, colMask);
                    Debug.DrawLine(rayStartPoint, rayStartPoint+(Vector2.right*(xMoveDist+skinWidth)), Color.white, Time.deltaTime);
                    if (rayHit) {
                        hitDetected = true;
                        //xHit = true;
                        leftHit = true;
                        if (Mathf.Abs((rayHit.point - rayStartPoint).x) < Mathf.Abs(shortestHitX)) {
                            shortestHitX = (rayHit.point - rayStartPoint).x;
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
                    rayHit = Physics2D.Raycast(rayStartPoint, Vector2.right, xMoveDist+skinWidth, colMask);
                    Debug.DrawLine(rayStartPoint, rayStartPoint+(Vector2.right*(xMoveDist+skinWidth)), Color.white, Time.deltaTime);
                    if (rayHit) {
                        hitDetected = true;
                        //xHit = true;
                        rightHit = true;
                        if (Mathf.Abs((rayHit.point - rayStartPoint).x) < Mathf.Abs(shortestHitX)) {
                            shortestHitX = (rayHit.point - rayStartPoint).x;
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
                    rayHit = Physics2D.Raycast(rayStartPoint, Vector2.up, yMoveDist+skinWidth, colMask);
                    Debug.DrawLine(rayStartPoint, rayStartPoint+(Vector2.up*(yMoveDist+skinWidth)), Color.white, Time.deltaTime);
                    if (rayHit) {
                        hitDetected = true;
                        //yHit = true;
                        topHit = true;
                        if (Mathf.Abs((rayHit.point - rayStartPoint).y) < Mathf.Abs(shortestHitY)) {
                            shortestHitY = (rayHit.point - rayStartPoint).y;
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
                    rayHit = Physics2D.Raycast(rayStartPoint, Vector2.up, yMoveDist+skinWidth, colMask);
                    Debug.DrawLine(rayStartPoint, rayStartPoint+(Vector2.up*(yMoveDist+skinWidth)), Color.white, Time.deltaTime);
                    if (rayHit) {
                        hitDetected = true;
                        //yHit = true;
                        botHit = true;
                        if (Mathf.Abs((rayHit.point - rayStartPoint).y) < Mathf.Abs(shortestHitY)) {
                            shortestHitY = (rayHit.point - rayStartPoint).y;
                        }
                    }
                }
            }
            // // Check diagonals.
            // // Top Left.
            // if (normMoveDir.x < 0 && normMoveDir.y > 0) {
            //     float diagonalMoveDist = xMoveDist+yMoveDist/2;
            //     float diagonalSkinWidth = Mathf.Sqrt((skinWidth*skinWidth)*2);
                
            //     Vector2 colDiagonalTopLeft = new Vector2(colBoundMinX+skinWidth, colBoundMaxY-skinWidth);
                
            //     rayHit = Physics2D.Raycast(colDiagonalTopLeft, new Vector2(-1, 1), diagonalMoveDist+diagonalSkinWidth, colMask);
                
            //     //Debug.DrawLine(colDiagonalTopLeft, colDiagonalTopLeft+(new Vector2(-1, 1)*1), Color.white, 100f);
            // }
            // // Top Right.
            // else if (normMoveDir.x > 0 && normMoveDir.y > 0) {

            // }
            // // Bottom Left.
            // else if (normMoveDir.x < 0 && normMoveDir.y < 0) {

            // }
            // // Bottom Right.
            // else if (normMoveDir.x > 0 && normMoveDir.y < 0) {

            // }

        }
        float modSkinWidthX = skinWidth;
        float modSkinWidthY = skinWidth;
        //float diagonalSkinWidths = Mathf.Sqrt((skinWidth*skinWidth)*2);
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
        return (finalMoveDistX, finalMoveDistY);

        // if (!xHit && !yHit) {
        //     return (xMoveDist, yMoveDist);
        // }
        // else if (xHit && yHit) {
        //     if (shortestHitX > 0) { modSkinWidthX *= (-1f); }
        //     else if (shortestHitX == 0) { modSkinWidthX = 0; }
        //     if (shortestHitY > 0) { modSkinWidthY *= (-1f); }
        //     else if (shortestHitY == 0) { modSkinWidthY = 0; }
        //     print("Result on the XY: " + shortestHitX+modSkinWidthX + " " + shortestHitY+modSkinWidthY);
        //     //print(new Vector2(shortestHitX, shortestHitY));
        //     return (shortestHitX+modSkinWidthX, shortestHitY+modSkinWidthY);
        // }
        // else if (xHit && !yHit) { 
        //     // Hitting a collision on the right side. Substract the skinwidth.
        //     if (shortestHitX > 0) { modSkinWidthX *= (-1f); } 
        //     else if (shortestHitX == 0) { modSkinWidthX = 0; }
        //     print("Result on the X: " + shortestHitX+modSkinWidthX);
        //     //print(new Vector2(shortestHitX/* +modSkinWidthX */, yMoveDist));
        //     //print(new Vector2(shortestHitX+modSkinWidthX, yMoveDist));
        //     return (shortestHitX+modSkinWidthX, yMoveDist);
        // }
        // else if (!xHit && yHit) {
        //     if (shortestHitY > 0) { modSkinWidthY *= (-1f); }
        //     else if (shortestHitY == 0) { modSkinWidthY = 0; }
        //     print("Result on the Y: " + shortestHitY+modSkinWidthY);
        //     //print("ONLY Y HIT: "+(shortestHitY+modSkinWidthX)+", shortest hit on Y.");
        //     return (xMoveDist, shortestHitY+modSkinWidthY);
        // }
        // else {
        //     //print("Returned else. Not sure when this happens...");
        //     return (xMoveDist, yMoveDist);
        // }
    }

    /*void OnDrawGizmos() {
        Vector3 thisPos = this.transform.position;
        Vector3 castPos = thisPos + gizmosMoveDir;
        float xDiff = castPos.x-thisPos.x;
        float yDiff = castPos.y-thisPos.y;
        Vector3 normMoveDir = gizmosMoveDir;
    
        if (!hitDetected) { Gizmos.color = Color.cyan; } else { Gizmos.color = Color.red; }

        // X rays:
        if (normMoveDir.x != 0) {
            // Rays to the left.
            if (normMoveDir.x < 0) {
                Vector2 colTopLeft = new Vector2(boxCol.bounds.min.x+skinWidth, boxCol.bounds.max.y-skinWidth);
                Gizmos.DrawRay(colTopLeft, Vector3.left*Mathf.Abs(xDiff));
                Vector2 colBotLeft = new Vector2(boxCol.bounds.min.x+skinWidth, boxCol.bounds.min.y+skinWidth);
                Gizmos.DrawRay(colBotLeft, Vector3.left*Mathf.Abs(xDiff));
                
                Vector2 segSize = (colTopLeft-colBotLeft) / (raysPerSeg+1);
                for (int i = 1; i < raysPerSeg+1; i++) {
                    Vector2 newRayPoint = colBotLeft + (segSize*i);
                    Gizmos.DrawRay(newRayPoint, Vector3.left*(gizmosMoveDistX+skinWidth));
                }
            }
            // Rays to the right.
            else if (normMoveDir.x > 0) {
                Vector2 colTopRight = new Vector2(boxCol.bounds.max.x-skinWidth, boxCol.bounds.max.y-skinWidth);
                Gizmos.DrawRay(colTopRight, Vector3.right*Mathf.Abs(xDiff));
                Vector2 colBotRight = new Vector2(boxCol.bounds.max.x-skinWidth, boxCol.bounds.min.y+skinWidth);
                Gizmos.DrawRay(colBotRight, Vector3.right*Mathf.Abs(xDiff));
                
                Vector2 segSize = (colTopRight-colBotRight) / (raysPerSeg+1);
                for (int i = 1; i < raysPerSeg+1; i++) {
                    Vector2 newRayPoint = colBotRight + (segSize*i);
                    Gizmos.DrawRay(newRayPoint, Vector3.right*(gizmosMoveDistX+skinWidth));
                }
            }
        }
        // Y rays:
        if (normMoveDir.y != 0) {
            // Rays to the top.
            if (normMoveDir.y > 0) {
                Vector2 colTopLeft = new Vector2(boxCol.bounds.min.x+skinWidth, boxCol.bounds.max.y-skinWidth);
                Gizmos.DrawRay(colTopLeft, Vector3.up*Mathf.Abs(yDiff));
                Vector2 colTopRight = new Vector2(boxCol.bounds.max.x-skinWidth, boxCol.bounds.max.y-skinWidth);
                Gizmos.DrawRay(colTopRight, Vector3.up*Mathf.Abs(yDiff));

                Vector2 segSize = (colTopRight-colTopLeft) / (raysPerSeg+1);
                for (int i = 1; i < raysPerSeg+1; i++) {
                    Vector2 newRayPoint = colTopLeft + (segSize*i);
                    Gizmos.DrawRay(newRayPoint, Vector3.up*(gizmosMoveDistY+skinWidth));
                }
            }
            // Rays to the bottom.
            else if (normMoveDir.y < 0) {
                Vector2 colBotLeft = new Vector2(boxCol.bounds.min.x+skinWidth, boxCol.bounds.min.y+skinWidth);
                Gizmos.DrawRay(colBotLeft, Vector3.down*Mathf.Abs(yDiff));
                Vector2 colBotRight = new Vector2(boxCol.bounds.max.x-skinWidth, boxCol.bounds.min.y+skinWidth);
                Gizmos.DrawRay(colBotRight, Vector3.down*Mathf.Abs(yDiff));

                Vector2 segSize = (colBotRight-colBotLeft) / (raysPerSeg+1);
                for (int i = 1; i < raysPerSeg+1; i++) {
                    Vector2 newRayPoint = colBotLeft + (segSize*i);
                    Gizmos.DrawRay(newRayPoint, Vector3.down*(gizmosMoveDistY+skinWidth));
                }
            }
        }
    }*/
}
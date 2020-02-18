using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_CollisionDetection : MonoBehaviour
{
    public BoxCollider2D boxCol;
    public LayerMask colMask;
    Vector3 gizmosMoveDir = Vector3.zero;
    bool hitDetected;
    Vector2[] rayStartPoints = new Vector2[0];
    public float skinWidth;
    public int raysPerSeg;

    public Vector3 CollisionCheck(Vector3 normMoveDir, Vector3 newPos, Vector3 curPos)
    {
        hitDetected = false;
        gizmosMoveDir = normMoveDir;

        float moveDist = (newPos - curPos).magnitude;

        float newX = newPos.x;
        float newY = newPos.y;
        float adjustedX = 0f;
        float adjustedY = 0f;

        (adjustedX, adjustedY) = MovementRaycasts(normMoveDir, moveDist);
        adjustedX = normMoveDir.x * adjustedX;
        adjustedY = normMoveDir.y * adjustedY;

        if (hitDetected) {
            Vector3 adjustedPos = new Vector3 (curPos.x + adjustedX, curPos.y + adjustedY, newPos.z);
            return adjustedPos;
        } 
        else {
            return newPos;
        }
    }

    (float, float) MovementRaycasts(Vector3 normMoveDir, float moveDist) {
        float shortestHitX = 999f;
        float shortestHitY = 999f;
        bool xHit = false;
        bool yHit = false;
        float xMoveDist = Mathf.Abs((normMoveDir * moveDist).x);
        float yMoveDist = Mathf.Abs((normMoveDir * moveDist).y);

        RaycastHit2D rayHit;
        if (rayStartPoints.Length != raysPerSeg+2) {
            rayStartPoints = new Vector2[raysPerSeg+2];
        }

        // Fire rays into x and y directions seperately based on moveDir.
        // X rays:
        if (normMoveDir.x != 0) {
            // Rays to the left.
            if (normMoveDir.x < 0) {
                Vector2 colTopLeft = new Vector2(boxCol.bounds.min.x+skinWidth, boxCol.bounds.max.y-skinWidth);
                rayStartPoints[0] = colTopLeft;
                
                Vector2 colBotLeft = new Vector2(boxCol.bounds.min.x+skinWidth, boxCol.bounds.min.y+skinWidth);
                rayStartPoints[1] = colBotLeft;

                Vector2 segSize = (colTopLeft-colBotLeft) / (raysPerSeg+1);
                for (int i = 1; i < raysPerSeg+1; i++) {
                    Vector2 newRayPoint = colBotLeft + (segSize*i);
                    rayStartPoints[i+1] = newRayPoint;
                }

                foreach(Vector2 rayStartPoint in rayStartPoints) {
                    rayHit = Physics2D.Raycast(rayStartPoint, Vector2.left, moveDist+skinWidth, colMask);
                    if (rayHit) {
                        hitDetected = true;
                        xHit = true;
                        if (Mathf.Abs((rayHit.point - rayStartPoint).x) < shortestHitX) {
                            shortestHitX = Mathf.Abs((rayHit.point - rayStartPoint).x);
                        }
                    }
                }
            }
            // Rays to the right.
            else if (normMoveDir.x > 0) {
                Vector2 colTopRight = new Vector2(boxCol.bounds.max.x-skinWidth, boxCol.bounds.max.y-skinWidth);
                rayStartPoints[0] = colTopRight;

                Vector2 colBotRight = new Vector2(boxCol.bounds.max.x-skinWidth, boxCol.bounds.min.y+skinWidth);
                rayStartPoints[1] = colBotRight;

                Vector2 segSize = (colTopRight-colBotRight) / (raysPerSeg+1);
                for (int i = 1; i < raysPerSeg+1; i++) {
                    Vector2 newRayPoint = colBotRight + (segSize*i);
                    rayStartPoints[i+1] = newRayPoint;
                }

                foreach(Vector2 rayStartPoint in rayStartPoints) {
                    rayHit = Physics2D.Raycast(rayStartPoint, Vector3.right, moveDist+skinWidth, colMask);
                    if (rayHit) {
                        hitDetected = true;
                        xHit = true;
                        if (Mathf.Abs((rayHit.point - rayStartPoint).x) < shortestHitX) {
                            shortestHitX = Mathf.Abs((rayHit.point - rayStartPoint).x);
                        }
                    }
                }
            }
        }
        // Y rays:
        if (normMoveDir.y != 0) {
            // Rays to the top.
            if (normMoveDir.y > 0) {
                Vector2 colTopLeft = new Vector2(boxCol.bounds.min.x+skinWidth, boxCol.bounds.max.y-skinWidth);
                rayStartPoints[0] = colTopLeft;

                Vector2 colTopRight = new Vector2(boxCol.bounds.max.x-skinWidth, boxCol.bounds.max.y-skinWidth);
                rayStartPoints[1] = colTopRight;

                Vector2 segSize = (colTopRight-colTopLeft) / (raysPerSeg+1);
                for (int i = 1; i < raysPerSeg+1; i++) {
                    Vector2 newRayPoint = colTopLeft + (segSize*i);
                    rayStartPoints[i+1] = newRayPoint;
                }

                foreach(Vector2 rayStartPoint in rayStartPoints) {
                    rayHit = Physics2D.Raycast(rayStartPoint, Vector3.up, moveDist+skinWidth, colMask);
                    if (rayHit) {
                        hitDetected = true;
                        yHit = true;
                        if (Mathf.Abs((rayHit.point - rayStartPoint).y) < shortestHitY) {
                            shortestHitY = Mathf.Abs((rayHit.point - rayStartPoint).y);
                        }
                    }
                }
            }
            // Rays to the bottom.
            else if (normMoveDir.y < 0) {
                Vector2 colBotLeft = new Vector2(boxCol.bounds.min.x+skinWidth, boxCol.bounds.min.y+skinWidth);
                rayStartPoints[0] = colBotLeft;

                Vector2 colBotRight = new Vector2(boxCol.bounds.max.x-skinWidth, boxCol.bounds.min.y+skinWidth);
                rayStartPoints[1] = colBotRight;

                Vector2 segSize = (colBotRight-colBotLeft) / (raysPerSeg+1);
                for (int i = 1; i < raysPerSeg+1; i++) {
                    Vector2 newRayPoint = colBotLeft + (segSize*i);
                    rayStartPoints[i+1] = newRayPoint;
                }

                foreach(Vector2 rayStartPoint in rayStartPoints) {
                    rayHit = Physics2D.Raycast(rayStartPoint, Vector3.down, moveDist+skinWidth, colMask);
                    if (rayHit) {
                        hitDetected = true;
                        yHit = true;
                        if (Mathf.Abs((rayHit.point - rayStartPoint).y) < shortestHitY) {
                            shortestHitY = Mathf.Abs((rayHit.point - rayStartPoint).y);
                        }
                    }
                }
            }
        }

        if (!xHit && !yHit) {
            return (xMoveDist, yMoveDist);
        }
        else if (xHit && yHit) {
            return (shortestHitX-skinWidth, shortestHitY-skinWidth);
        }
        else if (xHit && !yHit) {
            return (shortestHitX-skinWidth, yMoveDist);
        }
        else if (!xHit && yHit) {
            return (xMoveDist, shortestHitY-skinWidth);
        }
        else {
            print("Returned else. Not sure when this happens...");
            return (xMoveDist, yMoveDist);
        }
    }

    void OnDrawGizmos()
    {
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
                    Gizmos.DrawRay(newRayPoint, Vector3.left*Mathf.Abs(xDiff));
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
                    Gizmos.DrawRay(newRayPoint, Vector3.right*Mathf.Abs(xDiff));
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
                    Gizmos.DrawRay(newRayPoint, Vector3.up*Mathf.Abs(yDiff));
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
                    Gizmos.DrawRay(newRayPoint, Vector3.down*Mathf.Abs(yDiff));
                }
            }
        }
    }
}

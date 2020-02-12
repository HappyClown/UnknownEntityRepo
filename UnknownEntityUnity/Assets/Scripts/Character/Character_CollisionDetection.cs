using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_CollisionDetection : MonoBehaviour
{
    public Collider2D col;
    public BoxCollider2D boxCol;
    public ContactFilter2D castFilter;
    public LayerMask colMask;
    Vector3 gizmosMoveDir = Vector3.zero;
    bool hitDetected;
    bool adjustPos;
    public Vector3 CollisionCheck(Vector3 moveDir, float moveSpeed, Vector3 newPos, Vector3 curPos)
    {
        hitDetected = false;
        adjustPos = false;
        gizmosMoveDir = moveDir;

        float newX = newPos.x;
        float newY = newPos.y;
        float smallestX = 999f;
        float smallestY = 999f;
        float adjustedX = 0f;
        float adjustedY = 0f;


        //RaycastHit2D[] hits = new RaycastHit2D[4];
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        boxCol.Cast(moveDir, castFilter, hits, moveDir.magnitude);
        //hits[0] = Physics2D.Raycast(thisPos, moveDir, moveDir.magnitude, colMask);
        if (hits.Count > 0) {
            foreach (RaycastHit2D hit in hits)
            {
                print(hit.collider.name);

                if (Mathf.Abs(hit.point.x - curPos.x) < smallestX) {
                    smallestX = Mathf.Abs(hit.point.x - curPos.x);
                }
                if (Mathf.Abs(hit.point.y - curPos.y) < smallestY) {
                    smallestY = Mathf.Abs(hit.point.y - curPos.y);
                }
                Debug.DrawLine(curPos, hit.point, Color.green, 1);
            }
            hitDetected = true;
        }
        // Check to see if the movement is bigger then the distance to the obstacle on both axis.
        if (Mathf.Abs(newPos.x - curPos.x) > smallestX) {
            adjustedX = smallestX;
            adjustPos = true;
        }
        if (Mathf.Abs(newPos.y - curPos.y) > smallestY) {
            adjustedY = smallestY;
            adjustPos = true;
        }
        if (adjustPos) {
            Vector3 adjustedPos = new Vector3 (curPos.x + adjustedX, curPos.y + adjustedY, newPos.z);
            return adjustedPos;
        } else {
            return newPos;
        }
    }

    void OnDrawGizmos()
    {
        Vector3 thisPos = this.transform.position;

        Vector3 colTopLeft = new Vector3(boxCol.bounds.min.x, boxCol.bounds.max.y, 0);
        Vector3 colTopRight = new Vector3(boxCol.bounds.max.x, boxCol.bounds.max.y, 0);
        Vector3 colBotRight = new Vector3(boxCol.bounds.max.x, boxCol.bounds.min.y, 0);
        Vector3 colBotLeft = new Vector3(boxCol.bounds.min.x, boxCol.bounds.min.y, 0);

        float width = boxCol.bounds.max.x - boxCol.bounds.min.x;
        float height = boxCol.bounds.max.y - boxCol.bounds.min.y;

        Vector3 castPos = thisPos + gizmosMoveDir;

        float xDiff = castPos.x-thisPos.x;
        float yDiff = castPos.y-thisPos.y;

        Vector3 gizTopLeft = new Vector3(boxCol.bounds.min.x+xDiff, boxCol.bounds.max.y+yDiff, 0);
        Vector3 gizTopRight = new Vector3(boxCol.bounds.max.x+xDiff, boxCol.bounds.max.y+yDiff, 0);
        Vector3 gizBotRight = new Vector3(boxCol.bounds.max.x+xDiff, boxCol.bounds.min.y+yDiff, 0);
        Vector3 gizBotLeft = new Vector3(boxCol.bounds.min.x+xDiff, boxCol.bounds.min.y+yDiff, 0);
        if (!hitDetected) { Gizmos.color = Color.cyan; } else { Gizmos.color = Color.red; }

        Gizmos.DrawLine(colTopLeft, gizTopLeft);
        Gizmos.DrawLine(colTopRight, gizTopRight);
        Gizmos.DrawLine(colBotRight, gizBotRight);
        Gizmos.DrawLine(colBotLeft, gizBotLeft);

        Vector3 size = new Vector3(width, height, 1);

        Gizmos.DrawCube(castPos, size);
    }
}

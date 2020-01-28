using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleColliderGizmos : MonoBehaviour
{
    public bool drawCircleColGizmos;
    public Color circleColor;
    public CircleCollider2D myCircleCol;

    private void OnDrawGizmos() {
        if (drawCircleColGizmos){
            Gizmos.color = circleColor;
            Gizmos.DrawWireSphere(myVector2Pos + myCircleCol.offset, myCircleCol.radius);
        }
    }

    Vector2 myVector2Pos {
        get {
            return new Vector2(this.transform.position.x, this.transform.position.y);
        }
    }
}
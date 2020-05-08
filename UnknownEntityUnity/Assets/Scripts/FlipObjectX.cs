using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipObjectX : MonoBehaviour
{
    public Transform transToFlip;
    public Vector3 lastPos, curPos;
    float scaleX;

    public void Flip() {
        curPos = transToFlip.position;
        // Walking to the right.
        if (curPos.x > lastPos.x) {
            transToFlip.localScale = new Vector3(1, transToFlip.localScale.y, transToFlip.localScale.z);
        }
        // Walking to the left.
        else if (curPos.x < lastPos.x){
            transToFlip.localScale = new Vector3(-1, transToFlip.localScale.y, transToFlip.localScale.z);
        }
        lastPos = transToFlip.position;
    }
    public void PredictFlip(Vector3 curPos, Vector3 nextPos) {
        // Going to the left.
        if (curPos.x > nextPos.x) {
            transToFlip.localScale = new Vector3(-1, transToFlip.localScale.y, transToFlip.localScale.z);
        }
        // Going to the Right.
        else if (curPos.x < nextPos.x){
            transToFlip.localScale = new Vector3(1, transToFlip.localScale.y, transToFlip.localScale.z);
        }
    }
}
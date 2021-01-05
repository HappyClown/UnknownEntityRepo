using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLookAt : MonoBehaviour
{
    public MouseInputs moIn;
    public Transform pointerArrow;
    public bool lookAtEnabled = true;
    public bool lookAtPointerArrow = true;

    void Update()
    {
        if (lookAtEnabled) {
            if (lookAtPointerArrow) {
                this.transform.up = pointerArrow.up;
            }
            else {
                this.transform.up = moIn.mousePosWorld2D - new Vector2(this.transform.position.x,this.transform.position.y);
            }
        }
    }

    public void ForceLookAtUpdate() {
        if (lookAtPointerArrow) {
            this.transform.up = pointerArrow.up;
        }
        else {
            this.transform.up = moIn.mousePosWorld2D - new Vector2(this.transform.position.x,this.transform.position.y);
        }
    }
}
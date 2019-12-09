using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLookAt : MonoBehaviour
{
    public MouseInputs moIn;
    void Update()
    {
        this.transform.up = moIn.mousePosWorld2D - new Vector2(this.transform.position.x,this.transform.position.y);
    }
}
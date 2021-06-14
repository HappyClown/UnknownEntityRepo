using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testmat : MonoBehaviour
{
    [Range(0,1)]
    public float matValue;
    public SpriteRenderer spriteR;
    public bool assignValue;

    public void Test () {
        spriteR.material.SetFloat("_OverlayValue", matValue);
    }

    void Update() {
        if (assignValue) {
            Test();
        }    
    }

}

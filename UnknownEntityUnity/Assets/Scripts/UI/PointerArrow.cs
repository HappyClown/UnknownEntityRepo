using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerArrow : MonoBehaviour
{
    public MouseInputs moIn;
    public Transform playerTran;
    
    void Update()
    {
        // Pointer position
        this.transform.position = new Vector3 (moIn.mousePosWorld2D.x, moIn.mousePosWorld2D.y, this.transform.position.z);
        // Pointer rotation
        this.transform.up = moIn.mousePosWorld2D - new Vector2(playerTran.position.x, playerTran.position.y);
    }
}

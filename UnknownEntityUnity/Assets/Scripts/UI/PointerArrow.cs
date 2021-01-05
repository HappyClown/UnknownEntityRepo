using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerArrow : MonoBehaviour
{
    public MouseInputs moIn;
    public Transform playerTran;
    public Transform camTran;
    private Vector3 camLocalPos, camPos, adjustment;
    [Range(0f, 1f)]
    public float followScreenPosOrPlayerPos = 0;
    
    void Update()
    {
        camLocalPos = camTran.localPosition;
        camPos = camTran.position;
        adjustment = (camPos-playerTran.position)*followScreenPosOrPlayerPos;
        // Pointer position
        this.transform.position = new Vector3(moIn.mousePosWorld2D.x-adjustment.x, moIn.mousePosWorld2D.y-adjustment.y, this.transform.position.z);
        //camera pos - player pos
        // Pointer rotation
        this.transform.up = (moIn.mousePosWorld2D-(Vector2)adjustment) - new Vector2(playerTran.position.x, playerTran.position.y);
    }
}
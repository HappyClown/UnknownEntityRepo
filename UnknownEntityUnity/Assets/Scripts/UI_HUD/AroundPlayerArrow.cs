using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AroundPlayerArrow : MonoBehaviour
{
    public MouseInputs moIn;
    [Range(0.5f, 5f)]
    public float distanceFromPlayer;
    public Transform aroundPlayerArrowTransform;
    public SpriteRenderer aroundPlayerArrowSpriteR;
    Color lastColor;
    public Color arrowColor = Color.white;
    
    void Update() {
        if (arrowColor != lastColor) {
            aroundPlayerArrowSpriteR.color = arrowColor;
            lastColor = arrowColor;
        }
        aroundPlayerArrowTransform.localPosition = new Vector2(0f, distanceFromPlayer);
        this.transform.up = moIn.mousePosWorld2D - new Vector2(this.transform.position.x,this.transform.position.y);
        //this.transform.LookAt(moIn.mousePosWorld2D, Vector2.up); 
    }
}

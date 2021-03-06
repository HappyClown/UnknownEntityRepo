using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AroundPlayerArrow : MonoBehaviour
{
    public bool onPlayer;
    public MouseInputs moIn;
    [Range(0.5f, 5f)]
    public float distanceFromPlayer;
    public Transform aroundPlayerArrowTransform;
    public SpriteRenderer aroundPlayerArrowSpriteR;
    Color lastColor;
    public Color arrowColor = Color.white;
    public Transform arrowOriginTrans;
    Vector2 vectToMouse;
    float distFromPlayer;
    [Range(0f,1f)]
    public float percentPlayerToMouse;

    
    void Update() {
        if (onPlayer) {
            if (arrowColor != lastColor) {
                aroundPlayerArrowSpriteR.color = arrowColor;
                lastColor = arrowColor;
            }
            aroundPlayerArrowTransform.localPosition = new Vector2(0f, distanceFromPlayer);
        }
        else {
            vectToMouse = moIn.mousePosWorld2D - (Vector2)arrowOriginTrans.position;
            distFromPlayer = vectToMouse.magnitude * percentPlayerToMouse;
            aroundPlayerArrowTransform.localPosition = new Vector2(0f, distFromPlayer);
        }
        this.transform.up = moIn.mousePosWorld2D - new Vector2(this.transform.position.x,this.transform.position.y);
    }
}
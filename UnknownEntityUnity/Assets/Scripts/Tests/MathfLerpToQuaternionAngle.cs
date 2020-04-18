using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathfLerpToQuaternionAngle : MonoBehaviour
{

    public float startAngle, endAngle, duration;
    public AnimationCurve animationCurve;
    
    public float timer, rotationValue;

    void Update()
    {
        timer+=Time.deltaTime/duration;
        rotationValue = Mathf.Lerp(startAngle, endAngle, animationCurve.Evaluate(timer));
        this.transform.rotation = Quaternion.Euler(0, 0, rotationValue);
        if (timer >= 1f) {
            timer = 0f;
        }
    }
}
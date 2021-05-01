using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public UnityEngine.Experimental.Rendering.Universal.Light2D theLight;
    public bool flickering;
    public float minIntensity, maxIntensity;
    private float intensity;
    public float minOuterRadius, maxOuterRadius;
    private float outerRadius;
    public float minFlickerTime, maxFlickerTime;
    private float flickerTime;
    private float timer;

    void Update()
    {
        if (flickering) {
            timer += Time.deltaTime;
            if (timer > flickerTime) {
                timer = 0f;
                theLight.intensity = intensity;
                theLight.pointLightOuterRadius = outerRadius;
                intensity = Random.Range(minIntensity, maxIntensity);
                outerRadius = Random.Range(minOuterRadius, maxOuterRadius);
                flickerTime = Random.Range(minFlickerTime, maxFlickerTime);
            }
        }
    }
}

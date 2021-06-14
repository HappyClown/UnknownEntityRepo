using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ColorFlash : MonoBehaviour
{
    public float matValue;
    public SpriteRenderer spriteR;
    public float flashDuration;

    public void PlayColorFlash () {
        StartCoroutine(ColorFlash());
    }

    IEnumerator ColorFlash() {
        float timer = 0f;
        spriteR.material.SetFloat("_OverlayValue", matValue);
        while (timer <= flashDuration) {
            timer += Time.deltaTime;
            yield return null;
        }
        spriteR.material.SetFloat("_OverlayValue", 0f);
    }
}

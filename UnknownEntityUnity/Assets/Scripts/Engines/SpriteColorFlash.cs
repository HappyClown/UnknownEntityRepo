using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteColorFlash : MonoBehaviour
{
    public float flashDuration;

    public void PlayColorFlash (SpriteRenderer spriteR /*, SpriteRenderer[] spriteRs */) {
        StartCoroutine(ColorFlash(spriteR));
    }

    IEnumerator ColorFlash(SpriteRenderer spriteR/* SpriteRenderer[] spriteRs */) {
        float timer = 0f;
        // foreach (SpriteRenderer spriteR in spriteRs) {
        //     spriteR.material.SetFloat("_OverlayValue", 1f);
        // }
        spriteR.material.SetFloat("_OverlayValue", 1f);
        while (timer <= flashDuration) {
            timer += Time.deltaTime;
            yield return null;
        }
        // foreach (SpriteRenderer spriteR in spriteRs) {
        //     spriteR.material.SetFloat("_OverlayValue", 0f);
        // }
        spriteR.material.SetFloat("_OverlayValue", 0f);
    }
}

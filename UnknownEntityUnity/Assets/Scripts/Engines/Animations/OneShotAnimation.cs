using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotAnimation : MonoBehaviour
{
    public SpriteRenderer spriteR;

    public void StartAnimation(SO_AnimationValues sOAnimValues) {
        StartCoroutine(PlayAnimation(sOAnimValues));
    }

    IEnumerator PlayAnimation(SO_AnimationValues sOAV) {
        float timer = 0f;
        int spriteCount = 0;
        int loopCount = 0;
        bool playing = true;
        while (playing) {
            timer += Time.deltaTime;
            if (timer > sOAV.changeSprites[spriteCount]) {
                spriteR.sprite = sOAV.sprites[spriteCount];
                if (spriteCount < sOAV.changeSprites.Length-1) {
                    spriteCount++;
                }
            }
            if (timer > sOAV.totalDuration) {
                if (sOAV.loop) {
                    if (sOAV.loopAmnt > 0 ) {
                        loopCount++;
                        if (loopCount >= sOAV.loopAmnt) {
                            playing = false;
                        }
                    }
                    else {
                        timer = 0f;
                        spriteCount = 0;
                    }
                }
                else {
                    playing = false;
                }
            }
            yield return null;
        }
    }
}
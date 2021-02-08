using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotAnimation : MonoBehaviour
{
    public SpriteRenderer spriteR;
    [Header("Assign Only For Tests")]
    public bool startOnAwake = true;
    public SO_AnimationValues sOAnimTemp;
    public InputMaster input;

    void Awake() {
            input = new InputMaster();
            input.Player.Enable();
            if (startOnAwake) { StartAnimation(sOAnimTemp); }
    }
    void Update() {
        if (input.Player.Interact.triggered) {
            StartAnimation(sOAnimTemp);
        }
    }

    public void StartAnimation(SO_AnimationValues sOAnimValues) {
        StartCoroutine(PlayAnimation(sOAnimValues));
    }

    IEnumerator PlayAnimation(SO_AnimationValues sOAV) {
        float timer = 0f;
        int spriteCount = 0;
        int loopCount = 0;
        bool playing = true;
        spriteR.color = sOAV.color;
        yield return new WaitForSeconds(sOAV.Cooldown);
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
                            spriteR.sprite = null;
                            playing = false;
                        }
                    }
                    timer = 0f;
                    spriteCount = 0;
                    if (sOAV.maxCooldown > 0) {
                        if (sOAV.spriteOnCooldown) {
                            spriteR.sprite = sOAV.cooldownSprite;
                        }
                        else {
                            spriteR.sprite = null;
                        }
                        yield return new WaitForSeconds(sOAV.Cooldown);
                    }
                }
                else {
                    spriteR.sprite = null;
                    playing = false;
                }
            }
            yield return null;
        }
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_WalkAnim : MonoBehaviour
{
    public Enemy_Refs eRefs;
    public float cycleDuration;
    public float animSpeedMult = 1f;
    private float spriteDuration;
    private float timer;
    private int spriteNumber = 0;
    private int spriteCount;
    private SO_EnemyBase eSO;
    private SpriteRenderer spriteR;
    private Sprite[] currentWalkCycle;
    Vector2 curPos, lastPos;

    void Start() {
        // Set references, to be transfered out of Start() into a function triggered when the enemy is setup.
        eSO = eRefs.eSO;
        spriteR = eRefs.eSpriteR;
        spriteCount = eRefs.eSO.walkingSprites.Length;
        spriteDuration = cycleDuration/spriteCount;
        timer = spriteDuration;
        currentWalkCycle = eSO.walkingSprites;
    }

    // void Update() {
        // if (eRefs.eFollowPath.followingPath || eRefs.eFollowPath.directlyMovingtoTarget) {            
        //     // Cycle through the run cycle sprites.
        //     timer += Time.deltaTime / cycleDuration * speedAdjustment;
        //     if (timer > spriteDuration) {
        //         // Next sprite
        //         spriteR.sprite = eSO.walkingSprites[spriteNumber];
        //         spriteNumber++;
        //         if (spriteNumber >= spriteCount) {
        //             spriteNumber = 0;
        //         }
        //         timer = 0f;
        //     }
        // }
        // else {
        //     if (spriteR.sprite != eSO.walkingSprites[0]) {
        //         spriteR.sprite = eSO.walkingSprites[0];
        //         timer = spriteDuration;
        //         spriteNumber = 0;
        //     }
        // }
    // }

    // void Update() {
    //     curPos = this.transform.position;
    //     //print(curPos);
    //     if (curPos != lastPos) {
    //         timer += Time.deltaTime * animSpeedMult;
    //         if (timer > spriteDuration) {
    //             // Next sprite
    //             spriteR.sprite = currentWalkCycle[spriteNumber];
    //             spriteNumber++;
    //             if (spriteNumber >= spriteCount) {
    //                 spriteNumber = 0;
    //             }
    //             timer = 0f;
    //         }
    //     }
    //     else {
    //         //timer = 0f;
    //     }
    // }
    // void LateUpdate() {
    //     lastPos = this.transform.position;
    //     //print(lastPos);
    // }

    public void UpdateWalkCycleAnim() {            
        // Cycle through the run cycle sprites.
        timer += Time.deltaTime /* / cycleDuration  */* animSpeedMult;
        if (timer > spriteDuration) {
            // Next sprite
            spriteR.sprite = currentWalkCycle[spriteNumber];
            spriteNumber++;
            if (spriteNumber >= spriteCount) {
                spriteNumber = 0;
            }
            timer = 0f;
        }
    }

    public void ChangeCycleAnimation(Sprite[] newWalkCycle) {
        currentWalkCycle = newWalkCycle;
    }
}

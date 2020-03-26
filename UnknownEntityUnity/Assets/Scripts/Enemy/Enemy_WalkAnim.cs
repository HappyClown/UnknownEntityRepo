using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_WalkAnim : MonoBehaviour
{
    public Enemy_Refs eRefs;
    void Update() {
        if (eRefs.eFollowPath.followingPath) {
            eRefs.eSpriteR.sprite = eRefs.walkingSprite;
        }
    }
}

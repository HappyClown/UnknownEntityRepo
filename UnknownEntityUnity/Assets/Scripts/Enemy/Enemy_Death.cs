using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Death : MonoBehaviour
{
    public Enemy_Refs eRefs;
    // Death sprite anim to be put in a scriptable object or in animation form.
    public Sprite deadSprite;

    // Or just destroy the game object now and instatiate a sprite for the death anim. Destroying the game object will need to be done at some point.
    public void DeathSequence() {
        // Close or stop all active scripts (might be avoided by creating a state machine).
        eRefs.eFollowPath.followingPath = false;
        eRefs.eFollowPath.allowPathUpdate = false;
        eRefs.eAction.StopActions();
        eRefs.eAction.enabled = false;
        StopEnemyCoroutines();
        // Play death animation.
        eRefs.eSpriteR.sprite = deadSprite;
    }
    void StopEnemyCoroutines () {
        eRefs.eFollowPath.StopAllCoroutines();
        eRefs.eAggro.StopAllCoroutines();
    }
}

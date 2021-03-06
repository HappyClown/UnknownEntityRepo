using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_SpriteAnimEvents : MonoBehaviour
{
    public Character_Movement charMove;
    public Character_Health charHealth;

    public void AnimStartForwardRunLoop() {
        charMove.mySpriteAnim.Play(charMove.forwardLoop);
    }
    public void AnimStartBackRunLoop() {
        charMove.mySpriteAnim.Play(charMove.backLoop);
    }
    public void AnimPlayDustFX() {
        //charMove.SetupRunFX();
    }
    public void AnimTakeHitEnd() {
        charHealth.StopTakeHit();
    }
}

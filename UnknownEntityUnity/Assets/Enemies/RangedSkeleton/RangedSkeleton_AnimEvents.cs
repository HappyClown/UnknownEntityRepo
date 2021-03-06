using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSkeleton_AnimEvents : MonoBehaviour
{
    public RangedSkeleton_Bow rsBow;
    public RangedSkeleton_ThrowProjectile rsThrowProj;
    
    public void AnimStopRotatingBow() {
        rsBow.rotateBow = false;
    }
    public void AnimSetupProjectile() {
        rsBow.SetupProjectile();
    }
    public void AnimEndBowAttack(){
        rsBow.bowSpriteAnim.Stop();
        // Set sprite back to the default.
        rsBow.bowSpriteR.sprite = rsBow.defaultBowSprite;
        // Set rotation back to zero.
        rsBow.bowAndArrowGameObject.transform.eulerAngles = Vector3.zero;
        rsThrowProj.ProjectileAttackDone();
        //print("Bow attack's last frame");
    }

    //public void AnimStartChargeArrow() {
        //rsBow.chargeArrowSpriteAnim.Play(rsBow.chargeArrowAnimClip);
    //}    
    //public void AnimStopArrowCharge() {
        //rsBow.chargeArrowSpriteAnim.Stop();
        //rsBow.arrowSpriteR.sprite = null;
    //}
}

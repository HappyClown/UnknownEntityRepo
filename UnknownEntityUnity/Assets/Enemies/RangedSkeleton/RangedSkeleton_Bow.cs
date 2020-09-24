using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSkeleton_Bow : MonoBehaviour
{
    public Enemy_Refs eRefs;
    public FlipObjectX flip;
    public RangedSkeleton_ThrowProjectile throwProj;
    [Header("Skeleton")]
    public Sprite shootingStance;
    [Header("Bow Variables")]
    public GameObject bowGameObject;
    public SpriteRenderer bowSpriteR;
    public Sprite defaultBowSprite;
    //public Sprite bowSprite;
    private bool rotateBow;
    private Vector2 bowDirToTarget;
    [Header("Bow Shooting Animation")]
    public float bowTotalDuration;
    public Sprite[] bowSprites;
    public float[] bowSpriteChanges;
    public float[] bowEvents;
    [Header("Arrow Variables")]
    public SpriteRenderer arrowSpriteR;
    //public Sprite arrowSprite;
    public GameObject arrowGameObject;
    [Header("Arrow Charge Animation")]
    public float arrowTotalDuration;
    public Sprite[] arrowSprites;
    public float[] arrowSpriteChanges;
    public float[] arrowEvents;
    [Header("Projectile")]
    public SO_Projectile projSO;
    public ProjectilePool projPool;
    private Projectile proj;
    private Vector2 projDirNorm;
    // public Transform projOriginTrans; // projOriginTrans = the same as the arrow charge game object transform

    void Info()
    {
        //flip sprite to face target during rotation
        //smooth rotate bow arms towards target as the animation winds up
        //use event to stop the rotation following the player and the sprite flip, used to put a delay between the shot fired and the enemy aiming at the player so that the aim is slightly delayed (easy to dodge just by walking)
        //try to make a second anim loop for the arrow charge up then turn it off and spawn the projectile at the same location/orientation
    }

    public void StopAndReset () {
        // Stops all coroutines.
        this.StopAllCoroutines();
        // Restore walking bow. Sprite and rotation.
        bowSpriteR.sprite = defaultBowSprite;
        bowGameObject.transform.eulerAngles = Vector3.zero;
        // Turn off charging projectile.
        arrowSpriteR.sprite = null;
    }

    public void SetupProjectile() { // transfer into the bow anim script
        proj = projPool.RequestProjectile();
        // using the bow direction to the target for now as this will probably make the arrow paralele to the bow > target direction, just offset
        proj.LaunchProjectile(projSO, bowDirToTarget, arrowGameObject.transform.position);
    }

    // Orient the bow towards the target.
    public void AdjustBowOrientation () {
        //the bow transform.up = the normdirtotargetV2
        //make the initial rotation smooth? sometimes things too smooth make them more confusing
        // Vector direction from the bow's origin to the target
        bowDirToTarget = eRefs.NormDirToTargetV2(bowGameObject.transform.position, eRefs.PlayerPos);
        bowGameObject.transform.up = bowDirToTarget;
    }

    // The bow windup, hold and release animation. This also handles the events that happen during the animation like when to stop the rotation, possibly when to fire the arrow.
    public IEnumerator BowAnimation () {
        float timer = 0f;
        int spriteStep = 0;
        int eventStep = 0;
        // Enable bow rotation
        rotateBow = true;
        eRefs.eSpriteR.sprite = shootingStance;
        while (timer < bowTotalDuration) { //see the eventstep possible system, could assign the generic total duration from somewhere else, in this case if the coroutine was on a generic "scene global" object scripts could call and insert their own variables for total duration, etc. This would be a simpler alternative to having a method plug-in system, as the scripts could just hold their variables(sprites,whentochange,events) but the coroutines would be done on/from 1 game object. So not necessarily an alternative, could work together or would need to work together actually. Would make pausing all the scene's animations a LOT easier as they would all come from one place.
            timer += Time.deltaTime;
            if (spriteStep < bowSpriteChanges.Length && timer > bowSpriteChanges[spriteStep]) {
                bowSpriteR.sprite = bowSprites[spriteStep];
                spriteStep++;
            }
            if (eventStep < bowEvents.Length && timer > bowEvents[eventStep]) {
                if (eventStep == 0) { //could possibly do a system where the user can plug a method in the inspector along with the event step, which would allow the event trigger portion of the script/coroutine to be generic and the same for all anims
                    //stop bow rotation
                    rotateBow = false;
                }
                if (eventStep == 1) {
                    StartCoroutine(ArrowAnimation());
                }
                if (eventStep == 2) {
                    SetupProjectile(); // currently done in the ranged skeleton_throw projectiles script or at least the method is in that script. Do I want to just drop a reference and call it from here or move it to here, what would be left in the other script then? Can they be merged?
                }
                eventStep++;
            }
            //could put an active events section, which would mean things that need to occur every frame of this animation or for a portion of time based on a bool check, could set up both the recurring method and the bool to check against using an inspector system as mentionned in the steps to make the script generic.
            if (rotateBow) {
                AdjustBowOrientation();
                flip.FlipTowards(this.transform.position, eRefs.PlayerPos);
            }
            yield return null;
        }
        // Set sprite back to the default.
        bowSpriteR.sprite = defaultBowSprite;
        // Set rotation back to zero.
        bowGameObject.transform.eulerAngles = Vector3.zero;
        throwProj.ProjectileAttackDone();
        yield return null;
    }
    // The arrow charge up animation, happens during the bow animation.
    IEnumerator ArrowAnimation () {
        float timer = 0f;
        int spriteStep = 0;
        int eventStep = 0;
        while (timer < arrowTotalDuration) {
            timer += Time.deltaTime;
            if (spriteStep < arrowSpriteChanges.Length && timer > arrowSpriteChanges[spriteStep]) {
                arrowSpriteR.sprite = arrowSprites[spriteStep];
                spriteStep++;
            }
            if (eventStep < arrowEvents.Length && timer > arrowEvents[eventStep]) {
                if (eventStep == 0) {
                }
                eventStep++;
            }
            yield return null;
        }
        // Set sprite to null.
        arrowSpriteR.sprite = null;
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerTools;

public class SpriteAnimObject : MonoBehaviour
{
    public bool inUse;
    public SpriteRenderer spriteR;
    public SpriteAnim spriteAnim;
    public SO_SpriteAnimObject sOSpriteAnimObject;

    public void StartSpriteAnim(SO_SpriteAnimObject _sOSpriteAnimObject) {
        inUse = true;
        sOSpriteAnimObject = _sOSpriteAnimObject;
        this.gameObject.SetActive(true);
        spriteAnim.Play(sOSpriteAnimObject.animClip);
        StartCoroutine(InSpriteAnim());
    }

    public IEnumerator InSpriteAnim() {
        // Use either while playing or get the length of the animation and use a timer.
        while (spriteAnim.Playing) {
            
            yield return null;
        }
        this.gameObject.SetActive(false);
        spriteR.flipX = false;
        inUse = false;
    }
}

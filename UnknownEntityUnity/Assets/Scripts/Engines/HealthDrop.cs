using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerTools;

public class HealthDrop : MonoBehaviour
{
    public float healAmount = 5;
    public SpriteAnim spriteAnim;
    public AnimationClip loopAnimClip;


    void Start() {
        spriteAnim.Play(spriteAnim.Clip);
    }

    public void AnimStartAnimationLoop() {
        spriteAnim.Play(loopAnimClip);
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            // If the player is able to pick up the health drop, deactivate this object, etc.
            if (other.GetComponent<Character_Health>().CanIPickUpHealth(healAmount)) {
                this.gameObject.SetActive(false);
            }
        }
    }
}

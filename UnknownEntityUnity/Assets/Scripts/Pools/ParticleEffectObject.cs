using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectObject : MonoBehaviour
{
    public bool inUse;
    public ParticleSystem myParticleSystem;
    float timer;
    float partEffectDuration;
    bool followingObject;
    Transform objectToFollow;
    SO_ParticleEffect sO_ParticleEffect;

    public void StartParticleEffect(SO_ParticleEffect sOParticleEffect, Transform targetTrans, bool followObject = false, float duration = 0f, Sprite sprite = null) {
        inUse = true;
        sO_ParticleEffect = sOParticleEffect;
        // Find a way to assign all the particle system compnent values.
        var sOPSMain = sOParticleEffect.particleSystem.main;
        var myPSMain = myParticleSystem.main;
        myPSMain = sOPSMain;
        // If no duration is given upon request, take the duration from the scriptable object.
        if (duration > 0) {
            partEffectDuration = duration;
        }
        else {
            partEffectDuration = sOParticleEffect.inUseDuration;
        }
        var ts = myParticleSystem.textureSheetAnimation;
        // If no sprite is given upon request, take the sprite from the scriptable object.
        if (sprite != null) {
            ts.SetSprite(0, sprite);
        }
        else {
            ts.SetSprite(0, sO_ParticleEffect.sprite); 
        }
        // If the particle system needs to follow an object over its duration. Alternatively could just set the particle system's position once then assign the object to follow as its parent, then set its parent back to the pool at the end of the duration.
        if (followObject) {
            followingObject = true;
            objectToFollow = targetTrans;
        }
        else {
            this.transform.position = targetTrans.position;
        }
        timer = 0f;
        this.gameObject.SetActive(true);
        StartCoroutine(PlayParticleEffectFX());
    }

    public IEnumerator PlayParticleEffectFX() {
        myParticleSystem.Play();
        while (timer < partEffectDuration) {
            timer += Time.deltaTime;
            if (followingObject) {
                this.transform.position = objectToFollow.position;
            }
            yield return null;
        }
        timer = 0f;
        myParticleSystem.Stop();
        this.gameObject.SetActive(false);
        inUse = false;
    }
}

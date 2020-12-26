using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSpriteAnimation : MonoBehaviour
{
    static GenericSpriteAnimation instance;

    public static void StartGenericAnimation(SpriteRenderer targetSpriteRenderer, float totalDuration, Sprite[] sprites, float[] timings) {
        // Need to reference an instance of this monobehavior script to start a coroutine in a static method, aka need to start the coroutine on a regular nonstatic-monobehavior attached to a gameobject instanced in a scene.
        //StartCoroutine(PlayGenericAnimation(targetSpriteRenderer, totalDuration, sprites, timings));
    }

    IEnumerator PlayGenericAnimation(SpriteRenderer targetSpriteRenderer, float totalDuration, Sprite[] sprites, float[] timings) {
        float timer = 0f;
        int count = 0;
        while (timer < totalDuration) {
            timer += Time.deltaTime;
            if (timer > timings[count]) {
                targetSpriteRenderer.sprite = sprites[count];
                count++;
            }
        }
        yield return null;
    }
}

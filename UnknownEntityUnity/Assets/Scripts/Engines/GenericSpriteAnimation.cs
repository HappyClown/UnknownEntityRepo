using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSpriteAnimation : MonoBehaviour
{
    public SpriteRenderer spriteR;
    public float totalDuration;
    public Sprite[] sprites;
    public float[] changeSprites;
    public bool loop;
    public bool animate;
    private float timer = 0f;
    private int i = 0;

    public void Start() {
        i = Random.Range(0,changeSprites.Length);
        spriteR.sprite = sprites[i];
        timer = changeSprites[i];
    }

    public void Update() {
        if (animate) {
            timer += Time.deltaTime;
            if (timer > changeSprites[i]) {
                spriteR.sprite = sprites[i];
                if (i<changeSprites.Length-1) {
                    i++;
                }
            }
            if (timer >= totalDuration) {
                timer = 0f;
                i = 0;
                spriteR.sprite = sprites[i];
                if (!loop) {
                    animate = false;
                }
            }
        }
    }

    // static GenericSpriteAnimation instance;

    // public static void StartGenericAnimation(SpriteRenderer targetSpriteRenderer, float totalDuration, Sprite[] sprites, float[] timings) {
    //     // Need to reference an instance of this monobehavior script to start a coroutine in a static method, aka need to start the coroutine on a regular nonstatic-monobehavior attached to a gameobject instanced in a scene.
    //     //StartCoroutine(PlayGenericAnimation(targetSpriteRenderer, totalDuration, sprites, timings));
    // }

    // IEnumerator PlayGenericAnimation(SpriteRenderer targetSpriteRenderer, float totalDuration, Sprite[] sprites, float[] timings) {
    //     float timer = 0f;
    //     int count = 0;
    //     while (timer < totalDuration) {
    //         timer += Time.deltaTime;
    //         if (timer > timings[count]) {
    //             targetSpriteRenderer.sprite = sprites[count];
    //             count++;
    //         }
    //     }
    //     yield return null;
    // }
}

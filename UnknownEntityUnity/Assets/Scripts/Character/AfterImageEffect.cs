using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageEffect : MonoBehaviour
{
    public GameObject afterImagePrefab;
    public List<AfterImageObject> imagePoolObjects;
    float totalDuration;
    public float imageSpawnRate;
    public float imageFadeTime;
    Sprite sprite;
    bool flipX;
    Transform playerTransform;

    public void StartAfterImage(Sprite imageSprite, bool _flipX, float _totalDuration, Transform _playerTransform) {
        sprite = imageSprite;
        flipX = _flipX;
        totalDuration = _totalDuration;
        playerTransform = _playerTransform;
        StartCoroutine(InAfterImage());
    }

    IEnumerator InAfterImage() {
        float timer = 0f;
        float imageSpawnTimer = 0f;
        RequestAfterImageObject().StartFadeOut(imageFadeTime, sprite, playerTransform.position, flipX);
        while (timer < totalDuration) {
            timer += Time.deltaTime;
            imageSpawnTimer += Time.deltaTime;
            if (imageSpawnTimer > imageSpawnRate) {
                imageSpawnTimer = 0f;
                RequestAfterImageObject().StartFadeOut(imageFadeTime, sprite, playerTransform.position, flipX);
            }
            yield return null;
        }
        RequestAfterImageObject().StartFadeOut(imageFadeTime, sprite, playerTransform.position, flipX);
    }

    public AfterImageObject RequestAfterImageObject() {
        foreach (AfterImageObject obj in imagePoolObjects)
        {
            if (!obj.inUse) {
                return obj;
            }
        }
        imagePoolObjects.Add(Instantiate(afterImagePrefab, this.transform.position, Quaternion.identity, this.transform).GetComponent<AfterImageObject>());
        return imagePoolObjects[imagePoolObjects.Count-1];
    }
}

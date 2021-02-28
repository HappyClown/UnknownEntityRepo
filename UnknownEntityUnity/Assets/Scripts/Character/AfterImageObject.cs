using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageObject : MonoBehaviour
{
    public bool inUse;
    public SpriteRenderer mySpriteR;
    float fadeTime;

    public void StartFadeOut(float _fadeTime, Sprite _sprite, Vector2 _position, bool _flipX) {
        inUse = true;
        fadeTime = _fadeTime;
        mySpriteR.sprite = _sprite;
        this.transform.position = _position;
        mySpriteR.flipX = _flipX;
        this.gameObject.SetActive(true);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut() {
        float timer = 0f;
        float alphaValue = 1f;
        while (timer < 1f) {
            timer += Time.deltaTime / fadeTime;
            alphaValue = Mathf.Lerp(1, 0, timer);
            mySpriteR.color = new Color(mySpriteR.color.r, mySpriteR.color.g, mySpriteR.color.b, alphaValue);
        yield return null;
        }
        inUse = false;
        this.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_PlayerLifeBar : MonoBehaviour
{
    public float currentHealthPercent;
    // This could be grabbed automatically, usefull if the full life bar fill width changes during the game.
    //public float barMaxXScale; 
    //public Transform healthBarTrans;
    public float barRectTransMaxWidth;
    public float barRectTransHeight;
    public RectTransform healthBarRectTrans;
    [Header("WhiteBar")]
    public RectTransform whiteBarRectTrans;
    public float whiteBarStartDelay;
    public float whiteBarAdjustDur;
    Coroutine whiteBarCoroutine;

    public void AdjustHealthBar (float maxHealth, float curHealth) {
        currentHealthPercent = Mathf.Clamp01(curHealth / maxHealth);
        healthBarRectTrans.sizeDelta = new Vector2(barRectTransMaxWidth * currentHealthPercent, barRectTransHeight);
        //healthBarTrans.localScale = new Vector3(barMaxXScale * currentHealthPercent, healthBarTrans.localScale.y, healthBarTrans.localScale.z);
        if (whiteBarCoroutine != null) {
            StopCoroutine(whiteBarCoroutine);
        }
        whiteBarCoroutine = StartCoroutine(AdjustWhiteBar());
    }

    IEnumerator AdjustWhiteBar () {
        float timer = 0f;
        float startWidth = whiteBarRectTrans.rect.width;
        float endWidth = healthBarRectTrans.rect.width;
        yield return new WaitForSeconds(whiteBarStartDelay);
        while (timer < 1f) {
            timer += Time.deltaTime/whiteBarAdjustDur;
            whiteBarRectTrans.sizeDelta = new Vector2(Mathf.Lerp(startWidth, endWidth, timer), barRectTransHeight);
            yield return null;
        }
        whiteBarCoroutine = null;
    }

    // Could insert temporary white bar under the colored one to show hot much health was lost with the hit, the white bar would simply stay at the previous fill amount a bit longer then lerp to the current amount.
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_PlayerLifeBar : MonoBehaviour
{
    public float currentHealthPercent;
    public float barMaxXScale;
    public Transform healthBarTransform;

    public void AdjustHealthBar (float maxHealth, float curHealth) {
        currentHealthPercent = Mathf.Clamp01(curHealth / maxHealth);
        healthBarTransform.localScale = new Vector3(barMaxXScale * currentHealthPercent, healthBarTransform.localScale.y, healthBarTransform.localScale.z);
    }

    // Could insert temporary white bar under the colored one to show hot much health was lost with the hit, the white bar would simply stay at the previous fill amount a bit longer then lerp to the current amount.
}

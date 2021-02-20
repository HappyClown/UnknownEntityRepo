using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public float currentHealthPercent;
    public float barMaxXScale;
    public Transform healthBarTransform;

    public void AdjustHealthBar (float maxHealth, float curHealth) {
        currentHealthPercent = Mathf.Clamp01(curHealth / maxHealth);
        healthBarTransform.localScale = new Vector3(barMaxXScale * currentHealthPercent, healthBarTransform.localScale.y, healthBarTransform.localScale.z);
    }
}

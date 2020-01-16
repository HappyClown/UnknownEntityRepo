using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeGizmos : MonoBehaviour
{
    public float drawUpToXRange;
    public Color fromColor, toColor;
    private void OnDrawGizmos() {
        // float adjustment = 1/drawUpToXRange;
        // Color adjustedFromColor = new Color(fromColor.r - adjustment, fromColor.g - adjustment, fromColor.b - adjustment, 1);
        for (float i = 1; i <= drawUpToXRange; i++)
        {
            Gizmos.color = Color.Lerp(fromColor, toColor, (i-1)/(drawUpToXRange-1));
            Gizmos.DrawWireSphere(this.transform.position, i);
        }
    }
}

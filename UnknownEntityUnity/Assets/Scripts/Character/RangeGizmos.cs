using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeGizmos : MonoBehaviour
{
    public bool drawRangeGizmos;
    public float drawUpToXRange;
    public Color fromColor, toColor;

    private void OnDrawGizmos() {
        if (drawRangeGizmos){
            for (float i = 1; i <= drawUpToXRange; i++)
            {
                Gizmos.color = Color.Lerp(fromColor, toColor, (i-1)/(drawUpToXRange-1));
                Gizmos.DrawWireSphere(this.transform.position, i);
                //Handles.Label();
            }
        }
    }
}

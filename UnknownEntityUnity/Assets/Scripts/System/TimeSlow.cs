using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeSlow 
{
    

    public static void StartTimeSlow()
    {
        
    }

    public static IEnumerator SlowTimeScale(int frames, float timeScale) {
        int ticks = 0;
        Time.timeScale = timeScale;
        while (ticks < frames) {
            ticks++;
            yield return null;
        }
        Time.timeScale = 1;
    }
}

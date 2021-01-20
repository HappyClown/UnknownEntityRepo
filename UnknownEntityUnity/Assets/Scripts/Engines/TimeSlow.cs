using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlow : MonoBehaviour
{
    [Header("To-Set Values")]
    public AnimationCurve slowTimeAnimCurve;
    public static AnimationCurve slowTimeAnimCurve_St;
    [Header("Read Only")]
    public bool slowTimeOn;
    public bool allowTimeSlow;
    public static Coroutine slowCoroutine_St;
    public static TimeSlow timeSlowInstance_St;
    [Header("Static Values")]
    public static bool allowTimeSlow_St;
    public static bool slowTimeOn_St;
    public static int slowTimeticks_St;
    public static int slowTimeTotalFrames_St;
    private static float oneTickPercent_St;
    private static float ticksPercentage_St;

    void Start() {
        allowTimeSlow_St = allowTimeSlow;
        slowTimeAnimCurve_St = slowTimeAnimCurve;
        timeSlowInstance_St = this;
        // if (timeSlowInstance_St == null) {
        //     timeSlowInstance_St = new TimeSlow();
        // }
    }

    // void Update() {
    //     if (slowTimeOn_St) {
    //         if (slowTimeticks_St >= slowTimeTotalFrames_St) {
    //             Time.timeScale = 1f;
    //             slowTimeOn_St = false;
    //             return;
    //         }
    //         // Apply timeScale slow with anim curve, starting from 0f.
    //         Time.timeScale = slowTimeAnimCurve.Evaluate(ticksPercentage_St);
    //         //print("Ticks percent (on 1) value: "+ ticksPercentage_St);
    //         // Increase tick and tick value.
    //         ticksPercentage_St += oneTickPercent_St;
    //         slowTimeticks_St++;
    //         //print("TimeScale value: "+Time.timeScale);
    //     }
    // }

    public static void StartTimeSlow(int frames, float timeScale) {
        if (allowTimeSlow_St) {
            slowTimeticks_St = 0;
            ticksPercentage_St = 0f;
            // += will add the frames to prolong time slow if more slows are requested.
            // Could be changed to instead start a new time slow by setting to frames requested.
            slowTimeTotalFrames_St = frames;
            //print("Slow time total frame amount: "+ slowTimeTotalFrames_St);
            // This calculates the tick percentage to go from a value of 0 on the first frame to 1 on the last frame in the total amount of frames.
            oneTickPercent_St = (1f / slowTimeTotalFrames_St);
            oneTickPercent_St = oneTickPercent_St + ((1f / slowTimeTotalFrames_St) / (slowTimeTotalFrames_St-1f));
            //print("Tick percentage increase: "+oneTickPercent_St);
            //Find a way to trigger a coroutine/method from a static method to avoid using an Update
            slowTimeOn_St = true;
            //timeSlowInstance_St.StartCoroutine(DoTimeSlow());
            timeSlowInstance_St.StartCoroutineSlow();
        }
    }
    public void StartCoroutineSlow(){
        StartCoroutine(DoTimeSlow());
    }

    public IEnumerator DoTimeSlow() {
         while (slowTimeOn_St) {
            //print("Ticks percent (on 1) value: "+ ticksPercentage_St);
            // Increase tick and tick value.
            ticksPercentage_St += oneTickPercent_St;
            slowTimeticks_St++;
            // Apply timeScale slow with anim curve, starting from 0f.
            Time.timeScale = slowTimeAnimCurve_St.Evaluate(ticksPercentage_St);
            //print("TimeScale value: "+Time.timeScale);
            if (slowTimeticks_St >= slowTimeTotalFrames_St) {
                Time.timeScale = 1f;
                slowTimeOn_St = false;
            }
            yield return null;
        }
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

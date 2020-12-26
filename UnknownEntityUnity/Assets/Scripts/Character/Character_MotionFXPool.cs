using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_MotionFXPool : MonoBehaviour
{
    public List<Character_MotionFX> motionFXs = new List<Character_MotionFX>();
    public GameObject poolPrefab;

    public Character_MotionFX RequestMotionFX() {
        foreach (Character_MotionFX motionFX in motionFXs) {
            if (!motionFX.inUse) {
                return motionFX;
            }
        }
        motionFXs.Add(Instantiate(poolPrefab, poolPrefab.transform.localPosition, Quaternion.identity, this.transform).GetComponent<Character_MotionFX>());
        return motionFXs[motionFXs.Count-1];
    }
}

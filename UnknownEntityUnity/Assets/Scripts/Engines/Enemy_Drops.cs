using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Drops : MonoBehaviour
{
    [Header("Health Drops")]
    // On 100.
    public float baseHealthDropChance;
    // On 100.
    public float increasedPerKill; // Switch this to a value given by the enemy (small enemies give less, harder = more)
    // On 100.
    public float curHealthDropChance;
    public GameObject healthDropPrefab;
    
    public void CheckHealthDrop(Vector2 dropPosition, float curPlayerHealthPercent) {
        // if (curPlayerHealthPercent >= 1) {
        //     return;
        // }
        float healthDropRoll = Random.Range(0, 100);
        print("Health drop roll 0-100: "+healthDropRoll);
        if (healthDropRoll <= curHealthDropChance) {
            //Drop a health consumable.
            curHealthDropChance = baseHealthDropChance;
            // Switch this to use an object pool
            Instantiate(healthDropPrefab, dropPosition, Quaternion.identity);
        }
        else {
            curHealthDropChance += baseHealthDropChance;
        }
    }

}

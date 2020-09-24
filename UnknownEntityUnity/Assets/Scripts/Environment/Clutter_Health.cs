using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clutter_Health : MonoBehaviour
{
    public GameObject hitColliderObj;
    public SO_Clutter_Health clutterHealthSO;
    [Header("Read Only")]
    public float currentDamage;
    public void ReceiveDamage(float damageTaken) {
        // Add the damage received to check if you are destroyed.
        currentDamage += damageTaken;
        if (currentDamage >= clutterHealthSO.totalHealth) {
            IAmDestroyed();
        } 
    }

    public void IAmDestroyed() {
        // Turn off hit collider, change the sprite for the destroyed version, animate what needs animated.
        hitColliderObj.SetActive(false);
        print("Clutter destroyed, good job, you did it. You are just the best. Never give up.");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    public Enemy_Refs eRefs;
    public HealthBar healthBar;
    public float maxHealth;
    public float curHealth;
    public float damageModifier = 1f;

    private void Start() {
        // Relagate this to a startup function.
        maxHealth = eRefs.eSO.maximumLife;
        curHealth = maxHealth;
    }
    
    public void ReceiveDamage(float damage) {
        curHealth -= damage * damageModifier;
        healthBar.AdjustHealthBar(maxHealth, curHealth);
        if (curHealth <= 0f) {
            eRefs.eDeath.DeathSequence();
        }
    }
}

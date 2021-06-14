using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    public float healAmount = 5;
    // public Collider2D myCol;
    // public ContactFilter2D playerLayer;
    // List<Collider2D> results = new List<Collider2D>(); 
    
    // public float LootHealthDrop() {
    //     this.gameObject.SetActive(false);
    //     return healAmount;
    // }

    // public void Update() {
    //     Physics2D.OverlapCollider(myCol, playerLayer, results);
    //     print(results[0]);
    //     if (results[0] != null) {
    //         results[0].GetComponent<Character_Health>().CanIPickUpHealth();
    //     }
    // }

    void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            // If the player is able to pick up the health drop, deactivate this object, etc.
            if (other.GetComponent<Character_Health>().CanIPickUpHealth(healAmount)) {
                this.gameObject.SetActive(false);
            }
        }
    }
}

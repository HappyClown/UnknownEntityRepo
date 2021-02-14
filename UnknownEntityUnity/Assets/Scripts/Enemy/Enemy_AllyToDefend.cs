using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AllyToDefend : MonoBehaviour
{
    public Enemy_Refs eRefs;
    public bool defended;
    public Enemy_Defender defender;

    // void Update()
    // {
    //     if (eRefs.eDeath.isDead) {
    //         if (defender) {
    //             defender.canDefend = false;
    //         }
    //         this.enabled = false;
    //     }
    // }

    public void FreeUpDefender() {
        defender.canDefend = false;
    }
}

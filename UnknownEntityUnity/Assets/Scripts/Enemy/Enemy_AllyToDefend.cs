using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AllyToDefend : MonoBehaviour
{
    public Enemy_Refs eRefs;
    public bool defended;
    public Enemy_Defender defender;

    public void FreeUpDefender() {
        if (defender != null) {
            defender.canDefend = false;
        }
    }
}

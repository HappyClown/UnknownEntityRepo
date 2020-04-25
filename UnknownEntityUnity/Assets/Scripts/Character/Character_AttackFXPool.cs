using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AttackFXPool : MonoBehaviour
{
    public List<Character_AttackFX> atkFXs = new List<Character_AttackFX>();
    public GameObject poolPrefab;

    public Character_AttackFX RequestAttackFX() {
        foreach (Character_AttackFX atkFX in atkFXs) {
            if (!atkFX.inUse) {
                return atkFX;
            }
        }
        atkFXs.Add(Instantiate(poolPrefab, poolPrefab.transform.position, Quaternion.identity, this.transform).GetComponent<Character_AttackFX>());
        return atkFXs[atkFXs.Count-1];
    }
}
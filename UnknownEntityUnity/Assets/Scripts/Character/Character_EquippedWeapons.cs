using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_EquippedWeapons : MonoBehaviour
{
    public SO_WeaponBase firstWeap, secondWeap;
    public Character_Attack atk;
    public SpriteRenderer weaponSpriteR;

    void Update() {
        // Weapon swap.
        if (Input.GetKeyDown("q")) {
            if (firstWeap != null && secondWeap != null) {
                SO_WeaponBase tempSecondWeap = secondWeap;
                secondWeap = firstWeap;
                firstWeap = tempSecondWeap;
                atk.weapon = firstWeap;
                SpriteSwap();
            }
        }
    }

    public void SpriteSwap() {
        weaponSpriteR.sprite = firstWeap.weaponSprite;
        //Debug.Log("Weapons swapped.");
    }
}
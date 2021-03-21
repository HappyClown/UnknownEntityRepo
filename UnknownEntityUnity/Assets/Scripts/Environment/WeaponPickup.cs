using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public bool amIOn;
    public SpriteRenderer mySpriteR;
    public BoxCollider2D myBoxCol;
    [Header("Weapon Info")]
    public SO_Weapon weaponBase;
    public float durability = 100;

    public void SwapWeaponLoot(SO_Weapon newWeapBase) {
        weaponBase = newWeapBase;
        if (!mySpriteR) {
            mySpriteR = this.GetComponent<SpriteRenderer>();
        }
        mySpriteR.sprite = weaponBase.weaponSprite;
        // Play newloot anim?

    }

    public void TurnOff() {
        amIOn = false;
        this.gameObject.SetActive(false);
        //mySpriteR.enabled = false;
        //myBoxCol.enabled = false;
    }
}
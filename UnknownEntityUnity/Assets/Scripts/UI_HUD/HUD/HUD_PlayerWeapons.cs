using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_PlayerWeapons : MonoBehaviour
{
    public Image weapOneImage, weapTwoImage;
    public RectTransform weapOneTrans, weapTwoTrans;
    public RectTransform activeWeapHighlightTrans, activeWeapAtkChainResetTrans;
    public Image activeWeapAtkChainResetImage;
    public GameObject[] activeWeapAtkChainCounters;
    bool weaponOneIsActive = true;
    Vector2 newWeapPos;

    public void SwapActiveWeapon() {
        // change active weapon highlight to the other weapon
        // change chain reset to other weapon
        // transfer chain counters if possible or reset them, could poke the proper atkchain script to know how many there are
        if (weaponOneIsActive) {
            newWeapPos = weapTwoTrans.position;
            weaponOneIsActive = false;
        }
        else {
            newWeapPos = weapOneTrans.position;
            weaponOneIsActive = true;
        }
        activeWeapHighlightTrans.position = newWeapPos;
        activeWeapAtkChainResetTrans.position = newWeapPos;
    }
    public void PickUpWeapon(Sprite weapPickedUp) {
        // change image to newly picked up weapon
        // turn off all chain counters
        // reset chain reset to 0
        // 
        if (weaponOneIsActive) {
            weapOneImage.sprite = weapPickedUp;
            weapOneImage.SetNativeSize();
        }
        else {
            weapTwoImage.sprite = weapPickedUp;
            weapTwoImage.SetNativeSize();
        }
    }
}

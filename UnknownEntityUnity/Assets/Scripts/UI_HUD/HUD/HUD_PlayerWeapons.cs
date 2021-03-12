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
    [Header("Durability")]
    public Image weapOneDurabilityBarImage;
    public Image weapTwoDurabilityBarImage;
    public float durabilityBarMaxWidth;
    public RectTransform durabilityBarOneTrans, durabilityBarTwoTrans;
    //float currentDurabilityBarWidth;
    public Color fullDurabilityColor;
    public Color noDurabilityColor;

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
    public void AdjustDurabilityBar(float currentDurability) {
        float durabilityPercent = currentDurability/100;
        if (weaponOneIsActive) {
            // Current durability divided by the max durability to get the current durability percentage and multiply it to the bar's max width to get the current bar width.
            durabilityBarOneTrans.sizeDelta = new Vector2(durabilityPercent*durabilityBarMaxWidth, durabilityBarOneTrans.rect.height);
            weapOneDurabilityBarImage.color = Color.Lerp(noDurabilityColor, fullDurabilityColor, durabilityPercent);
        }
        else {
            durabilityBarTwoTrans.sizeDelta = new Vector2(durabilityPercent*durabilityBarMaxWidth, durabilityBarTwoTrans.rect.height);
            weapTwoDurabilityBarImage.color = Color.Lerp(noDurabilityColor, fullDurabilityColor, durabilityPercent);
        }
    }
}

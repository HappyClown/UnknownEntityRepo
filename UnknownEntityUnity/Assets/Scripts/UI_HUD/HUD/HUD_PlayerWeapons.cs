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
    public void PickUpWeapon(Sprite weapPickedUp, float durability) {
        // change image to newly picked up weapon
        // turn off all chain counters
        // reset chain reset to 0
        // Check if my inactive weapon is null, if set the active weapon to the inactive weapon slot (one to two or two to one).
        // if (weaponOneIsActive) {
        //     if (weapTwoImage == null) {
        //         // Switch weapon one to weapon two slot.
        //     }
        // }

        // If a weapon has no image switch the active weapon to the other slot (this assumes the other slot will always be the empty one at any point when this is called). May need to change this to check which is active in the EquippedWeapon script or have it call this method while telling it if it only has an empty weapon slot.
        if (weapOneImage.sprite == null || weapTwoImage.sprite == null) {
            SwapActiveWeapon();
        }
        // 
        if (weaponOneIsActive) {
            weapOneImage.sprite = weapPickedUp;
            weapOneImage.SetNativeSize();
            
            float durabilityPercent = durability/100;
            durabilityBarOneTrans.sizeDelta = new Vector2(Mathf.Clamp(durabilityPercent*durabilityBarMaxWidth, 0, durabilityBarMaxWidth), durabilityBarOneTrans.rect.height);
            weapOneDurabilityBarImage.color = Color.Lerp(noDurabilityColor, fullDurabilityColor, durabilityPercent);
        }
        else {
            weapTwoImage.sprite = weapPickedUp;
            weapTwoImage.SetNativeSize();

            float durabilityPercent = durability/100;
            durabilityBarTwoTrans.sizeDelta = new Vector2(Mathf.Clamp(durabilityPercent*durabilityBarMaxWidth, 0, durabilityBarMaxWidth), durabilityBarTwoTrans.rect.height);
            weapTwoDurabilityBarImage.color = Color.Lerp(noDurabilityColor, fullDurabilityColor, durabilityPercent);
        }
        // Adjust durability bar to the picked up weapons durability.

    }
    public void AdjustDurabilityBar(bool damageWeaponOne, float currentDurability) {
        float durabilityPercent = currentDurability/100;
        if (damageWeaponOne) {
            // Current durability divided by the max durability to get the current durability percentage and multiply it to the bar's max width to get the current bar width.
            durabilityBarOneTrans.sizeDelta = new Vector2(Mathf.Clamp(durabilityPercent*durabilityBarMaxWidth, 0, durabilityBarMaxWidth), durabilityBarOneTrans.rect.height);
            weapOneDurabilityBarImage.color = Color.Lerp(noDurabilityColor, fullDurabilityColor, durabilityPercent);
        }
        else {
            durabilityBarTwoTrans.sizeDelta = new Vector2(Mathf.Clamp(durabilityPercent*durabilityBarMaxWidth, 0, durabilityBarMaxWidth), durabilityBarTwoTrans.rect.height);
            weapTwoDurabilityBarImage.color = Color.Lerp(noDurabilityColor, fullDurabilityColor, durabilityPercent);
        }
    }
    public void RemoveBrokenWeapon() {
        // Set sizeDelta to 0 because setting a UI image component to null leaves a white rectangle of the objects current size. This is put back to a good size without addition lines of code when SetNativeSize is called on weapon pickup.
        if (weaponOneIsActive) {
            weapOneImage.sprite = null;
            weapOneImage.rectTransform.sizeDelta = Vector2.zero;
            durabilityBarOneTrans.sizeDelta = new Vector2(0f , durabilityBarOneTrans.rect.height);
        }
        else {
            weapTwoImage.sprite = null;
            weapTwoImage.rectTransform.sizeDelta = Vector2.zero;
            durabilityBarTwoTrans.sizeDelta = new Vector2(0f , durabilityBarTwoTrans.rect.height);
        }
        // sinceweapon was equipped the hud highlight should change to the other one, but this will be called in the EquippedWEapons script where it wil lcall swap active weapon
    }
}

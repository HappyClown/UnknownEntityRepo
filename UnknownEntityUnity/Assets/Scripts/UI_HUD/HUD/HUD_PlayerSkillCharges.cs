using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_PlayerSkillCharges : MonoBehaviour
{
    public List<Image> chargeImages = new List<Image>();
    public Sprite readyCharge;
    public Sprite usedCharge;

    public void UseCharge() {
        for (int i = chargeImages.Count-1; i >= 0; i--) {
            if (chargeImages[i].sprite == readyCharge) {
                chargeImages[i].sprite = usedCharge;
                chargeImages[i].SetNativeSize();
                break;
            }
        }
        Debug.Log("Wow wow wow la!! A skill charge was supposed to be used but nothing was found, you should investigate this.");
    }
    public void RefillCharge() {
        for (int i = 0; i < chargeImages.Count; i++) {
            if (chargeImages[i].sprite == usedCharge) {
                chargeImages[i].sprite = readyCharge;
                chargeImages[i].SetNativeSize();
                break;
            }
        }
        Debug.Log("Hey! Hey you!! A skill charge was supposed to be refilled but nothing was found, you should investigate this.");
    }

    // public void AddNewCharge() {}
}

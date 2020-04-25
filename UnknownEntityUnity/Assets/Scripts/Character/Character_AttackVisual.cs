using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AttackVisual : MonoBehaviour
{
    //public SpriteRenderer[] atkSpriteRs;
    //public bool[] activeAttacks;

    public IEnumerator AttackAnimation(Sprite[] newSprites, float[] spriteChanges, float attackLength, int chainNum, Character_AttackFX atkFX) {
        // if (activeAttacks[chainNum]) {
        //     activeAttacks[chainNum] = false;
        //     yield return null;
        // }
        //activeAttacks[chainNum] = true;
        atkFX.inUse = true;
        SpriteRenderer atkSpriteR = atkFX.spriteR;
        atkFX.gameObject.SetActive(true);
        Sprite[] atkSprites = newSprites;
        float[] atkSpriteChangeTimes = spriteChanges;
        float thisAtkLength = attackLength;
        int thisChainNum = chainNum;
        float timer = 0f;
        int thisSpriteIndex = 0;
        atkSpriteR.sprite = atkSprites[thisSpriteIndex];
        
        while (/* activeAttacks[thisChainNum] &&  */timer < thisAtkLength) {
            timer += Time.deltaTime;
            if (timer >= atkSpriteChangeTimes[thisSpriteIndex]) {
                atkSpriteR.sprite = atkSprites[thisSpriteIndex];
                if (thisSpriteIndex < atkSpriteChangeTimes.Length -1) {
                    thisSpriteIndex++;
                }
            }
            yield return null;
        }
        atkSpriteR.sprite = null;
        //activeAttacks[thisChainNum] = false;
        atkFX.gameObject.SetActive(false);
        atkFX.inUse = false;
        yield return null;
    }
}
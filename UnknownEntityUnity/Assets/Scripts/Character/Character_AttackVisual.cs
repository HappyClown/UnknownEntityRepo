using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AttackVisual : MonoBehaviour
{
    public SpriteRenderer[] atkSpriteRs;
    public bool[] activeAttacks;

    public IEnumerator AttackAnimation(Sprite[] newSprites, float[] spriteChanges, float attackLength, int chainNum) {
        if (activeAttacks[chainNum]) {
            activeAttacks[chainNum] = false;
            yield return null;
        }
        activeAttacks[chainNum] = true;
        Sprite[] atkSprites = newSprites;
        float[] atkSpriteChangeTimes = spriteChanges;
        float thisAtkLength = attackLength;
        int thisChainNum = chainNum;
        float timer = 0f;
        int thisSpriteIndex = 0;
        atkSpriteRs[thisChainNum].sprite = atkSprites[thisSpriteIndex];
        
        while (activeAttacks[thisChainNum] && timer < thisAtkLength) {
            timer += Time.deltaTime;
            if (timer >= atkSpriteChangeTimes[thisSpriteIndex]) {
                atkSpriteRs[thisChainNum].sprite = atkSprites[thisSpriteIndex];
                if (thisSpriteIndex < atkSpriteChangeTimes.Length -1) {
                    thisSpriteIndex++;
                }
            }
            yield return null;
        }
        atkSpriteRs[thisChainNum].sprite = null;
        activeAttacks[thisChainNum] = false;
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSpritePoolObject : MonoBehaviour
{
    public bool inUse;
    public SpriteRenderer spriteR;

    public void StaticSpriteInUse() {
        inUse = true;
        this.gameObject.SetActive(true);
    }
}

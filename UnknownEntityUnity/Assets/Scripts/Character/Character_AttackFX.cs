using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AttackFX : MonoBehaviour
{
    public bool inUse;
    public bool stopOnStun;
    public bool canInterrupt = true;
    public bool involuntaryCancel = true;
    public bool holdLastSprite;
    public SpriteRenderer spriteR;
    public PolygonCollider2D col;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_SpecialAttackFX", menuName = "SO_WeaponStuff/SO_AttackStuff/SpecialAttackFX", order = 0)]
public class SO_SpecialAttackFX : ScriptableObject {

    #region WindUp section of the special attack
    public float windupDuration; // Total duration of the WindUp
    public Sprite[] sprites;
    public float[] spriteChangeTiming;
    public float spawnDistance;
    public float moveDistance;
    public float moveDelay;
    public bool setupBeforeDelay;
    public bool followWeapon;
    public float followWeaponHeight; // Y value adjustment for FXs that follow the weapon orientation.
    public AnimationCurve moveAnimCurve;
    #endregion

    #region Hold section of the special attack
    public bool loopAnimation;
    public float holdDuration;
    public Sprite[] holdSprites;
    public float[] holdSpriteChangeTiming;

    #endregion

    #region Release section of the special attack
    
    #endregion

}

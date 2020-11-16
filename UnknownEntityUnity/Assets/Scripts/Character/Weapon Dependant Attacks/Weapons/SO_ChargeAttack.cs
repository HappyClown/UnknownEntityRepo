using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_ChargeAttack")]
public class SO_ChargeAttack : ScriptableObject
{
    // Most likely for charged attacks with an intensity ramp up or intensity stages with variable effects, AoE, damage, etc.
    // For a chargeable attack, immediately start these as the timer increases towards the minimum charge time, if the player lets go of the button too early the attack fizzles out.
    //Keep in mind the InputMaster's min press down time for it to be considered a hold (ex: 0.2)
    public float chargingTimeReq;
    public SO_AttackFX chargingFX;
    public SO_CharAtk_Motion chargingWeapMotion;
    public SO_CharAtk_Motion chargingCharMotion;
    // If the player held the button long enough,there is a hold/ looking FX as the character can move around while holding the button waiting to release the attack when they want.
    public SO_AttackFX holdFX;
    public SO_CharAtk_Motion holdWeapMotion;
    public SO_CharAtk_Motion holdCharMotion;
    //Release effects are handles by the SO_Weapon's attack chain attributes.
}

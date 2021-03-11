using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerTools;

public class Character_MovementSkill_Dash : Character_MovementSkills
{
    [Header("Scripts")]
    public MouseInputs moIn;
    public Character_Movement charMove;
    public Character_Attack charAttack;
    public HUD_Manager HUDManager;
    // A lot of the values could fit in a Scriptable Object.
    [Header("Dash Values")]
    public bool dashing;
    Vector3 dashDirection;
    float dashSpeed;
    public float dashDuration;
    public float dashDistance;
    Vector3 newPos, curPos;
    [Header("Charges")]
    public int maxCharges = 3;
    public int curCharges = 3;
    public float chargeCooldown;
    int chargesOnCooldown;
    Coroutine chargeCooldownCoroutine;
    [Header("VFX")]
    public SpriteAnimPool spriteAnimPool;
    public AfterImageEffect afterImageEffect;
    public SO_SpriteAnimObject sOSpriteAnimObject;
    SpriteAnimObject spriteAnimObject;

    // void Update() {
    //     // MOVE THIS TO THE OTHER INPUT ACTION SCRIPT
    //     if (moIn.movementSkillPressed) {
    //         CanIUseMovementSkill();
    //     }
    // }
    public override bool CanIUseMovementSkill() {
        if (!dashing && curCharges > 0 && charMove.running && charAttack.CanInterruptAttackCheck()) {
            // Either the player needs to be running or it dashes in the towards/away from where the player is pointing.
            StartMovementSkill();
            return true;
        }
        return false;
    }

    public void StartMovementSkill() {
        // Stop the player's current attack.
        charAttack.StopAttack();
        dashDirection = charMove.normalizedMovement;
        dashSpeed = dashDistance/dashDuration;
        // This is just to avoid dividing every update. Example: Time.deltaTime/dashDuration -> Time.deltaTime*multiplierDuration.
        charMove.canInputMove = false;
        // Stop player from attacking.
        // Use and put a charge on cooldown, change the HUD accordingly.
        UseACharge();
        dashing = true;
        // Request, adjust, and play the dash dust FX sprite animation.
        spriteAnimObject = spriteAnimPool.RequestSpriteAnimObject();
        spriteAnimObject.gameObject.transform.position = charMove.transform.position;
         if (dashDirection.x < 0) spriteAnimObject.gameObject.GetComponent<SpriteRenderer>().flipX = true;
         else spriteAnimObject.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        spriteAnimObject.StartSpriteAnim(sOSpriteAnimObject);
        // Start the after image FX.
        afterImageEffect.StartAfterImage(charMove.spriteRend.sprite, charMove.spriteRend.flipX, dashDuration, charMove.transform);
        StartCoroutine(Dashing());
    }

    IEnumerator Dashing() {
        float timer = 0f;
        while (timer < dashDuration) {
        //print("Hello hello hola, let the movement go dash");
            timer += Time.deltaTime;
            curPos = charMove.transform.position;
            newPos = curPos + (dashDirection*dashSpeed*Time.deltaTime);
            charMove.MoveThePlayer(dashDirection, newPos, curPos);
            yield return null;
        }
        StopMovementSkill();
    }

    public void StopMovementSkill() {
        dashing = false;
        charMove.canInputMove = true;
        charMove.charCanFlip = true;
        charAttack.readyToAtk = true;
    }

    void UseACharge() {
        curCharges--;
        chargesOnCooldown++;
        HUDManager.playerSkillCharges.UseCharge();
        if (chargeCooldownCoroutine == null) {
            chargeCooldownCoroutine = StartCoroutine(ChargeCooldown());
        }
    }

    IEnumerator ChargeCooldown() {
        float timer = 0f;
        while (timer < chargeCooldown) {
            timer += Time.deltaTime;
            if (timer > chargeCooldown) {
                chargesOnCooldown--;
                curCharges++;
                HUDManager.playerSkillCharges.RefillCharge();
                if (chargesOnCooldown > 0) {
                    timer = 0f;
                }
            }
            yield return null;
        }
        chargeCooldownCoroutine = null;
    }
}

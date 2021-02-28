using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_MovementSkill_Dash : MonoBehaviour
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
    public Character_MotionFXPool charMotionFXPool;
    public AfterImageEffect afterImageEffect;
    //public ParticleSystem partSys;
    //public ParticleEffectPool partEffectPool;
    //public SO_ParticleEffect sOPartEffect;
    //ParticleEffectObject partEffectObject;

    // Dust VFX + after images using the character's current sprite in a... dum Dum DUUUM, unity particle system. Make a pool for particle systems.

    void Update() {
        // MOVE THIS TO THE OTHER INPUT ACTION SCRIPT
        if (moIn.movementSkillPressed) {
            CanIUseMovementSkill();
        }
    }
    public void CanIUseMovementSkill() {
        if (!dashing && curCharges > 0) {
            // and player is input moving
            StartMovementSkill();
        }
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
        // Start the initial particle effects.
        //partEffectObject = partEffectPool.RequestParticleEffect();
        // partEffectObject.StartParticleEffect(sOPartEffect, this.transform, true, dashDuration, charMove.spriteRend.sprite);
        //AfterImagePartSys();
        afterImageEffect.StartAfterImage(charMove.spriteRend.sprite, charMove.spriteRend.flipX, dashDuration, charMove.transform);
        StartCoroutine(Dashing());
    }

    IEnumerator Dashing() {
        float timer = 0f;
        while (timer < dashDuration) {
        print("Hello hello hola, let the movement go dash");
            timer += Time.deltaTime;
            curPos = this.transform.position;
            newPos = curPos + (dashDirection*dashSpeed*Time.deltaTime);
            charMove.MoveThePlayer(dashDirection, newPos, curPos);
            yield return null;
        }
        // Activate gameobject.
        //partSys.gameObject.SetActive(false);
        StopMovementSkill();
    }

    public void StopMovementSkill() {
        dashing = false;
        charMove.canInputMove = true;
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

    // public void AfterImagePartSys() {
    //     // Assign duration.
    //     var psMain = partSys.main;
    //     psMain.duration = dashDuration;
    //     // Assign sprite.
    //     var psTexSheetAnim = partSys.textureSheetAnimation;
    //     psTexSheetAnim.SetSprite(0, charMove.spriteRend.sprite);
    //     // Set X scale to the same as the player.
        
    //     // Activate gameobject.
    //     partSys.gameObject.SetActive(true);
    //     partSys.Play();
    // }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Health : MonoBehaviour
{
    public Character_Attack charAtk;
    public Character_Movement charMov;
    public Character_Death charDeath;
    public WeaponLookAt weapLookAt;
    public HealthBar healthBar;
    public float maximumHealth;
    public  float currentHealth;
    // Not sure if this is gonna stay here, hit sprites.
    [Header("Get Hit Animation")]
    public SpriteRenderer spriteR;
    public Sprite[] hitSprites;
    public float[] hitSpriteTimings;
    public float getHitDuration;
    float timer;
    int spriteNumber;
    // Same as animation, they could be ported somewhere else, or not.
    [Header("Get Hit Movement")]
    public bool charHitMoveOn;
    private float moveSpeed, moveDistance;
    private Vector3 normHitDirection;
    private float moveTimer;
    private Vector3 curPosition, newPosition;
    // Same as animation, 
    // [Header("Hit FX Impact")]
    // public SO_ImpactFX sOImpactFX;
    // private ImpactFX impactFX;
    // public ImpactFXPool impactFXPool;
    // private Vector3 hitPosition;

    public IEnumerator TakeHitAnimation() {
        // Hit FX.
        //HitEffect();
        // Slow down time.
        TimeSlow.StartTimeSlow(5, 0f);
        // Set camera nudge backwards from the hit.
        CameraFollow.CameraNudge_St(normHitDirection, 0.15f);
        //StartCoroutine(TimeSlow.SlowTimeScale(5, 0));
        // Can be transfered to the update.
        // Switch this for an Interumpt/Stun, stop all input, attacks, etc. Can also add variables for knockback and stun duration, based on damage received, buffs, damage immunity, etc.
        // Player cannot move with input.
        charMov.StopInputMove();
        // Player cannot flip.
        charMov.charCanFlip = false;
        charMov.FlipSpriteDirectionBased(normHitDirection);
        // Stops; player attack movement, attack FXs that are stopped on stun, weapon attack motion.
        charAtk.StopAttack();
        // Player weapon does not follow cursor.
        weapLookAt.lookAtEnabled = false;
        //
        // Setup hit animation.
        timer = 0f;
        spriteNumber = 0;
        spriteR.sprite = hitSprites[0];
        spriteNumber++;
        while (timer < getHitDuration) {
            //print("Should be changing sprite to getting hit in the face sprite.");
            timer += Time.deltaTime/getHitDuration;
            if (spriteNumber < hitSprites.Length && timer > hitSpriteTimings[spriteNumber]) {
                spriteR.sprite = hitSprites[spriteNumber];
                spriteNumber++;
            }
            yield return null;
        }
        // If health reaches 0, iniate death sequence.
        if (currentHealth <= 0f) {
            charDeath.CharacterDies();
        }
        else {
            charMov.canInputMove = true;
            charAtk.atkChain.ready = true;
            weapLookAt.lookAtEnabled = true;
            charMov.charCanFlip = true;
        }
    }

    public void TakeDamage(float damage, Vector2 normHitDirection_) {
        currentHealth -= damage;
        healthBar.AdjustHealthBar(maximumHealth, currentHealth);
        // Normalized hit direction from the hittingCollider to the playerCollider.
        normHitDirection = new Vector3(normHitDirection_.x, normHitDirection_.y, charMov.transform.position.z);
        //hitPosition = hitPos;
        StartCoroutine(TakeHitAnimation());
        HitTakenMovement();
    }

    public void HitTakenMovement() {
        // Hard coded: 
        moveDistance = 1f;
        // 
        charHitMoveOn = true;
        moveTimer = 0f;
        moveSpeed = moveDistance / getHitDuration;
    }

    // public void HitEffect() {
    //     impactFX = impactFXPool.RequestImpactFX();
    //     impactFX.transform.position = new Vector3(hitPosition.x, hitPosition.y, impactFX.transform.position.z);
    //     impactFX.transform.up = normHitDirection;
    //     impactFX.StartImpactFX(sOImpactFX);
    // }

    void Update() {
        // Can be transfered to the coroutine for the animation, specially since they both rely on the same get hit duration.
        if (charHitMoveOn) {
            moveTimer += Time.deltaTime / getHitDuration;
            if (moveTimer >= getHitDuration) {
                charHitMoveOn = false;
                moveTimer = 0f;
            }
            //print(curPosition.z + "And new posz: " + newPosition.z);
            curPosition = charMov.transform.position;
            newPosition = curPosition + (normHitDirection*moveSpeed*Time.deltaTime);
            charMov.MoveThePlayer(normHitDirection, newPosition, curPosition);
        }
    }
}

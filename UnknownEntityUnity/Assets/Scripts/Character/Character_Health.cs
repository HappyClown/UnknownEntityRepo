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
    private Vector3 moveDirection;
    private float moveTimer;
    private Vector3 curPosition, newPosition;

    public IEnumerator TakeHitAnimation() {
        // Can be transfered to the update.
        // Switch this for an Interumpt/Stun, stop all input, attacks, etc. Can also add variables for knockback and stun duration, based on damage received, buffs, damage immunity, etc.
        // Player cannot move with input.
        charMov.StopInputMove();
        // Player cannot flip.
        charMov.charCanFlip = false;
        charMov.FlipSpriteDirectionBased(moveDirection);
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

    public void TakeDamage(float damage, Vector2 hitDirection) {
        currentHealth -= damage;
        healthBar.AdjustHealthBar(maximumHealth, currentHealth);
        // Hit movement and animation.
        moveDirection = new Vector3(hitDirection.x, hitDirection.y, charMov.transform.position.z);
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
    void Update() {
        // Can be transfered to the coroutine for the animation, specialyl since they both rely on he same get hit duration.
        if (charHitMoveOn) {
            moveTimer += Time.deltaTime / getHitDuration;
            if (moveTimer >= getHitDuration) {
                charHitMoveOn = false;
                moveTimer = 0f;
            }
            //print(curPosition.z + "And new posz: " + newPosition.z);
            curPosition = charMov.transform.position;
            newPosition = curPosition + (moveDirection*moveSpeed*Time.deltaTime);
            charMov.MoveThePlayer(moveDirection, newPosition, curPosition);
        }
    }
}

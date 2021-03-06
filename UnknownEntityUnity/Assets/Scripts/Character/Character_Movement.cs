﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerTools;

public class Character_Movement : MonoBehaviour
{
    [Header("Script References")]
    public MouseInputs moIn;
    public Character_CollisionDetection colDetect;
    [Header("To-set variables")]
    public float baseRunSpeed = 1f; 
    public SpriteRenderer spriteRend;
    public Transform characterSprite;
    public Animator animator;
    
    [Header("Read Only")]
    private float moddedRunSpeed, lessSpeedModifier, moreSpeedModifier;
    public bool canInputMove = true;
    public bool charCanFlip = true;
    public float normMagMovement;
    public float moveDirX, moveDirY;
    private bool lookLeft, lookRight, moveLeft, moveRight;
    private bool runningLastFrame;
    public bool running;
    public Vector3 normalizedMovement;
    [Header("Run FX")]
    public float runFXFrequency;
    // Could hold most of the sprite animation values in a ScriptableObject.
    public Character_MotionFXPool charMotionFXPool;
    public float dustTotDur;
    public Sprite[] dustSprites;
    public float[] dustTimings;
    private float timeRunning;
    [Header("Animation Test")]
    public Sprite idleSprite;
    private Sprite[] curSprites;
    // Hand sprite animations test
    public Sprite[] startRunForward, startRunBackwards, runForward, runbackwards;
    public float[] startRunForwardTimings, startRunBackwardsTimings, runForwardTimings, runBackwardsTimings;
    private bool startRunForwardBool, startRunBackwardsBool, runForwardBool, runBackwardsBool;
    public Sprite[] idleOne, idleTwo;
    public float[] idleOneTimings, idleTwoTimings;
    private bool firstIdleFrame;
    public float idleCooldownDur;
    private bool idleOnCooldown = false;
    private float animTimer;
    private int tick;
    public float animRunSpeedMult = 1f;
    // SpriteAnim
    public SpriteAnim mySpriteAnim;
    public AnimationClip forwardStart, backStart, forwardLoop, backLoop;
    public AnimationClip idle;
    //
    private bool lastLookLeft, lastLookRight, lastMoveLeft, lastMoveRight;


    private Vector2 testposcurpos;

    // Positive values to lower speed by % (0 to 1), negative values to return lowered speed.
    public void ReduceSpeed (float percentOnOne) {
        lessSpeedModifier += percentOnOne;
        CalculateRunSpeed();
    }
    // Positive values to increase speed by % (0 to 1), negative values to return increased speed.
    public void IncreaseSpeed (float percentOnOne) {
        moreSpeedModifier += percentOnOne;
        CalculateRunSpeed();
    }
    // Calculate actual run speed.
    public void CalculateRunSpeed() {
        moddedRunSpeed = Mathf.Clamp(baseRunSpeed * ((1-lessSpeedModifier) + moreSpeedModifier), 0f, 15f);
    }

    void Start() {
        CalculateRunSpeed();
    }

    void Update() {
        // Check if sprite needs to be flipped based on mouse position.
        if (moIn.mouseMoved && charCanFlip) {
          FlipSpriteMouseBased();
        }
        // An int system could be used to determine if canInputMove is true/instead of it. int canInputMove=0, canInputMove++, if (canInputMove == 0) { The player can move.}
        if (canInputMove) {
            // Detect Player Character movement.
            normalizedMovement = new Vector3(moIn.x, moIn.y, 0f);
            normMagMovement = normalizedMovement.magnitude;
            moveDirX = normalizedMovement.x;
            if (normalizedMovement.magnitude >= 0.01f) {
                running = true;
                runningLastFrame = true;
                // Check run direction, left or right.
                if (moveDirX < 0) {
                    moveLeft = true;
                    moveRight = false;
                }
                else if (moveDirX > 0) {
                    moveLeft = false;
                    moveRight = true;
                }
                else {
                    moveRight = lookRight;
                    moveLeft = lookLeft;
                }
                // Add up the time.deltaTime every frame the player is moving. The same concept could be used for more but at least to know when to spawn running dust.
                timeRunning += Time.deltaTime;
                if (timeRunning >= runFXFrequency) {
                    timeRunning = 0f;
                    SetupRunFX();
                }
                // Check if sprite need to be flipped based on the character's movement axis values.
                // FlipSprite();
                // Check for collisions.
                Vector3 curPosition = this.transform.position;
                Vector3 newPosition = curPosition + normalizedMovement * moddedRunSpeed * Time.deltaTime;
                MoveThePlayer(normalizedMovement, newPosition, curPosition);
                // print("TimedeltaTime: "+Time.deltaTime);
                // print("Norm Dir: "+normalizedMovement+", ModdedRunSpeed: "+moddedRunSpeed);
                float moveMag = (newPosition-curPosition).magnitude;
                testposcurpos = curPosition;
                //print("newPosX"+(newPosition.x-curPosition.x)+"   Y: "+(newPosition.y-curPosition.y));
            }
            else {
                running = false;
                moveLeft = false;
                moveRight = false;
            }
            // SetRunAnimation();
            if (running) {
                // Check if direction is different from last update.
                ShouldRunAnimChange();
                // Animate based on current sprites and bools.
                //RunAnim();
            }
            else {
                if (runningLastFrame) {
                    timeRunning = 0f;
                    StartIdling();
                    //mySpriteAnim.Play(idle);
                    //StartIdleAnimation();
                    runningLastFrame = false;
                }
                IdleAnim();
                // spriteRend.sprite = idleSprite;
                // Begin idle animations.
                // If I was running last frame, reset idle values; timer, ticks
                // firstIdleFrame = true;
            }
            
        }
    }

    public void StartIdling() {
        moveLeft = false;
        moveRight = false;
        lastMoveLeft = true;
        lastMoveRight = true;
        mySpriteAnim.Play(idle);
    }

    public void SetupRunFX() {
        Character_MotionFX charMo = charMotionFXPool.RequestMotionFX();
        charMo.inUse = true;
        charMo.transform.position = this.transform.position;
        charMo.gameObject.SetActive(true);
        StartCoroutine(SpawnRunFX(charMo, dustTotDur, dustSprites, dustTimings));
    }

    IEnumerator SpawnRunFX(Character_MotionFX charMo, float totalDuration, Sprite[] sprites, float[] timings) {
        float timer = 0f;
        int count = 0;
        while (timer < totalDuration) {
            timer += Time.deltaTime;
            if (timer > timings[count]) {
                charMo.spriteR.sprite = sprites[count];
                if (count < timings.Length-1) {
                    count++;
                }
            }
            yield return null;
        }
        charMo.inUse = false;
        charMo.spriteR.sprite = null;
        charMo.gameObject.SetActive(false);
    }

    public void FlipSpriteMouseBased () {
        // Flip sprite based on player mouse position.
        if (moIn.mousePosWorld2D.x < this.transform.position.x) {
            //spriteRend.flipX = true;
            characterSprite.transform.localScale = new Vector3(-1, 1 ,1);
            lookLeft = true;
            lookRight = false;
        }
        else {
            //spriteRend.flipX = false;
            characterSprite.transform.localScale = new Vector3(1, 1 ,1);
            lookLeft = false;
            lookRight = true;
        }
    }

    public void FlipSpritePositionBased(Vector3 otherPosition) {
        if (otherPosition.x < this.transform.position.x) {
            spriteRend.flipX = true;
        }
        else {
            spriteRend.flipX = false;
        }
    }

    public void FlipSpriteDirectionBased(Vector3 direction) {
        if (direction.x > 0f) {
            //spriteRend.flipX = true;
            characterSprite.transform.localScale = new Vector3(-1, 1 ,1);
        }
        else {
            //spriteRend.flipX = false;
            characterSprite.transform.localScale = new Vector3(1, 1 ,1);
        }
    }

    void StartIdleAnimation() {
        animTimer = 0f;
        tick = 0;
        idleOnCooldown = true;
        spriteRend.sprite = idleSprite;
    }
    void IdleAnim() {
        animTimer += Time.deltaTime;
        //print(animTimer);
        if (idleOnCooldown) {
            if (animTimer >= idleCooldownDur) {
                idleOnCooldown = false;
                animTimer = 0f;
                //print("incooldown");
            }
            return;
        }
        if (animTimer > idleOneTimings[tick]) {
            if (tick < idleOneTimings.Length-1) {
                spriteRend.sprite = idleOne[tick];
            }
            tick++;
        }
        if (tick >= idleOneTimings.Length) {
            animTimer = 0f;
            tick = 0;
            idleOnCooldown = true;
            spriteRend.sprite = idleSprite;
        }
    }

    void ShouldRunAnimChange() {
        if (moveLeft != lastMoveLeft || moveRight != lastMoveRight || lookLeft != lastLookLeft || lookRight != lastLookRight) {
            RunAnimChange();
        }
        lastMoveLeft = moveLeft;
        lastMoveRight = moveRight;
        lastLookLeft = lookLeft;
        lastLookRight = lookRight;
    }

    void RunAnimChange() {
        // Animation just changes, reset animation values.
        animTimer = 0f;
        tick = 0;
        startRunForwardBool = false;
        startRunBackwardsBool = false;
        runForwardBool = false;
        runBackwardsBool = false;
        // Check to see if the character should run forward or backwards based on its move and look direcrtions.
        if (moveLeft && lookLeft) {
            startRunForwardBool = true;
            mySpriteAnim.Play(forwardStart);
        }
        else if (moveLeft && lookRight) {
            startRunBackwardsBool = true;
            mySpriteAnim.Play(backStart);
        }
        else if (moveRight && lookRight) {
            startRunForwardBool = true;
            mySpriteAnim.Play(forwardStart);
        }
        else if (moveRight && lookLeft) {
            startRunBackwardsBool = true;
            mySpriteAnim.Play(backStart);
        }
        else {
            startRunForwardBool = false;
            startRunBackwardsBool = false;
        }
        // Set the current run animation sprites to the start run sprites of the correct direction.
        if (startRunForwardBool) {
            curSprites = startRunForward;
        }
        else if (startRunBackwardsBool) {
            curSprites = startRunBackwards;
        }
    }

    void RunAnim() {
        animTimer += Time.deltaTime * animRunSpeedMult;

        // Running forward starting frames. (played once)
        if (startRunForwardBool) {
            if (animTimer > startRunForwardTimings[tick]) {
                if (tick < startRunForwardTimings.Length-1) {
                    spriteRend.sprite = startRunForward[tick];
                    tick++;
                }
            }
            // Change to run forward loop.
            if (tick >= curSprites.Length) {
                animTimer = 0f;
                tick = 0;
                curSprites = runForward;
                startRunForwardBool = false;
                runForwardBool = true;
                return;
            }
        }
        // Running forward looping frames.
        else if (runForwardBool) {
            if (animTimer > runForwardTimings[tick]) {
                if (tick < runForwardTimings.Length-1) {
                    spriteRend.sprite = curSprites[tick];
                    tick++;
                }
            }
            // Restart animation.
            if (tick >= curSprites.Length) {
                animTimer = 0f;
                tick = 0;
                return;
            }
        }

        // Running backward starting frames. (played once)
        if (startRunBackwardsBool) {
            if (animTimer > startRunBackwardsTimings[tick]) {
                if (tick < startRunBackwardsTimings.Length-1) {
                    spriteRend.sprite = startRunBackwards[tick];
                    tick++;
                }
            }
            // Change to run backward loop.
            if (tick >= curSprites.Length) {
                animTimer = 0f;
                tick = 0;
                curSprites = runbackwards;
                startRunBackwardsBool = false;
                runBackwardsBool = true;
                return;
            }
        }
        // Running backward looping frames.
        else if (runBackwardsBool) {
            if (animTimer > runBackwardsTimings[tick]) {
                if (tick < runBackwardsTimings.Length-1) {
                    spriteRend.sprite = curSprites[tick];
                    tick++;
                }
            }
            // Restart animation.
            if (tick >= curSprites.Length) {
                animTimer = 0f;
                tick = 0;
                return;
            }
        }
    }

    public void StopInputMove(bool stopSpriteAnim = true, bool setIdlePlayerSprite = true) {
        canInputMove = false;
        running = false;
        lastMoveLeft = true;
        lastMoveRight = true;
        moveLeft = false;
        moveRight = false;

        if (stopSpriteAnim) mySpriteAnim.Stop();
        if (setIdlePlayerSprite) spriteRend.sprite = idleSprite;
        //animator.SetBool("Running", false);
        //SetRunAnimation();
    }

    public void MoveThePlayer(Vector3 _normMoveDir, Vector3 _newPosition, Vector3 _curPosition) {
        this.transform.position = colDetect.CollisionCheck(_normMoveDir, _newPosition, _curPosition);
        //print(" REAL    PosX"+(this.transform.position.x-testposcurpos.x)+"   Y: "+(this.transform.position.y-testposcurpos.y));
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Movement : MonoBehaviour
{
    [Header("Script References")]
    public MouseInputs moIn;
    public Character_CollisionDetection colDetect;
    [Header("To-set variables")]
    public float baseRunSpeed = 1f; 
    public SpriteRenderer spriteRend;
    public Animator animator;
    [Header("Read Only")]
    private float moddedRunSpeed, lessSpeedModifier, moreSpeedModifier;
    public bool canInputMove = true;
    public bool charCanFlip = true;
    public float normMagMovement;
    public float moveDirX, moveDirY;
    private bool lookLeft, lookRight, moveLeft, moveRight;
    private bool running;
    //public List<float> speedLessValues = new List<float>();
    //public List<float> speedMoreValues = new List<float>();
    // public List<float> speedAddValues = new List<float>();
    // public List<float> speedReduceValues = new List<float>();

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

    void FixedUpdate() {
        // Check if sprite needs to be flipped based on mouse position.
        if (moIn.mouseMoved && charCanFlip) {
          FlipSprite();
        }
        if (canInputMove) {
            // Detect Player Character movement.
            //Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
            //Vector3 normalizedMovement = movement.normalized;
            Vector3 normalizedMovement = new Vector3(moIn.x, moIn.y, 0f);
            normMagMovement = normalizedMovement.magnitude;
            // moveDirX = Input.GetAxis("Horizontal");
            // moveDirY = Input.GetAxis("Vertical");
            moveDirX = normalizedMovement.x;
            if (normalizedMovement.magnitude >= 0.01f) {
                running = true;
                animator.SetBool("Running", true);
                // Check run direction, left or right.
                if (moveDirX < 0) {
                    moveLeft = true;
                    moveRight = false;
                }
                else if (moveDirX > 0) {
                    moveLeft = false;
                    moveRight = true;
                }
                // Check if sprite need to be flipped based on the character's movement axis values.
                // FlipSprite();
            }
            else {
                running = false;
                moveLeft = false;
                moveRight = false;
                animator.SetBool("Running", false);
            }
            SetRunAnimation();
            // Check for collisions.
            Vector3 curPosition = this.transform.position;
            Vector3 newPosition = curPosition + normalizedMovement * moddedRunSpeed * Time.deltaTime;
            MoveThePlayer(normalizedMovement, newPosition, curPosition);
            //this.transform.position = curPosition + normalizedMovement * runSpeed * Time.deltaTime;
        }
    }

    public void FlipSprite () {
        // Flip sprite based on player mouse position.
        if (moIn.mousePosWorld2D.x < this.transform.position.x) {
            spriteRend.flipX = true;
            lookLeft = true;
            lookRight = false;
        }
        else {
            spriteRend.flipX = false;
            lookLeft = false;
            lookRight = true;
        }
    }

    void SetRunAnimation () {
        if (running) {
            if (moveLeft && lookLeft) {
                if (!animator.GetBool("RunForward")) {
                    animator.SetTrigger("StartRunForward");
                }
                animator.SetBool("RunForward", true);
                animator.SetBool("RunBackward", false);
                return;
            }
            if (moveLeft && lookRight) {
                if (!animator.GetBool("RunBackward")) {
                    animator.SetTrigger("StartRunBackward");
                }
                animator.SetBool("RunForward", false);
                animator.SetBool("RunBackward", true);
                return;
            }
            if (moveRight && lookLeft) {
                if (!animator.GetBool("RunBackward")) {
                    animator.SetTrigger("StartRunBackward");
                }
                animator.SetBool("RunForward", false);
                animator.SetBool("RunBackward", true);
                return;
            }
            if (moveRight && lookRight) {
                if (!animator.GetBool("RunForward")) {
                    animator.SetTrigger("StartRunForward");
                }
                animator.SetBool("RunForward", true);
                animator.SetBool("RunBackward", false);
                return;
            }
            if (!moveLeft && !moveRight) {
                if (!animator.GetBool("RunForward")) {
                    animator.SetTrigger("StartRunForward");
                }
                animator.SetBool("RunForward", true);
            }
        }
        else {
            animator.SetBool("RunForward", false);
            animator.SetBool("RunBackward", false);
        }
    }

    public void StopInputMove() {
        canInputMove = false;
        running = false;
        moveLeft = false;
        moveRight = false;
        animator.SetBool("Running", false);
        SetRunAnimation();
    }
    
    public void MoveThePlayer(Vector3 _normMoveDir, Vector3 _newPosition, Vector3 _curPosition) {
        this.transform.position = colDetect.CollisionCheck(_normMoveDir, _newPosition, _curPosition);
    }

    // public void SetupMoveThePlayerOverTime(float movementDuration, Vector3 targetPos, bool stopPlayerMove = false) {
    //     if (stopPlayerMove) {
    //         canMove = false;
    //     }
    //     StartCoroutine(MoveThePlayerOverTime(movementDuration, targetPos, stopPlayerMove));
    // }

    // public IEnumerator MoveThePlayerOverTime(float movementDuration, Vector3 targetPos, bool playerStopped) {
    //     float moveTimer = 0f;
    //     float speed = (this.transform.position-targetPos).magnitude / movementDuration;
    //     while (moveTimer < 1f) {
    //         moveTimer += Time.deltaTime/movementDuration;
    //         Vector3 curPosition = this.transform.position;
    //         Vector3 normDir = (targetPos - curPosition).normalized;
    //         Vector3 newPosition = curPosition + normDir * speed * runSpeed * Time.deltaTime;
    //         MoveThePlayer(normDir, newPosition, curPosition);
    //         yield return null;
    //     }
    //     if (playerStopped) {
    //         canMove = true;
    //     }
    // }
}

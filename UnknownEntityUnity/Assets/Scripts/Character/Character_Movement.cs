using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Movement : MonoBehaviour
{
    public MouseInputs moIn;
    public Character_CollisionDetection colDetect;
    private bool running;
    public float runSpeed = 1f; 
    public SpriteRenderer spriteRend;
    public Animator animator;
    public float normMagMovement;
    public float moveDirX;
    private bool lookLeft, lookRight, moveLeft, moveRight;

    void Update() {
        // Check if sprite needs to be flipped based on mouse position.
        if (moIn.mouseMoved) {
          FlipSprite();
        }
        // Detect Player Character movement.
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
        Vector3 normalizedMovement = movement.normalized;
        normMagMovement = normalizedMovement.magnitude;
        moveDirX = Input.GetAxis("Horizontal");
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
        Vector3 newPosition = curPosition + normalizedMovement * runSpeed * Time.deltaTime;
        this.transform.position = colDetect.CollisionCheck(normalizedMovement, runSpeed * Time.deltaTime, newPosition, curPosition);
        //this.transform.position = curPosition + normalizedMovement * runSpeed * Time.deltaTime;
    }

    void FlipSprite () {
        // Based on player mouse position.
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
        // Based on player movement axis values.
        // if (Input.GetAxis("Horizontal") < 0f) {
        //     spriteRend.flipX = true;
        // }
        // else if (Input.GetAxis("Horizontal") > 0f) {
        //     spriteRend.flipX = false;
        // }
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
}

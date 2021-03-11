using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//[DefaultExecutionOrder(-50)]
public class MouseInputs : MonoBehaviour
{
    public bool cursorOff;
    public Camera cam;
    public Vector3 mousePos;
    public Vector3 mousePosWorld;
    public Vector2 mousePosWorld2D, lastMousePosWorld2D;
    public bool attackButtonPressed, attackButtonHeld, mouseMoved;
    [Header("Actions")]
    public AttackButtonActions attackButtonActions;
    public OtherButtonActions otherButtonActions;
    [Header("Input Management")]
    public Vector2 inputMovementValues;
    public float x, y;
    public InputMaster inputMaster;
    public InputAction moveSkillAction;
    [Header("Grace Period")]
    public bool graceUsesFrames;
    // public float attackGraceDuration;
    // public int attackGraceFrames;
    // private float attackGraceTimer;
    // private int attackGraceFrameCount;
    // public bool attackButtonTapInGrace;
    [Header("Other Inputs")]
    public float weaponSwapGraceDuration;
    public int weaponSwapGraceFrames;
    private float weaponSwapGraceTimer;
    private int weaponSwapGraceFrameCount;
    public bool weaponSwapPressed;
    public bool interactPressed;
    public bool confirmPressed;
    public bool movementSkillPressed;

    void Awake() {
        inputMaster = new InputMaster();
        inputMaster.Player.Enable();
        inputMaster.Player.InputMovement.performed += ctx => inputMovementValues = ctx.ReadValue<Vector2>();
        inputMaster.Player.InputMovement.canceled += ctx => inputMovementValues = Vector2.zero;
        inputMaster.Player.AttackPressed.performed += ctx => attackButtonActions.AttackButtonPressedChecks();
        inputMaster.Player.AttackReleased.performed += ctx => attackButtonActions.AttackButtonReleasedChecks();
        inputMaster.Player.MovementSkill.performed += ctx => otherButtonActions.MoveSkillButtonPressedChecks();
        inputMaster.Player.Interact.performed += ctx => otherButtonActions.InteractButtonChecks();
        inputMaster.Player.WeaponSwap.performed += ctx => otherButtonActions.WeaponSwapButtonChecks();
        if (cursorOff) { Cursor.visible = false; }
    }

    void Update()
    {
        x = inputMovementValues.x;
        y = inputMovementValues.y;

        lastMousePosWorld2D = mousePosWorld2D;

        // Swap weapon key pressed. Also checks if grace frames are on.
        if (weaponSwapPressed) {
            // Grace period counted in Update() frames.
            if (graceUsesFrames) {
                weaponSwapGraceFrameCount++;
                if (weaponSwapGraceFrameCount >= weaponSwapGraceFrames) {
                    weaponSwapPressed = false;
                    weaponSwapGraceFrameCount = 0;
                }
            }
            // Grace period counted in deltaTime. (seconds)
            else {
                weaponSwapGraceTimer += Time.deltaTime;
                if (weaponSwapGraceTimer >= weaponSwapGraceDuration) {
                    weaponSwapPressed = false;
                    weaponSwapGraceTimer = 0f;
                }
            }
        }
        if (inputMaster.Player.WeaponSwap.triggered) {
            weaponSwapPressed = true;
        }

        // Interact key pressed. Used to interact with object, or pick up weapons.
        // interactPressed = false;
        // if (inputMaster.Player.Interact.triggered) {
        //     interactPressed = true;
        // }
        
        // Confirm key pressed.
        confirmPressed = false;
        if (inputMaster.Player.Confirm.triggered) {
            confirmPressed = true;
        }

        // Movement skill key pressed.
        // movementSkillPressed = false;
        // if (inputMaster.Player.MovementSkill.triggered) {
        //     movementSkillPressed = true;
        // }

        // Check if the mouse moved.
        // mousePos = UnityEngine.Input.mousePosition;
        mousePos = inputMaster.Player.MousePosition.ReadValue<Vector2>();
        //mousePos = PlayerInputManager.
        mousePosWorld = cam.ScreenToWorldPoint(mousePos);
        mousePosWorld2D = new Vector2(mousePosWorld.x, mousePosWorld.y);
        if (mousePosWorld2D != lastMousePosWorld2D) {
            mouseMoved = true;
        }
        else {
            mouseMoved = false;
        }

    }
}

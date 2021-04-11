using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//[DefaultExecutionOrder(-50)]
public class MouseInputs : MonoBehaviour
{
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
    [Header("Cursor")]
    public bool cursorOff;
    // public Texture2D cursorTexture;
    // public CursorMode cursorMode = CursorMode.Auto;
    // public Vector2 cursorHotSpot = Vector2.zero;

    void Awake() {
        inputMaster = new InputMaster();
        inputMaster.Player.Enable();
        inputMaster.Player.InputMovement.performed += ctx => inputMovementValues = ctx.ReadValue<Vector2>();
        inputMaster.Player.InputMovement.canceled += ctx => inputMovementValues = Vector2.zero;
        inputMaster.Player.AttackPressed.performed += ctx => attackButtonActions.AttackButtonPressedChecks();
        inputMaster.Player.AttackReleased.performed += ctx => attackButtonActions.AttackButtonReleasedChecks();
        inputMaster.Player.AttackSpecial.performed += ctx => attackButtonActions.SpecialAttackButtonPerformed();
        inputMaster.Player.AttackSpecial.canceled += ctx => attackButtonActions.SpecialAttackButtonCanceled();
        inputMaster.Player.MovementSkill.performed += ctx => otherButtonActions.MoveSkillButtonPressedChecks();
        inputMaster.Player.Interact.performed += ctx => otherButtonActions.InteractButtonChecks();
        inputMaster.Player.WeaponSwap.performed += ctx => otherButtonActions.WeaponSwapButtonChecks();
        inputMaster.Player.Pause.performed += ctx => otherButtonActions.OpenPauseMenu();
        // Cursor settings
        if (cursorOff) { Cursor.visible = false; }
        //Cursor.SetCursor(cursorTexture, cursorHotSpot, cursorMode);
    }

    void Update()
    {
        x = inputMovementValues.x;
        y = inputMovementValues.y;

        lastMousePosWorld2D = mousePosWorld2D;
        // Confirm key pressed.
        confirmPressed = false;
        if (inputMaster.Player.Confirm.triggered) {
            confirmPressed = true;
        }
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

    public void SwapToUIInputs() {
        inputMaster.Player.Disable();
        inputMaster.UI.Enable();
    }
    public void SwapToPlayerInputs() {
        inputMaster.Player.Enable();
        inputMaster.UI.Disable();
    }
}

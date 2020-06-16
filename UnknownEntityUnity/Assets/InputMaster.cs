// GENERATED AUTOMATICALLY FROM 'Assets/InputMaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputMaster : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""a13003c2-4cc8-4fbc-8869-ba77e43fa401"",
            ""actions"": [
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""e4b79122-3395-47b7-9c9c-849bfd763600"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""c703da65-6504-4dce-a5eb-8ea1d9e86367"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Weapon Swap"",
                    ""type"": ""Button"",
                    ""id"": ""1ab3e431-4260-4733-9456-019f1600e8be"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""InputMovement"",
                    ""type"": ""Value"",
                    ""id"": ""b2fced96-ee76-4983-8caa-77781d384117"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""b8de6daf-b2f0-4363-9f7d-e077963b645b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Confirm"",
                    ""type"": ""Button"",
                    ""id"": ""c035e3ac-8a5e-4ef2-8a10-71cc06942258"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MovementSkill"",
                    ""type"": ""Button"",
                    ""id"": ""ac9fa40d-fc81-4e04-81ff-18deaa1f2286"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f6163277-646a-491c-b887-4ef6fd251a08"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d6fb94e3-946b-4fe6-b5e5-abcd4227edea"",
                    ""path"": ""<Keyboard>/#(E)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b55a5d32-9ede-4b76-9d0d-e18bd7c98b77"",
                    ""path"": ""<Keyboard>/#(Q)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Weapon Swap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""efc1f484-7301-4432-ad68-80e40d85f782"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InputMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""036d41a5-d5c4-4def-9aca-855c14544b08"",
                    ""path"": ""<Keyboard>/#(W)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InputMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""665ed6af-856b-4f06-8b9a-90a7df0794fa"",
                    ""path"": ""<Keyboard>/#(S)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InputMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""9e764fb1-4b9b-4761-8fda-e5d77e07b744"",
                    ""path"": ""<Keyboard>/#(A)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InputMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e8dba835-fa1c-4a76-a91c-b4e9fc97a8cb"",
                    ""path"": ""<Keyboard>/#(D)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InputMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""e7f9ccdd-3664-4502-95e6-fcd8c7eb7880"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6f676e91-42a6-4f15-b75d-761cac2703fa"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""01381f5d-8918-4cd7-bcdf-b5b7e60d49e7"",
                    ""path"": ""<Keyboard>/numpadEnter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9e8b993d-8fe4-4b75-abab-5bcca1cedabf"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementSkill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Attack = m_Player.FindAction("Attack", throwIfNotFound: true);
        m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
        m_Player_WeaponSwap = m_Player.FindAction("Weapon Swap", throwIfNotFound: true);
        m_Player_InputMovement = m_Player.FindAction("InputMovement", throwIfNotFound: true);
        m_Player_MousePosition = m_Player.FindAction("MousePosition", throwIfNotFound: true);
        m_Player_Confirm = m_Player.FindAction("Confirm", throwIfNotFound: true);
        m_Player_MovementSkill = m_Player.FindAction("MovementSkill", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Attack;
    private readonly InputAction m_Player_Interact;
    private readonly InputAction m_Player_WeaponSwap;
    private readonly InputAction m_Player_InputMovement;
    private readonly InputAction m_Player_MousePosition;
    private readonly InputAction m_Player_Confirm;
    private readonly InputAction m_Player_MovementSkill;
    public struct PlayerActions
    {
        private @InputMaster m_Wrapper;
        public PlayerActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Attack => m_Wrapper.m_Player_Attack;
        public InputAction @Interact => m_Wrapper.m_Player_Interact;
        public InputAction @WeaponSwap => m_Wrapper.m_Player_WeaponSwap;
        public InputAction @InputMovement => m_Wrapper.m_Player_InputMovement;
        public InputAction @MousePosition => m_Wrapper.m_Player_MousePosition;
        public InputAction @Confirm => m_Wrapper.m_Player_Confirm;
        public InputAction @MovementSkill => m_Wrapper.m_Player_MovementSkill;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Attack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Interact.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @WeaponSwap.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponSwap;
                @WeaponSwap.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponSwap;
                @WeaponSwap.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponSwap;
                @InputMovement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInputMovement;
                @InputMovement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInputMovement;
                @InputMovement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInputMovement;
                @MousePosition.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMousePosition;
                @Confirm.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnConfirm;
                @Confirm.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnConfirm;
                @Confirm.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnConfirm;
                @MovementSkill.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovementSkill;
                @MovementSkill.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovementSkill;
                @MovementSkill.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovementSkill;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @WeaponSwap.started += instance.OnWeaponSwap;
                @WeaponSwap.performed += instance.OnWeaponSwap;
                @WeaponSwap.canceled += instance.OnWeaponSwap;
                @InputMovement.started += instance.OnInputMovement;
                @InputMovement.performed += instance.OnInputMovement;
                @InputMovement.canceled += instance.OnInputMovement;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @Confirm.started += instance.OnConfirm;
                @Confirm.performed += instance.OnConfirm;
                @Confirm.canceled += instance.OnConfirm;
                @MovementSkill.started += instance.OnMovementSkill;
                @MovementSkill.performed += instance.OnMovementSkill;
                @MovementSkill.canceled += instance.OnMovementSkill;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnAttack(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnWeaponSwap(InputAction.CallbackContext context);
        void OnInputMovement(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnConfirm(InputAction.CallbackContext context);
        void OnMovementSkill(InputAction.CallbackContext context);
    }
}

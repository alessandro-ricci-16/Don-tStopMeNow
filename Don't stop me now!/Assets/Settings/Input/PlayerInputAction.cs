//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Settings/Input/PlayerInputAction.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputAction: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputAction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputAction"",
    ""maps"": [
        {
            ""name"": ""OnGround"",
            ""id"": ""52250a99-8d64-407b-ab9b-447ca74829bb"",
            ""actions"": [
                {
                    ""name"": ""Acceleration"",
                    ""type"": ""Value"",
                    ""id"": ""28026163-0339-4b25-853c-a3cf61a5b326"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""594174cf-b78a-4c15-94ce-4567d79daafc"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Acceleration"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""fe74b41f-ddf2-4c9f-a735-c824419036eb"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Acceleration"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""c5535529-d327-415a-bc87-52de4e9e39bb"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Acceleration"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""d3bb80f0-54d1-4838-9373-548915d01846"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Acceleration"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""ddd29c01-2a82-49c9-aa0f-a2e55ea7186f"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Acceleration"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""8d8806d4-3fcd-4454-a2e9-1216c0c19025"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Acceleration"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""OnAir"",
            ""id"": ""58d490fd-e149-4695-90eb-70cd8a9b52a1"",
            ""actions"": [
                {
                    ""name"": ""GroundPound"",
                    ""type"": ""Button"",
                    ""id"": ""6cb290c3-20c0-4772-b098-475854a2ea9d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""148ef6e0-ca99-4d71-ba7f-6cab05b18c24"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""308cc2e7-6cd4-4c25-9b86-06a13273c0a0"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""591078b5-4f70-4169-abc5-35093b6de0ff"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f71070df-22b7-4d66-aefa-fc4babf94a9c"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GroundPound"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Jump"",
            ""id"": ""46fc3664-33f5-4f10-869e-c32bbcc9bae4"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""52926afe-ca6f-4860-a822-5b440cc5ff62"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a02b2365-e2c1-4208-b138-bbcef89fc974"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""16e38777-0ff7-4398-bea3-3ddfbba3002c"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // OnGround
        m_OnGround = asset.FindActionMap("OnGround", throwIfNotFound: true);
        m_OnGround_Acceleration = m_OnGround.FindAction("Acceleration", throwIfNotFound: true);
        // OnAir
        m_OnAir = asset.FindActionMap("OnAir", throwIfNotFound: true);
        m_OnAir_GroundPound = m_OnAir.FindAction("GroundPound", throwIfNotFound: true);
        m_OnAir_Dash = m_OnAir.FindAction("Dash", throwIfNotFound: true);
        // Jump
        m_Jump = asset.FindActionMap("Jump", throwIfNotFound: true);
        m_Jump_Jump = m_Jump.FindAction("Jump", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // OnGround
    private readonly InputActionMap m_OnGround;
    private List<IOnGroundActions> m_OnGroundActionsCallbackInterfaces = new List<IOnGroundActions>();
    private readonly InputAction m_OnGround_Acceleration;
    public struct OnGroundActions
    {
        private @PlayerInputAction m_Wrapper;
        public OnGroundActions(@PlayerInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Acceleration => m_Wrapper.m_OnGround_Acceleration;
        public InputActionMap Get() { return m_Wrapper.m_OnGround; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(OnGroundActions set) { return set.Get(); }
        public void AddCallbacks(IOnGroundActions instance)
        {
            if (instance == null || m_Wrapper.m_OnGroundActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_OnGroundActionsCallbackInterfaces.Add(instance);
            @Acceleration.started += instance.OnAcceleration;
            @Acceleration.performed += instance.OnAcceleration;
            @Acceleration.canceled += instance.OnAcceleration;
        }

        private void UnregisterCallbacks(IOnGroundActions instance)
        {
            @Acceleration.started -= instance.OnAcceleration;
            @Acceleration.performed -= instance.OnAcceleration;
            @Acceleration.canceled -= instance.OnAcceleration;
        }

        public void RemoveCallbacks(IOnGroundActions instance)
        {
            if (m_Wrapper.m_OnGroundActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IOnGroundActions instance)
        {
            foreach (var item in m_Wrapper.m_OnGroundActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_OnGroundActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public OnGroundActions @OnGround => new OnGroundActions(this);

    // OnAir
    private readonly InputActionMap m_OnAir;
    private List<IOnAirActions> m_OnAirActionsCallbackInterfaces = new List<IOnAirActions>();
    private readonly InputAction m_OnAir_GroundPound;
    private readonly InputAction m_OnAir_Dash;
    public struct OnAirActions
    {
        private @PlayerInputAction m_Wrapper;
        public OnAirActions(@PlayerInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @GroundPound => m_Wrapper.m_OnAir_GroundPound;
        public InputAction @Dash => m_Wrapper.m_OnAir_Dash;
        public InputActionMap Get() { return m_Wrapper.m_OnAir; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(OnAirActions set) { return set.Get(); }
        public void AddCallbacks(IOnAirActions instance)
        {
            if (instance == null || m_Wrapper.m_OnAirActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_OnAirActionsCallbackInterfaces.Add(instance);
            @GroundPound.started += instance.OnGroundPound;
            @GroundPound.performed += instance.OnGroundPound;
            @GroundPound.canceled += instance.OnGroundPound;
            @Dash.started += instance.OnDash;
            @Dash.performed += instance.OnDash;
            @Dash.canceled += instance.OnDash;
        }

        private void UnregisterCallbacks(IOnAirActions instance)
        {
            @GroundPound.started -= instance.OnGroundPound;
            @GroundPound.performed -= instance.OnGroundPound;
            @GroundPound.canceled -= instance.OnGroundPound;
            @Dash.started -= instance.OnDash;
            @Dash.performed -= instance.OnDash;
            @Dash.canceled -= instance.OnDash;
        }

        public void RemoveCallbacks(IOnAirActions instance)
        {
            if (m_Wrapper.m_OnAirActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IOnAirActions instance)
        {
            foreach (var item in m_Wrapper.m_OnAirActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_OnAirActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public OnAirActions @OnAir => new OnAirActions(this);

    // Jump
    private readonly InputActionMap m_Jump;
    private List<IJumpActions> m_JumpActionsCallbackInterfaces = new List<IJumpActions>();
    private readonly InputAction m_Jump_Jump;
    public struct JumpActions
    {
        private @PlayerInputAction m_Wrapper;
        public JumpActions(@PlayerInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_Jump_Jump;
        public InputActionMap Get() { return m_Wrapper.m_Jump; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(JumpActions set) { return set.Get(); }
        public void AddCallbacks(IJumpActions instance)
        {
            if (instance == null || m_Wrapper.m_JumpActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_JumpActionsCallbackInterfaces.Add(instance);
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
        }

        private void UnregisterCallbacks(IJumpActions instance)
        {
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
        }

        public void RemoveCallbacks(IJumpActions instance)
        {
            if (m_Wrapper.m_JumpActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IJumpActions instance)
        {
            foreach (var item in m_Wrapper.m_JumpActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_JumpActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public JumpActions @Jump => new JumpActions(this);
    public interface IOnGroundActions
    {
        void OnAcceleration(InputAction.CallbackContext context);
    }
    public interface IOnAirActions
    {
        void OnGroundPound(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
    }
    public interface IJumpActions
    {
        void OnJump(InputAction.CallbackContext context);
    }
}

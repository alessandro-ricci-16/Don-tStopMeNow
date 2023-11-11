using System;
using System.Collections;
using System.Collections.Generic;
using Ice_Cube.States;
using ScriptableObjects;
using UnityEngine;

namespace Ice_Cube.States
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class IceCubeStateManager : MonoBehaviour
    {
        private Dictionary<IceCubeStatesEnum, IceCubeState> _stateDictionary =
            new Dictionary<IceCubeStatesEnum, IceCubeState>();

        private Queue<IceCubeState> _stateQueue = new Queue<IceCubeState>();
        private PlayerInputAction _playerInputAction;
        private IceCubeState _currentState;
        private Rigidbody2D _rigidbody2D;

        public IceCubeParameters _parameters;

        //caching the ground check
        private bool _isGrounded;
        public bool IsGrounded => _isGrounded;

        /// <summary>
        /// Initializes the state dictionary by creating instances of all IceCubeState classes
        /// based on the IceCubeStatesEnum values.
        /// </summary>
        private void OnEnable()
        {
            EventManager.StartListening(EventNames.OnGround, ListenToGround);
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            
            if (_currentState.ChangeStateOnFinish())
            {
                //if the current state should be switched on the end we have to set OnGround or OnAir
                if (_isGrounded)
                    SetNextState(IceCubeStatesEnum.OnGround);
                else
                    SetNextState(IceCubeStatesEnum.OnAir);
            }
            
        }

        private void OnDestroy()
        {
            EventManager.StopListening(EventNames.OnGround, ListenToGround);
        }

        private void ListenToGround(bool value)
        {
            if (_isGrounded != value)
            {
                _isGrounded = value;
                if (_isGrounded)
                {
                    SetNextState(IceCubeStatesEnum.OnGround);
                }
                else
                {
                    SetNextState(IceCubeStatesEnum.OnAir);
                }
            }
        }


        /// <summary>
        /// Returns the current state.
        /// </summary>
        /// <returns></returns>
        public IceCubeState GetCurrentState()
        {
            return _currentState;
        }

        /// <summary>
        /// Creates an instance of an IceCubeState based on the specified IceCubeStatesEnum value. The class name should be the same as the enum value + "State".
        /// </summary>
        /// <param name="stateEnum">The IceCubeStatesEnum value to determine the state class to create.</param>
        /// <returns>The newly created IceCubeState instance, or throws an exception if the class is not found.</returns>
        private IceCubeState CreateIceCubeState(IceCubeStatesEnum stateEnum)
        {
            // Get the corresponding class name based on the enum value.
            string className = stateEnum + "State";

            // Get the Type of the state class using reflection within the shared namespace.
            Type stateType = Type.GetType("Ice_Cube.States." + className);

            if (stateType != null)
            {
                // Create an array of parameters to pass to the constructor
                object[] constructorParams = { _playerInputAction, _rigidbody2D, _parameters };

                // Use Activator.CreateInstance with parameters
                // newState is an instance of IceCubeState created with the provided parameters
                return (IceCubeState)Activator.CreateInstance(stateType, constructorParams);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Class related to the enum not found. It should be named " +
                                                      className);
            }
        }

        private void Init()
        {
            foreach (var state in Enum.GetValues(typeof(IceCubeStatesEnum)))
            {
                _stateDictionary[(IceCubeStatesEnum)state] = CreateIceCubeState((IceCubeStatesEnum)state);
            }

            _currentState = _stateDictionary[IceCubeStatesEnum.OnAir];
        }

        /// <summary>
        /// Sets the next state specified by the given enum and returns it.
        /// Additionally, it calls the EnterState method of the newly set state.
        /// The new state will not be automatically set, but only if is the current state is interuptable.
        /// </summary>
        /// <param name="stateEnum">The enum value representing the desired state.</param>
        public void SetNextState(IceCubeStatesEnum stateEnum)
        {
            if (stateEnum != _currentState.GetEnumState() && _stateDictionary.TryGetValue(stateEnum, out var stateInstance))
            {
                if (_currentState.ShouldBeInterrupted())
                    SwitchState(stateInstance);
            }
        }

        /// <summary>
        /// Should be called before setting the next state to set the player input action.
        /// </summary>
        /// <param name="playerInputAction"></param>
        public void SetPlayerInputAction(PlayerInputAction playerInputAction)
        {
            _playerInputAction = playerInputAction;
            Init();
        }

        /// <summary>
        /// This method is used to switch the current state. It also calls the EnterState method of the newly set state.
        /// </summary>
        /// <param name="state"> The state that is going to be set</param>
        private void SwitchState(IceCubeState state)
        {
            if (_currentState.GetEnumState() != state.GetEnumState())
            {
                _currentState = state;
                _currentState.EnterState();
            }
        }
    }
}
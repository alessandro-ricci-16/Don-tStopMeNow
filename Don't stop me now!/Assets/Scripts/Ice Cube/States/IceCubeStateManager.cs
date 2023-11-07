using System;
using System.Collections;
using System.Collections.Generic;
using Ice_Cube.States;
using ScriptableObjects;
using UnityEngine;

namespace Ice_Cube.States
{
    public class IceCubeStateManager : MonoBehaviour
    {
        private Dictionary<IceCubeStatesEnum, IceCubeState> _stateDictionary =
            new Dictionary<IceCubeStatesEnum, IceCubeState>();

        private Queue<IceCubeState> _stateQueue = new Queue<IceCubeState>();
        private PlayerInputAction _playerInputAction;
        private IceCubeState _currentState;
        
        //caching the ground check
        private bool _isGrounded;
        private float _stateDurationLeft;

        /// <summary>
        /// Initializes the state dictionary by creating instances of all IceCubeState classes
        /// based on the IceCubeStatesEnum values.
        /// </summary>
        private void OnEnable()
        {
            EventManager.StartListening(EventNames.OnGround, ListenToGround);

            foreach (var state in Enum.GetValues(typeof(IceCubeStatesEnum)))
            {
                _stateDictionary[(IceCubeStatesEnum)state] = CreateIceCubeState((IceCubeStatesEnum)state);
            }
            _currentState = _stateDictionary[IceCubeStatesEnum.OnAir];
        }

        private void Update()
        {
            if (_stateDurationLeft > 0)
            {
                _stateDurationLeft -= Time.deltaTime;
            }
            else
            {
                if (_currentState.ShouldBeSwitchedOnEnd())
                {
                    //if the current state should be switched on the end we have to dequeue the previous state and set it as the current state
                    switchState( _stateQueue.Dequeue());
                }
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
        /// Creates an instance of an IceCubeState based on the specified IceCubeStatesEnum value. The class name should be the same as the enum value + "State".
        /// </summary>
        /// <param name="stateEnum">The IceCubeStatesEnum value to determine the state class to create.</param>
        /// <returns>The newly created IceCubeState instance, or throws an exception if the class is not found.</returns>
        private static IceCubeState CreateIceCubeState(IceCubeStatesEnum stateEnum)
        {
            // Get the corresponding class name based on the enum value.
            string className = stateEnum + "State";

            // Get the Type of the state class using reflection within the shared namespace.
            Type stateType = Type.GetType("Ice_Cube.States." + className);

            if (stateType != null)
            {
                // If the Type exists, create an instance using Activator.CreateInstance.
                return (IceCubeState)Activator.CreateInstance(stateType);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Class related to the enum not found. It should be named " +
                                                      className);
            }
        }

        /// <summary>
        /// Retrieves the instance of an IceCubeState based on the specified IceCubeStatesEnum value.
        /// </summary>
        /// <param name="stateEnum">The IceCubeStatesEnum value to determine the state instance to retrieve.</param>
        /// <returns>The retrieved IceCubeState instance, or null if the state is not found.</returns>
        public IceCubeState GetStateInstance(IceCubeStatesEnum stateEnum)
        {
            if (_stateDictionary.TryGetValue(stateEnum, out var stateInstance))
            {
                return stateInstance;
            }

            Debug.LogError("State not found - This should not happen. In case it does, please contact Emanuele :)");
            return null;
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
        /// Sets the next state specified by the given enum and returns it.
        /// Additionally, it calls the EnterState method of the newly set state.
        /// The new state will not be automatically set, but only if is the current state is interuptable.
        /// </summary>
        /// <param name="stateEnum">The enum value representing the desired state.</param>
        public void SetNextState(IceCubeStatesEnum stateEnum)
        {
            _stateDurationLeft = _currentState.GetDurationLeft();
            if (_stateDurationLeft==0 && _stateDictionary.TryGetValue(stateEnum, out var stateInstance))
            {
                
                if (stateInstance.ShouldBeSwitchedOnEnd())
                {
                    //if the next state should be switched on the end we have to queue the current state
                    //IMPORTANT DO NOT CHANGE THE ORDER OF THE FOLLOWING LINES
                    _stateQueue.Enqueue(_currentState);
                    
                }
                switchState(stateInstance);
            }
        }

        /// <summary>
        /// Should be called before setting the next state to set the player input action.
        /// </summary>
        /// <param name="playerInputAction"></param>
        public void SetPlayerInputAction(PlayerInputAction playerInputAction)
        {
            _playerInputAction = playerInputAction;
        }
        /// <summary>
        /// This method is used to switch the current state. It also calls the EnterState method of the newly set state.
        /// </summary>
        /// <param name="state"> The state that is going to be set</param>
        private void switchState(IceCubeState state)
        {
            Debug.Log("Previous state: " + _currentState.GetEnumState() + " New state: " + state.GetEnumState());
            _currentState = state;
            _currentState.EnterState(_playerInputAction);
        }
    }
}
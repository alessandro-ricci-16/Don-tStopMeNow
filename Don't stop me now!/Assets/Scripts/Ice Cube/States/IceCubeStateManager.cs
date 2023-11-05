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

        private PlayerInputAction _playerInputAction;

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
        /// Initializes the state dictionary by creating instances of all IceCubeState classes
        /// based on the IceCubeStatesEnum values.
        /// </summary>
        private void OnEnable()
        {
            foreach (var state in Enum.GetValues(typeof(IceCubeStatesEnum)))
            {
                _stateDictionary[(IceCubeStatesEnum)state] = CreateIceCubeState((IceCubeStatesEnum)state);
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
        /// Sets the next state specified by the given enum and returns it.
        /// Additionally, it calls the EnterState method of the newly set state.
        /// </summary>
        /// <param name="stateEnum">The enum value representing the desired state.</param>
        /// <returns>The instance of the newly set state, or null if the state is not found.</returns>
        public IceCubeState SetNextState(IceCubeStatesEnum stateEnum)
        {
            if (_stateDictionary.TryGetValue(stateEnum, out var stateInstance))
            {
                stateInstance.EnterState(_playerInputAction);
                return stateInstance;
            }

            Debug.LogError("State not found - This should not happen. In case it does, please contact Emanuele :)");
            return null;
        }

        //method to set private input action
        public void SetPlayerInputAction(PlayerInputAction playerInputAction)
        {
            _playerInputAction = playerInputAction;
        }
    }
}
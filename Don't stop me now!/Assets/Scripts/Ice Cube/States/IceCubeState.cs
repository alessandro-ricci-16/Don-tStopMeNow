using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;

namespace Ice_Cube.States
{
    /// <summary>
    /// Base class for IceCube states. Each concrete state should inherit from this class.
    /// </summary>
    public abstract class IceCubeState
    {
        
        private IceCubeStatesEnum _currentStateEnum;
    
        /// <summary>
        /// Gets the current state as an IceCubeStatesEnum value.
        /// </summary>
        /// <returns>The current state as an enum value.</returns>
        public abstract IceCubeStatesEnum GetCurrentState();

        /// <summary>
        /// Method called when entering this state. Derived classes should implement this method.
        /// </summary>
        /// <param name="playerInputAction"></param>
        public abstract void EnterState(PlayerInputAction playerInputAction);
        //TODO PASSING PARAMETERS IS PROBABLY BAD REMOVE IT LATER
        public abstract void PerformPhysicsAction(Rigidbody2D rigidbody2D, IceCubeParameters parameters,
            Vector2 currentDirection);
    }   
}
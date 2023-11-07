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
        public abstract IceCubeStatesEnum GetEnumState();

        /// <summary>
        /// Method called when entering this state. Derived classes should implement this method.
        /// </summary>
        /// <param name="playerInputAction"></param>
        public abstract void EnterState(PlayerInputAction playerInputAction);

        //TODO PASSING PARAMETERS IS PROBABLY BAD REMOVE IT LATER
        public abstract void PerformPhysicsAction(Rigidbody2D rigidbody2D, IceCubeParameters parameters,
            Vector2 currentDirection);

        /// <summary>
        /// This method can be called to understand if we can interrupt the current state or not. The state manager will start a counter based on the duration: only when it ends it will switch to next state.
        /// </summary>
        /// <returns>Defaul value = 0 (which means it can interrupted in any moments). Ovverride it to change</returns>
        public virtual float GetDurationLeft()
        {
            return 0;
        }

        /// <summary>
        /// This method is called to understand if the state should be switched to the next one when it's duration is 0. If it returns true, the state will be switched to the one in the queue.
        /// Override when I want that my state is switched to the next one when it's duration is 0.
        /// </summary>
        /// <returns></returns>
        public virtual bool ShouldBeSwitchedOnEnd()
        {
            return false;
        }
    }
}
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
        protected PlayerInputAction PlayerInputAction;
        protected Rigidbody2D Rigidbody2D;
        protected IceCubeParameters Parameters;
        
        //constructor that receives the playerInputAction, the rigidBody2D and the parameters
        protected IceCubeState(PlayerInputAction playerInputAction, Rigidbody2D rigidbody2D, IceCubeParameters parameters)
        {
            PlayerInputAction = playerInputAction;
            Rigidbody2D = rigidbody2D;
            Parameters = parameters;
        }
        
        
        /// <summary>
        /// Gets the current state as an IceCubeStatesEnum value.
        /// </summary>
        /// <returns>The current state as an enum value.</returns>
        public abstract IceCubeStatesEnum GetEnumState();

        /// <summary>
        /// Method called when entering this state. Derived classes should implement this method.
        /// </summary>
        public abstract void EnterState();
        
        public abstract void PerformPhysicsAction(Vector2 currentDirection);

        /// <summary>
        /// Should the State Manager check at every update if the state can be interrupted?
        /// Should be false for OnGround and OnAir.
        /// </summary>
        /// <returns></returns>
        public virtual bool ChangeStateOnFinish()
        {
            return false;
        }
        
        /// <summary>
        /// Should the state be interrupted?
        /// Should always be true for OnGround and OnAir.
        /// </summary>
        /// <returns></returns>
        public virtual bool ShouldBeInterrupted()
        {
            return true;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Ice Cube Parameters", menuName = "ScriptableObjects/IceCubeParameters")]
    public class IceCubeParameters : ScriptableObject
    {
        [Header("Movement")]
        public float defaultSpeed = 5.0f;
        public float slowSpeed = 3.5f;
        public float fastSpeed = 7.0f;
        public float acceleration = 20.0f;
        [Tooltip("Must be > 0, minus sign is added in code")]
        public float deceleration = 20.0f;
    
        [Header("Jump")]
        [Tooltip("Max height reached by jump")]
        public float maxJumpHeight = 4.0f;
        public float upwardGravityMultiplier = 6.0f;
        public float downwardGravityMultiplier = 8.0f;
        public float defaultGravityMultiplier = 1.0f;
        public float maxJumpBufferTime = 0.1f;
        public float maxCoyoteTime = 0.1f;
        public float maxWallJumpBufferTime = 0.2f;
        public float maxWallCoyoteTime = 0.2f;
    }
    
}


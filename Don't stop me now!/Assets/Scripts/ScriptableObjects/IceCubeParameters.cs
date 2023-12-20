using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Ice Cube Parameters", menuName = "ScriptableObjects/IceCubeParameters")]
    public class IceCubeParameters : ScriptableObject
    {
        [Header("Movement")] public float defaultSpeed = 5.0f;
        public float slowSpeed = 3.5f;
        public float fastSpeed = 7.0f;
        public float acceleration = 20.0f;
        public float maxFallingSpeed = 69.0f;

        [Tooltip("Must be > 0, minus sign is added in code")]
        public float deceleration = 20.0f;

        [Header("Jump")] [Tooltip("Max height reached by jump")]
        public float maxJumpHeight = 4.0f;

        [Tooltip("Max height reached by wall jump")]
        public float maxWallJumpHeight = 4.0f;

        [Tooltip("Gravity scale when the ice cube is going upwards; increase for faster and more vertical jump")]
        public float upwardGravityScale = 6.0f;

        [Tooltip("Gravity scale when the ice cube is going downwards; increase for faster fall in jumps")]
        public float downwardGravityScale = 8.0f;

        [Tooltip("Time that passes before halving the velocity after the player releases the jump button")]
        public float jumpReleaseTime = 0.1f;

        [Header("Ground pound")] public float groundPoundSpeed = 25.0f;
        public float groundPoundTimeScale = 0.2f;
        public float groundPoundTimeSlowDown = 0.2f;

        [FormerlySerializedAs("dashSpeed")] [Header("Dash")]
        public float dashIntensity = 20.0f;

        public float dashDuration = 0.4f;
        public int maxDashesNumber = 1;

        [Header("Input parameters")]
        [Tooltip("How much time (in seconds) before actually reaching the ground you're allowed to press jump")]
        public float maxJumpBufferTime = 0.1f;

        [Tooltip("How much time (in seconds) the ice cube is allowed to jump after leaving the ground")]
        public float maxCoyoteTime = 0.1f;

        [Tooltip("Same as the jump buffer time but for the wall jump; leave a little bigger than the normal one")]
        public float maxWallJumpBufferTime = 0.2f;

        [Tooltip("Same as the coyote time but for the wall jump; leave a little bigger than the normal one")]
        public float maxWallCoyoteTime = 0.2f;

        [Tooltip("Max number of wall jumps the player can perform before touching the ground again")]
        public int maxWallJumpsNumber = 1;
    }
}
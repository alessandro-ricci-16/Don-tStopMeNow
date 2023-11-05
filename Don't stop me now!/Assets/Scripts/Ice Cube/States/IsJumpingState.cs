using ScriptableObjects;
using UnityEngine;

namespace Ice_Cube.States
{
    public class IsJumpingState: IceCubeState
    {
        public override  IceCubeStatesEnum GetCurrentState()
        {
            return IceCubeStatesEnum.IsJumping;
        }

        public override void EnterState(PlayerInputAction playerInputAction)
        {
            //TODO what to do in this case?
            playerInputAction.OnGround.Disable();
        }
        /// <summary>
        /// The function computes the jump speed necessary to reach the 
        /// standard maxJumpHeight and sets the rigidbody y velocity to that value.
        /// IMPORTANT: the function does NOT check if the player is on the ground.
        /// </summary>
        public override void PerformPhysicsAction(Rigidbody2D rigidbody2D, IceCubeParameters parameters,
            Vector2 currentDirection)
        {
            // compute jump speed to reach maxJumpHeight
            float jumpForce = Mathf.Sqrt(-2f * Physics2D.gravity.y *
                                         parameters.upwardGravityScale * parameters.maxJumpHeight);
            // if not already updated, set the gravity multiplier to the upwards gravity scale
            // (otherwise it will update next frame and create problems)
            rigidbody2D.gravityScale = parameters.upwardGravityScale;
            jumpForce -= rigidbody2D.velocity.y;
            rigidbody2D.AddForce(jumpForce*Vector2.up, ForceMode2D.Impulse);
            //SetGrounded(false);
            
        }
    }
}
using ScriptableObjects;
using UnityEngine;

namespace Ice_Cube.States
{
    public class IsJumpingState: IceCubeState
    {
        //use the base constructor
        public IsJumpingState(PlayerInputAction playerInputAction, Rigidbody2D rigidbody2D, IceCubeParameters parameters)
            : base(playerInputAction, rigidbody2D, parameters)
        {
        }
        public override  IceCubeStatesEnum GetEnumState()
        {
            return IceCubeStatesEnum.IsJumping;
        }

        public override void EnterState()
        {
            PlayerInputAction.OnAir.Disable();
            PlayerInputAction.OnGround.Disable();
        }
        /// <summary>
        /// The function computes the jump speed necessary to reach the 
        /// standard maxJumpHeight and sets the rigidbody y velocity to that value.
        /// IMPORTANT: the function does NOT check if the player is on the ground.
        /// </summary>
        public override void PerformPhysicsAction(Vector2 currentDirection)
        {
            // compute jump speed to reach maxJumpHeight
            float jumpForce = Mathf.Sqrt(-2f * Physics2D.gravity.y *
                                         Parameters.upwardGravityScale * Parameters.maxJumpHeight);
            // if not already updated, set the gravity multiplier to the upwards gravity scale
            // (otherwise it will update next frame and create problems)
            Rigidbody2D.gravityScale = Parameters.upwardGravityScale;
            jumpForce -= Rigidbody2D.velocity.y;
            Rigidbody2D.AddForce(jumpForce*Vector2.up, ForceMode2D.Impulse);
            
        }
        
    }
}
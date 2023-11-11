using ScriptableObjects;
using UnityEngine;

namespace Ice_Cube.States
{
    public class IsWallJumpingState : IceCubeState
    {
        //use the base constructor
        public IsWallJumpingState(PlayerInputAction playerInputAction, Rigidbody2D rigidbody2D, IceCubeParameters parameters)
            : base(playerInputAction, rigidbody2D, parameters)
        {
        }
        private bool _inExecution = false;
        public override void EnterState()
        {
            PlayerInputAction.OnGround.Disable();
            PlayerInputAction.OnAir.Disable();
            _inExecution=true;
        }
        
        public override void PerformPhysicsAction(Vector2 currentDirection)
        {
            // compute jump speed to reach maxJumpHeight
            float jumpForce = Mathf.Sqrt(-2f * Physics2D.gravity.y *
                                         Parameters.upwardGravityScale * Parameters.maxWallJumpHeight);
            // if not already updated, set the gravity multiplier to the upwards gravity scale
            // (otherwise it will update next frame and create problems)
            Rigidbody2D.gravityScale = Parameters.upwardGravityScale;
            jumpForce -= Rigidbody2D.velocity.y;
            Rigidbody2D.AddForce(jumpForce*Vector2.up, ForceMode2D.Impulse);
            //SetGrounded(false);
            //since we only have to execute for 1 fixed update we can set this to true
            _inExecution = false;
        }
        
        
        public override bool ShouldBeInterrupted()
        {
            return !_inExecution;
        }

        public override bool ChangeStateOnFinish()
        {
            return true;
        }

        public override IceCubeStatesEnum GetEnumState()
        {
            return IceCubeStatesEnum.IsWallJumping;
        }
    }
}
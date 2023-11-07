using ScriptableObjects;
using UnityEngine;

namespace Ice_Cube.States
{
    public class IsAcceleratingState : IceCubeState
    {
        float _xinput;

        public override IceCubeStatesEnum GetEnumState()
        {
            return IceCubeStatesEnum.IsAccelerating;
        }

        public override void EnterState(PlayerInputAction playerInputAction)
        {
            playerInputAction.OnGround.Acceleration.performed += ctx => Accelerate(ctx.ReadValue<float>());
            playerInputAction.OnGround.Acceleration.canceled += ctx => _xinput = 0; // Reset _xinput when the input is released
        }

        public override void PerformPhysicsAction(Rigidbody2D rigidbody2D, IceCubeParameters parameters,
            Vector2 currentDirection)
        {
            Vector2 rigidbody2DVelocity= rigidbody2D.velocity;
            // speedInput > 0 if user input and current direction are concordant
            float speedInput = _xinput * Mathf.Sign(currentDirection.x);
        
            // case 1: xInput in the current direction of the cube
            // add force to increase speed to match fast speed
            if (speedInput > 0.0f)
            {
                if (Mathf.Abs(rigidbody2DVelocity.x) < parameters.fastSpeed)
                    rigidbody2D.AddForce(parameters.acceleration * currentDirection, ForceMode2D.Force);
            }
            // case 2: xInput in opposite direction of the cube
            // add force to decrease speed to match slow speed
            else if (speedInput < 0.0f)
            {
                if (Mathf.Abs(rigidbody2DVelocity.x) > parameters.slowSpeed)
                    rigidbody2D.AddForce(- parameters.deceleration * currentDirection, ForceMode2D.Force);
            }
        }

        private void Accelerate(float acceleration)
        {
            _xinput = acceleration;
        }
        public override bool ShouldBeSwitchedOnEnd()
        {
            //if the input is equal to 0 then this should be immediately be switched
            return _xinput==0;
        }
    }
}
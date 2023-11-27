using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ice_Cube.States
{
    public class IsAcceleratingState : IceCubeState
    {
        float _xinput = 0;

        //use the base constructor
        public IsAcceleratingState(PlayerInputAction playerInputAction, Rigidbody2D rigidbody2D,
            IceCubeParameters parameters)
            : base(playerInputAction, rigidbody2D, parameters)
        {
        }

        public override IceCubeStatesEnum GetEnumState()
        {
            return IceCubeStatesEnum.IsAccelerating;
        }

        public override void EnterState()
        {
            PlayerInputAction.OnGround.Acceleration.performed += Accelerate;
            PlayerInputAction.OnGround.Acceleration.canceled +=
                ctx => _xinput = 0; // Reset _xinput when the input is released
        }

        public override void PerformPhysicsAction(Vector2 currentDirection)
        {
            Vector2 rigidbody2DVelocity = Rigidbody2D.velocity;
            // speedInput > 0 if user input and current direction are concordant
            float speedInput = _xinput * Mathf.Sign(currentDirection.x);

            // case 1: xInput in the current direction of the cube
            // add force to increase speed to match fast speed
            if (speedInput > 0.0f)
            {
                if (Mathf.Abs(rigidbody2DVelocity.x) < Parameters.fastSpeed)
                    Rigidbody2D.AddForce(Parameters.acceleration * currentDirection, ForceMode2D.Force);
            }
            // case 2: xInput in opposite direction of the cube
            // add force to decrease speed to match slow speed
            else if (speedInput < 0.0f)
            {
                if (Mathf.Abs(rigidbody2DVelocity.x) > Parameters.slowSpeed)
                    Rigidbody2D.AddForce(-Parameters.deceleration * currentDirection, ForceMode2D.Force);
            }
        }

        private void Accelerate(InputAction.CallbackContext ctx)
        {
            _xinput = ctx.ReadValue<float>();
        }

        public override bool ChangeStateOnFinish()
        {
            if (_xinput == 0)
            {
                PlayerInputAction.OnGround.Acceleration.performed -= Accelerate;
                return true;
            }

            return false;
        }

        public override bool ShouldBeInterrupted()
        {
            return true;
        }
    }
}
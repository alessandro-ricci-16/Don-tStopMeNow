using ScriptableObjects;
using UnityEngine;

namespace Ice_Cube.States
{
    public class IsAcceleratingState : IceCubeState
    {
        float _acceleration;

        public override IceCubeStatesEnum GetCurrentState()
        {
            return IceCubeStatesEnum.IsAccelerating;
        }

        public override void EnterState(PlayerInputAction playerInputAction)
        {
            playerInputAction.OnGround.Acceleration.performed += ctx => Accelerate(ctx.ReadValue<float>());
        }

        public override void PerformPhysicsAction(Rigidbody2D rigidbody2D, IceCubeParameters parameters,
            Vector2 currentDirection)
        {
            float epsilon = 0.1f;
            var prevFrameVelocity = rigidbody2D.velocity;
            if (Mathf.Abs(prevFrameVelocity.x) < parameters.defaultSpeed - epsilon)
            {
                rigidbody2D.AddForce(parameters.acceleration * currentDirection, ForceMode2D.Force);
            }
            else if (Mathf.Abs(prevFrameVelocity.x) > parameters.defaultSpeed + epsilon)
            {
                rigidbody2D.AddForce(-parameters.deceleration * currentDirection, ForceMode2D.Force);
            }
        }

        private void Accelerate(float acceleration)
        {
            _acceleration = acceleration;
        }
    }
}
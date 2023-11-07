using ScriptableObjects;
using UnityEngine;

namespace Ice_Cube.States
{
    public class OnGroundState : IceCubeState
    {
        public override void EnterState(PlayerInputAction playerInputAction)
        {
            playerInputAction.OnGround.Enable();
            playerInputAction.OnAir.Disable();
        }

        public override IceCubeStatesEnum GetEnumState()
        {
            return IceCubeStatesEnum.OnGround;
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
    }
}
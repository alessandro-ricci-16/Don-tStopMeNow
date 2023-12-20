using ScriptableObjects;
using UnityEngine;

namespace Ice_Cube.States
{
    public class OnAirState : IceCubeState
    {
        //use the base constructor
        public OnAirState(PlayerInputAction playerInputAction, Rigidbody2D rigidbody2D, IceCubeParameters parameters)
            : base(playerInputAction, rigidbody2D, parameters)
        {
        }

        public override void EnterState()
        {
            PlayerInputAction.OnGround.Disable();
            PlayerInputAction.OnAir.Enable();
        }

        public override IceCubeStatesEnum GetEnumState()
        {
            return IceCubeStatesEnum.OnAir;
        }

        /// <summary>
        /// This function updates the gravity scale according to whether the ice cube is going upwards or downwards.
        /// </summary>
        public override void PerformPhysicsAction(Vector2 currentDirection)
        {
            Vector2 velocity = Rigidbody2D.velocity;
            if (velocity.y >= 0)
                Rigidbody2D.gravityScale = Parameters.upwardGravityScale;
            else if (velocity.y < 0)
            {
                Rigidbody2D.gravityScale = Parameters.downwardGravityScale;
            }

            if (velocity.y < -Parameters.maxFallingSpeed)
            {
                velocity.y = -Parameters.maxFallingSpeed;
                Rigidbody2D.velocity = velocity;
            }
        }
    }
}
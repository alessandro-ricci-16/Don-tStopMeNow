using System.Collections;
using ScriptableObjects;
using UnityEngine;

namespace Ice_Cube.States
{
    public class OnGroundState : IceCubeState
    {
        //use the base constructor
        public OnGroundState(PlayerInputAction playerInputAction, Rigidbody2D rigidbody2D, IceCubeParameters parameters)
            : base(playerInputAction, rigidbody2D, parameters)
        {
        }

        public override void EnterState()
        {
            PlayerInputAction.OnGround.Enable();
            PlayerInputAction.OnAir.Disable();
            Rigidbody2D.gravityScale = Parameters.downwardGravityScale;
        }

        public override IceCubeStatesEnum GetEnumState()
        {
            return IceCubeStatesEnum.OnGround;
        }

        public override void PerformPhysicsAction(Vector2 currentDirection)
        {
            float epsilon = 0.1f;
            var prevFrameVelocity = Rigidbody2D.velocity;
            if (Mathf.Abs(prevFrameVelocity.x) < Parameters.defaultSpeed - epsilon)
            {
                Rigidbody2D.AddForce(Parameters.acceleration * currentDirection, ForceMode2D.Force);
            }
            else if (Mathf.Abs(prevFrameVelocity.x) > Parameters.defaultSpeed + epsilon)
            {
                Rigidbody2D.AddForce(-Parameters.deceleration * currentDirection, ForceMode2D.Force);
            }
        }
    }
}
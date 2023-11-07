using ScriptableObjects;
using UnityEngine;

namespace Ice_Cube.States
{
    public class IsGroundPoundingState : IceCubeState
    {
        private bool _forceAlreadyApplied;

        //use the base constructor
        public IsGroundPoundingState(PlayerInputAction playerInputAction, Rigidbody2D rigidbody2D,
            IceCubeParameters parameters)
            : base(playerInputAction, rigidbody2D, parameters)
        {
        }

        public override void EnterState()
        {
            PlayerInputAction.OnGround.Disable();
            PlayerInputAction.OnAir.Disable();
            _forceAlreadyApplied = false;
        }

        public override void PerformPhysicsAction(Vector2 currentDirection)
        {
            if (!_forceAlreadyApplied)
            {
                // set the gravity scale to zero so only the vertical force affects the rigidbody
                Rigidbody2D.gravityScale = 0.0f;
                // temporarily reset velocity
                Rigidbody2D.velocity = Vector2.zero;
                Rigidbody2D.AddForce(Parameters.groundPoundSpeed * Vector2.down, ForceMode2D.Impulse);
                _forceAlreadyApplied = true;
            }
        }

        public override IceCubeStatesEnum GetEnumState()
        {
            return IceCubeStatesEnum.IsGroundPounding;
        }
    }
}
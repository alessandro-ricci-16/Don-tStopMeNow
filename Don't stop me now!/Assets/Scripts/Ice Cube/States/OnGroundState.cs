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

        public override IceCubeStatesEnum GetCurrentState()
        {
            return IceCubeStatesEnum.OnGround;
        }

        public override void PerformPhysicsAction(Rigidbody2D rigidbody2D, IceCubeParameters parameters,
            Vector2 currentDirection)
        {
            //throw new System.NotImplementedException();
        }
    }
}
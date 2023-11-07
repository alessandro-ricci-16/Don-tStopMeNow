using ScriptableObjects;
using UnityEngine;

namespace Ice_Cube.States
{
    public class OnAirState:IceCubeState
    {
        public override void EnterState(PlayerInputAction playerInputAction)
        {
            playerInputAction.OnGround.Disable();
            playerInputAction.OnAir.Enable();
        }

        public override IceCubeStatesEnum GetEnumState()
        {
            return IceCubeStatesEnum.OnAir;
        }

        public override void PerformPhysicsAction(Rigidbody2D rigidbody2D, IceCubeParameters parameters,
            Vector2 currentDirection)
        {
            //TODO: does this needs to do somethings?
        }
    }
}
using ScriptableObjects;
using UnityEngine;

namespace Ice_Cube.States
{
    public class IsDashingState:IceCubeState
    {
        //use the base constructor
        public IsDashingState(PlayerInputAction playerInputAction, Rigidbody2D rigidbody2D, IceCubeParameters parameters)
            : base(playerInputAction, rigidbody2D, parameters)
        {
        }
        public override void EnterState()
        {
            throw new System.NotImplementedException();
        }

        public override void PerformPhysicsAction(Vector2 currentDirection)
        {
            throw new System.NotImplementedException();
        }

        public override IceCubeStatesEnum GetEnumState()
        {
            return IceCubeStatesEnum.IsDashing;
        }
    }
    
}
using System;
using System.Collections;
using ScriptableObjects;
using UnityEngine;

namespace Ice_Cube.States
{
    public class IsDashingState : IceCubeState
    {
        private float _timeLeft;
        private bool _inExecution;
        private bool _forceApplied;

        //use the base constructor
        public IsDashingState(PlayerInputAction playerInputAction, Rigidbody2D rigidbody2D,
            IceCubeParameters parameters)
            : base(playerInputAction, rigidbody2D, parameters)
        {
        }

        public override void EnterState()
        {
            _inExecution = true;

            _timeLeft = Parameters.dashDuration;
            _forceApplied = false;
            PlayerInputAction.OnGround.Disable();
            PlayerInputAction.OnAir.Disable();

            //We also have to set the gravity scale to 0 to avoid the gravity to affect the player
            Rigidbody2D.gravityScale = 0;
            Rigidbody2D.velocity = Vector2.zero;
        }

        public override void PerformPhysicsAction(Vector2 currentDirection)
        {
            // if the dash time is over, reset normal velocity
            if (_timeLeft - Time.deltaTime <= 0.0f)
            {
                //Restore the gravity scale
                Rigidbody2D.gravityScale = Parameters.downwardGravityScale;
                Rigidbody2D.velocity = Vector2.zero;
                Rigidbody2D.AddForce(Parameters.defaultSpeed * currentDirection, ForceMode2D.Impulse);
                _inExecution = false;
            }
            // if not, apply force for the first time
            else if (!_forceApplied)
            {
                Rigidbody2D.AddForce(Parameters.dashIntensity * currentDirection, ForceMode2D.Impulse);
                _forceApplied = true;
            }

            _timeLeft -= Time.deltaTime;
        }

        public override IceCubeStatesEnum GetEnumState()
        {
            return IceCubeStatesEnum.IsDashing;
        }

        public override bool ShouldBeInterrupted()
        {
            return _forceApplied;
        }

        public override bool ChangeStateOnFinish()
        {
            return !_inExecution;
        }
    }
}
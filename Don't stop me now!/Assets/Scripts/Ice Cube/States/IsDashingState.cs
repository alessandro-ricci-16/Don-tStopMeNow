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
        private float _previousGravityScale;
        private float _previousVelocityX;
        private bool _startedRightDirection;

        //use the base constructor
        public IsDashingState(PlayerInputAction playerInputAction, Rigidbody2D rigidbody2D,
            IceCubeParameters parameters)
            : base(playerInputAction, rigidbody2D, parameters)
        {
        }

        public override void EnterState()
        {
            _inExecution = true;
            //StartCoroutine(DashCoroutine());
            _timeLeft = Parameters.dashDuration;
            _forceApplied = false;
            PlayerInputAction.OnGround.Disable();
            PlayerInputAction.OnAir.Disable();
            //We also have to set the gravity scale to 0 to avoid the gravity to affect the player
            _previousGravityScale = Rigidbody2D.gravityScale;
            Rigidbody2D.gravityScale = 0;

            _previousVelocityX = Rigidbody2D.velocity.x;
            Rigidbody2D.velocity = Vector2.zero;
            //set the direction of the dash
            _startedRightDirection = _previousVelocityX > 0;
        }
        //function of the coroutine

        public override void PerformPhysicsAction(Vector2 currentDirection)
        {
            Vector2 horizontalDirection = new Vector2(currentDirection.x, 0f).normalized;
            if (Mathf.Approximately(_timeLeft - Time.deltaTime, 0f))
            {
                //Restore the gravity scale
                Rigidbody2D.gravityScale = _previousGravityScale;
                //if we switched direction during the dash change the x component of the previous velocity
                float signDirectionX = Mathf.Sign(currentDirection.x);
                float signPreviousVelocityX = Mathf.Sign(_previousVelocityX);

                if (signDirectionX != signPreviousVelocityX)
                {
                    _previousVelocityX *= -1;
                }
                Rigidbody2D.velocity = new Vector2(_previousVelocityX, 0);
                _inExecution = false;
            }
            //Apply the force for the first time
            else if (!_forceApplied)
            {
                //Debug.Log("Force applied in IsDashingState right direction");
                Rigidbody2D.AddForce(Parameters.dashIntensity * horizontalDirection, ForceMode2D.Impulse);
                _forceApplied = true;
            }
            //if we collide with something we have to change direction and therefore apply again the impulse
            else if ((_startedRightDirection && currentDirection.x < 0) ||
                     (!_startedRightDirection && currentDirection.x > 0))
            {
                //Debug.Log($"Force applied in IsDashingState {(currentDirection.x < 0 ? "opposite" : "right")} direction");
                Rigidbody2D.velocity = Vector2.zero;
                Rigidbody2D.AddForce(Parameters.dashIntensity * horizontalDirection, ForceMode2D.Impulse);
            }

            _timeLeft -= Time.deltaTime;
        }

        public override IceCubeStatesEnum GetEnumState()
        {
            return IceCubeStatesEnum.IsDashing;
        }

        public override bool ShouldBeSwitchedOnEnd()
        {
            return !_inExecution;
        }
    }
}
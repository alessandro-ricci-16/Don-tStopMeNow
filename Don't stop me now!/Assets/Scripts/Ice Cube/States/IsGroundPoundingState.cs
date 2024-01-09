using System.Collections;
using ScriptableObjects;
using UnityEngine;

namespace Ice_Cube.States
{
    public class IsGroundPoundingState : IceCubeState
    {
        private bool _forceAlreadyApplied;
        private bool _timeScaleReset;

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
            _timeScaleReset = false;
            Time.timeScale = Parameters.groundPoundTimeScale;
            //start a coroutine that will set the time scale back to 1.0f after a certain amount of time
            GameManager.Instance.StartCoroutine(ResetScale());
        }

        private IEnumerator ResetScale()
        {
            //we need to multiply the time scale by the time slow down factor because the time scale is already slowed down
            yield return new WaitForSeconds(Parameters.groundPoundTimeSlowDown * Parameters.groundPoundTimeScale);
            Time.timeScale = 1.0f;
            Rigidbody2D.AddForce(Parameters.groundPoundSpeed * Vector2.down, ForceMode2D.Impulse);
            _forceAlreadyApplied = true;
        }

        public override void PerformPhysicsAction(Vector2 currentDirection)
        {
            Debug.Log(Rigidbody2D.velocity);
            if (!_forceAlreadyApplied)
            {
                // set the gravity scale to zero so only the vertical force affects the rigidbody
                Rigidbody2D.gravityScale = 0.0f;
                // temporarily reset velocity
                Rigidbody2D.velocity = Vector2.zero;
            }
            else
            {
                if (!_timeScaleReset)
                {
                    Time.timeScale = 1.0f;
                    _timeScaleReset = true;
                }
            }
        }

        public override IceCubeStatesEnum GetEnumState()
        {
            return IceCubeStatesEnum.IsGroundPounding;
        }
    }
}
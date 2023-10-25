using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class IceCubeInput : IceCubePhysics
{
    private float _jumpBufferCounter;
    private float _coyoteTimeCounter;

    protected override void Update()
    {
        base.Update();
        GetPlayerInput();
    }

    /// <summary>
    /// Handles player input. To be called inside Update.
    /// This function also handles coyote time and jump buffer time.
    /// </summary>
    private void GetPlayerInput()
    {
        // jump buffer handling
        if (Input.GetButtonDown("Jump"))
        {
            _jumpBufferCounter = maxJumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }
        // coyote time handling
        if (OnGround)
        {
            _coyoteTimeCounter = maxCoyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }

        JumpCounter -= Time.deltaTime;
        // jump input
        if (_coyoteTimeCounter > 0.0f && _jumpBufferCounter > 0.0f && JumpCounter < 0.0f)
        {
            ShouldJump = true;
            _jumpBufferCounter = 0.0f;
            _coyoteTimeCounter = 0.0f;
        }
        if (Input.GetButtonUp("Jump"))
        {
            InterruptJump();
        }
        
        // speed input (speed update is computed in Move())
        XInput = Input.GetAxisRaw("Horizontal");
    }

}

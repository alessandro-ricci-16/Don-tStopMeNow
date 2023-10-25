using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class IceCubeInput : IceCubePhysics
{
    private float _jumpBufferCounter;
    private float _coyoteTimeCounter;
    private float _wallJumpBufferCounter;
    private float _wallCoyoteTimeCounter;

    protected override void Update()
    {
        base.Update();
        GetPlayerInput();
    }

    /// <summary>
    /// Handles player input. To be called inside Update.
    /// This function also handles coyote time and jump buffer time both for normal jumps and wall jumps.
    /// </summary>
    private void GetPlayerInput()
    {
        // jump buffer 
        if (Input.GetButtonDown("Jump"))
        {
            _jumpBufferCounter = maxJumpBufferTime;
            _wallJumpBufferCounter = maxJumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
            _wallJumpBufferCounter -= Time.deltaTime;
        }
        // normal jump coyote time
        if (OnGround)
        {
            _coyoteTimeCounter = maxCoyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }
        // wall jump coyote time
        if (OnWall)
        {
            _wallCoyoteTimeCounter = maxCoyoteTime;
        }
        else
        {
            _wallCoyoteTimeCounter -= Time.deltaTime;
        }
        
        // jump input
        if (_coyoteTimeCounter > 0.0f && _jumpBufferCounter > 0.0f)
        {
            ShouldJump = true;
            _jumpBufferCounter = 0.0f;
            _coyoteTimeCounter = 0.0f;
        }
        if (_wallJumpBufferCounter > 0.0f && _wallCoyoteTimeCounter > 0.0f)
        {
            ShouldJump = true;
            _wallJumpBufferCounter = 0.0f;
            _wallCoyoteTimeCounter = 0.0f;
        }
        if (Input.GetButtonUp("Jump"))
        {
            InterruptJump();
        }
        
        // speed input (speed update is computed in Move())
        XInput = Input.GetAxisRaw("Horizontal");
    }

}

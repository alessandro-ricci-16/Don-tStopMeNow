using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

/*
 * TODO:
 * - change input system to event map system
 */

public class IceCubeInput : IceCubePhysics
{
    private float _jumpBufferCounter;
    private float _coyoteTimeCounter;
    private float _wallJumpBufferCounter;
    private float _wallCoyoteTimeCounter;
    private int _wallJumpCounter;

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
        HandleGroundPoundInput();
        
        // input is not considered while ground pounding
        if (!IsGroundPounding)
        {
            HandleJumpInput();
            // speed input (speed update is computed in Move())
            XInput = Input.GetAxisRaw("Horizontal");
        }
        
    }
    
    /// <summary>
    /// Handles jump input both for normal jumps and wall jumps; handles coyote time and jump buffer time
    /// for both types of jump.
    /// </summary>
    private void HandleJumpInput()
    {
        // TIMERS AND COUNTERS UPDATE
        
        // jump buffer (input is not considered while ground pounding)
        if (Input.GetButtonDown("Jump") && !IsGroundPounding)
        {
            _jumpBufferCounter = parameters.maxJumpBufferTime;
            _wallJumpBufferCounter = parameters.maxWallJumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
            _wallJumpBufferCounter -= Time.deltaTime;
        }
        if (OnGround)
        {
            // normal jump coyote time
            _coyoteTimeCounter = parameters.maxCoyoteTime;
            // possibility to wall jump resets
            _wallJumpCounter = 0;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }
        // wall jump coyote time
        if (OnWall)
        {
            _wallCoyoteTimeCounter = parameters.maxWallCoyoteTime;
        }
        else
        {
            _wallCoyoteTimeCounter -= Time.deltaTime;
        }
        
        // JUMP INPUT
        
        // normal jump input (cannot jump while ground pounding)
        if (_coyoteTimeCounter > 0.0f && _jumpBufferCounter > 0.0f && !IsGroundPounding)
        {
            ShouldJump = true;
            _jumpBufferCounter = 0.0f;
            _coyoteTimeCounter = 0.0f;
            // to avoid jumping two times with the same input when close to a wall
            _wallJumpBufferCounter = 0.0f;
            _wallCoyoteTimeCounter = 0.0f;
        }
        // wall jump input (cannot jump while ground pounding, cannot wall jump more than the max number of times)
        if (_wallJumpCounter < parameters.maxWallJumpsNumber && _wallCoyoteTimeCounter > 0.0f 
                                                             && _wallJumpBufferCounter > 0.0f && !IsGroundPounding)
        {
            ShouldJump = true;
            _wallJumpCounter += 1;
            _wallJumpBufferCounter = 0.0f;
            _wallCoyoteTimeCounter = 0.0f;
        }
        if (Input.GetButtonUp("Jump"))
        {
            InterruptJump();
        }
    }

    private void HandleGroundPoundInput()
    {
        if (Input.GetButtonDown("GroundPound") && !OnGround)
        {
            ShouldGroundPound = true;
        }
    }
    
}

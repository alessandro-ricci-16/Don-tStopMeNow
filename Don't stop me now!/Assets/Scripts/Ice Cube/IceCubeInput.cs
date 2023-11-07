using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using Ice_Cube.States;
using UnityEngine.InputSystem;

/*
 * TODO:
 * - change input system to event map system
 */
[RequireComponent(typeof(IceCubeStateManager))]
public class IceCubeInput : IceCubePhysics
{
    
    private float _jumpBufferCounter;
    private float _coyoteTimeCounter;
    private float _wallJumpBufferCounter;
    private float _wallCoyoteTimeCounter;
    private int _wallJumpCounter;
    private PlayerInputAction _playerInputAction;
    private IceCubeState _currentState;
    private IceCubeStateManager _stateManager;

    private void OnEnable()
    {
        _playerInputAction = new PlayerInputAction();
        _stateManager = GetComponent<IceCubeStateManager>();
        _stateManager.SetPlayerInputAction(_playerInputAction);
        _playerInputAction.OnGround.Enable();
        _playerInputAction.OnGround.Jump.started += context => JumpStarted();
        // _playerInputAction.OnAir.Jump.started += context => JumpStarted();
        //_playerInputAction.OnAir.Jump.started+= context => JumpStarted();
        _playerInputAction.OnGround.Jump.canceled += context => JumpCanceled();
        _playerInputAction.OnAir.Dash.started += context => DashStarted();
        _playerInputAction.OnGround.Acceleration.started += AccelerationStarted;
        _stateManager.SetNextState(IceCubeStatesEnum.OnAir);
    }

    private void AccelerationStarted(InputAction.CallbackContext value)
    {
        _stateManager.SetNextState(IceCubeStatesEnum.IsAccelerating);
    }
    
    private void DashStarted()
    {
        //Debug.Log("Dash started");
    }

    private void JumpCanceled()
    {
        //TODO This is just here because also before it was here, but I don't know if it's correct to do it not in the fixed update.
        InterruptJump();
    }

    /// <summary>
    /// JumpStarted get called when the jump button is pressed. It doesn't automatically mean that the player will jump.
    /// It will just set the jump buffer counter to the max value.
    /// </summary>
    private void JumpStarted()
    {
        _jumpBufferCounter = parameters.maxJumpBufferTime;
        _wallJumpBufferCounter = parameters.maxWallJumpBufferTime;
    }

    protected void Update()
    {
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

        _jumpBufferCounter -= Time.deltaTime;
        _wallJumpBufferCounter -= Time.deltaTime;

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

        // normal jump input 
        if (_coyoteTimeCounter > 0.0f && _jumpBufferCounter > 0.0f)
        {
             _stateManager.SetNextState(IceCubeStatesEnum.IsJumping);
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
            //TODO CHANGE IN WALL JUMPING
            _stateManager.SetNextState(IceCubeStatesEnum.IsJumping);
            _wallJumpCounter += 1;
            _wallJumpBufferCounter = 0.0f;
            _wallCoyoteTimeCounter = 0.0f;
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
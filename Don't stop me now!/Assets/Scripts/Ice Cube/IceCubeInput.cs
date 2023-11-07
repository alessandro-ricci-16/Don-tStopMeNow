using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using Ice_Cube.States;
using ScriptableObjects;
using UnityEngine.InputSystem;

/*
 * TODO:
 * - change input system to event map system
 */
[RequireComponent(typeof(IceCubeStateManager))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class IceCubeInput : MonoBehaviour
{
    [SerializeField] protected IceCubeParameters parameters;
    
    // maximum tolerance for normals in collision handling
    private const float Epsilon = 0.1f;
    
    private bool _onGround;
    private bool _onWall;
    private float _jumpBufferCounter;
    private float _coyoteTimeCounter;
    private float _wallJumpBufferCounter;
    private float _wallCoyoteTimeCounter;
    private int _wallJumpCounter;
    
    
    // should be Vector2.left or Vector2.right;
    // does not take into account vertical movement by design
    private Vector2 _currentDirection;
    // velocity at the previous frame
    private Vector2 _prevFrameVelocity;
    
    private Rigidbody2D _rigidbody2D;
    
    // TODO delete this variable (only here for debugging)
    private SpriteRenderer _spriteRenderer;
    private PlayerInputAction _playerInputAction;
    private IceCubeState _currentState;
    private IceCubeStateManager _stateManager;

    private void Start()
    {
        //callback initialization
        _playerInputAction = new PlayerInputAction();
        _stateManager = GetComponent<IceCubeStateManager>();
        _stateManager.SetPlayerInputAction(_playerInputAction);
        _playerInputAction.OnGround.Enable();
        _playerInputAction.OnGround.Jump.started += NormalJumpStarted;
        _playerInputAction.OnGround.Acceleration.started += AccelerationStarted;

        _playerInputAction.OnAir.GroundPound.started += GroundPoundStarted;
        _playerInputAction.OnAir.Dash.started += DashStarted;
        _playerInputAction.OnAir.WallJump.started += JumpWallStarted;
        
        //class initialization
        _onGround = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _stateManager = GetComponent<IceCubeStateManager>();
        _rigidbody2D.gravityScale = parameters.downwardGravityScale;
        _rigidbody2D.freezeRotation = true;
        _currentDirection = Vector2.right;
    }


    private void Update()
    {
        HandleJumpInput();
        _spriteRenderer.flipX = _currentDirection == Vector2.left;
    }
    private void FixedUpdate()
    {
        _prevFrameVelocity = _rigidbody2D.velocity;
        _stateManager.GetCurrentState().PerformPhysicsAction(_currentDirection);
    }
    
    #region Collisions
    
    /// <summary>
    /// Function to be called inside OnCollisionEnter2D and OnCollisionStay2D.
    /// Takes as input the Collision2D and checks all normals of collision points.
    /// If the normal is Vector2.left or Vector2.right, it adjust the variable
    /// _currentDirection accordingly and applies an impulse to the body in order
    /// to change direction.
    /// If the normal is Vector2.up, it sets onGround to true.
    /// </summary>
    /// <param name="other"></param> parameter from OnCollisionEnter2D or
    /// OnCollisionStay2D
    private void HandleCollisions(Collision2D other)
    {
        // get the contacts from the collision
        int contactsNumber = other.contactCount;
        ContactPoint2D[] contacts = new ContactPoint2D[contactsNumber];
        other.GetContacts(contacts);
        
        // assume I am not on the ground and not on a wall
        bool isPlayerOnGround = false;
        bool isPlayerOnWall = false;
        
        // iterate and check normals
        foreach (ContactPoint2D c in contacts)
        {
            Vector2 normal = c.normal;
            if ((normal - Vector2.left).magnitude < Epsilon)
            {
                isPlayerOnWall = true;
                // direction check: avoid applying force multiple times for different contact points
                if (_prevFrameVelocity.x >= -Mathf.Epsilon && _currentDirection != Vector2.left)
                {
                    _currentDirection = Vector2.left;
                    _rigidbody2D.AddForce(parameters.defaultSpeed*Vector2.left, ForceMode2D.Impulse);
                }
            }
            else if ((normal - Vector2.right).magnitude < Epsilon)
            {
                isPlayerOnWall = true;
                if (_prevFrameVelocity.x <= Mathf.Epsilon && _currentDirection != Vector2.right)
                {
                    _currentDirection = Vector2.right;
                    _rigidbody2D.AddForce(parameters.defaultSpeed*Vector2.right, ForceMode2D.Impulse);
                }
            }
            else if ((normal - Vector2.up).magnitude < Epsilon)
            {
                // if velocity.y > 0, then I'm jumping and I'm leaving the platform
                // (problem with spamming jump button during coyoteTime)
                // TODO: check if this actually changes anything
                if (_prevFrameVelocity.y < Epsilon)
                    isPlayerOnGround = true;
            }
        }
        SetGrounded(isPlayerOnGround);
        _onWall = isPlayerOnWall;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        HandleCollisions(other);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        HandleCollisions(other);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        SetGrounded(false);
        _onWall = false;
    }
    
    #endregion
    
    
    /// <summary>
    /// Handle jump input with jump buffer and coyone time. When it can jump it invokes jump or wallJump state
    /// </summary>
    private void HandleJumpInput()
    {
        // TIMERS AND COUNTERS UPDATE

        // jump buffer (input is not considered while ground pounding)

        _jumpBufferCounter -= Time.deltaTime;
        _wallJumpBufferCounter -= Time.deltaTime;

        if (_onGround)
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
        if (_onWall)
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

        // wall jump input (Cannot wall jump more than the max number of times)
        if (_wallJumpCounter < parameters.maxWallJumpsNumber && _wallCoyoteTimeCounter > 0.0f &&
            _wallJumpBufferCounter > 0.0f)
        {
            _stateManager.SetNextState(IceCubeStatesEnum.IsWallJumping);
            _wallJumpCounter += 1;
            _wallJumpBufferCounter = 0.0f;
            _wallCoyoteTimeCounter = 0.0f;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="grounded"></param>
    private void SetGrounded(bool grounded)
    {
        if (_onGround != grounded)
        {
            _onGround = grounded;
            EventManager.TriggerEvent(EventNames.OnGround, grounded);
        }
    }
    #region CallBacks

    private void GroundPoundStarted(InputAction.CallbackContext obj)
    {
        _stateManager.SetNextState(IceCubeStatesEnum.IsGroundPounding);
    }

    private void AccelerationStarted(InputAction.CallbackContext value)
    {
        _stateManager.SetNextState(IceCubeStatesEnum.IsAccelerating);
    }

    private void DashStarted(InputAction.CallbackContext value)
    {
        //Debug.Log("Dash started");
    }

    /// <summary>
    /// JumpStarted get called when the jump button is pressed. It doesn't automatically mean that the player will jump.
    /// It will just set the jump buffer counter to the max value.
    /// </summary>
    private void NormalJumpStarted(InputAction.CallbackContext value)
    {
        _jumpBufferCounter = parameters.maxJumpBufferTime;
    }

    /// <summary>
    /// JumpWallStarted get called when the jump button is pressed. It doesn't automatically mean that the player will jump.
    /// It will just set the wall jump buffer counter to the max value.
    /// </summary>
    private void JumpWallStarted(InputAction.CallbackContext value)
    {
        _wallJumpBufferCounter = parameters.maxWallJumpBufferTime;
    }

    #endregion
}
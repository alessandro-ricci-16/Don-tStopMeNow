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

public enum Direction { Right, Left }


[RequireComponent(typeof(IceCubeStateManager))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(TrailRenderer))]
public class IceCubeInput : MonoBehaviour
{
    [Header("Movement and input parameters")]
    public Direction initialDirection;
    [SerializeField] protected IceCubeParameters parameters;
    [SerializeField] private bool canWallJump = true;
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool canGroundPound = true;

    // maximum tolerance for normals in collision handling
    private const float Epsilon = 0.1f;

    private bool _onGround;
    private bool _onWall;
    private float _jumpBufferCounter;
    private float _coyoteTimeCounter;
    private float _wallJumpBufferCounter;
    private float _wallCoyoteTimeCounter;
    private int _wallJumpCounter;
    private int _dashCounter;
    
    // minimum time between two collisions to send the event
    private float _collisionCoolDown = 0.1f;
    private float _collisionCoolDownTimer = 0.0f;

    // should be Vector2.left or Vector2.right;
    // does not take into account vertical movement by design
    private Vector2 _currentDirection;

    // velocity at the previous frame
    private Vector2 _prevFrameVelocity;

    private Rigidbody2D _rigidbody2D;
    private PlayerInputAction _playerInputAction;
    private IceCubeState _currentState;
    private IceCubeStateManager _stateManager;

    #region Setup Methods
    
    private void Start()
    {
        //class initialization
        _onGround = false;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _stateManager = GetComponent<IceCubeStateManager>();
        TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.enabled = false;
        _rigidbody2D.gravityScale = parameters.downwardGravityScale;
        _rigidbody2D.freezeRotation = true;
        
        SetPositionAndDirection();
        
        if (initialDirection == Direction.Left)
            SetCurrentDirection(Vector2.left);
        else
            SetCurrentDirection(Vector2.right);

        _rigidbody2D.velocity = parameters.defaultSpeed * _currentDirection;
        
        trailRenderer.enabled = true;
        
        InitializeCallbacks();
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from events and clean up resources
        if (_playerInputAction != null)
        {
            _playerInputAction.Jump.Jump.started -= JumpStarted;
            _playerInputAction.OnGround.Acceleration.started -= AccelerationStarted;
            _playerInputAction.OnAir.GroundPound.started -= GroundPoundStarted;
            _playerInputAction.OnAir.Dash.started -= DashStarted;
            _playerInputAction.Jump.Jump.canceled -= InterruptJump;

            // Clean up the _playerInputAction object
            _playerInputAction.Dispose();
        }
        
        EventManager.StopListening(EventNames.GamePause, DisableInput);
        EventManager.StopListening(EventNames.GameResume, EnableInput);
    }
    
    private void Update()
    {
        HandleJumpInput();
        HandleCollisionTimer();
    }

    private void FixedUpdate()
    {
        _stateManager.GetCurrentState().PerformPhysicsAction(_currentDirection);
        _prevFrameVelocity = _rigidbody2D.velocity;
    }
    
    #endregion

    #region Initialization and Settings

    /// <summary>
    /// If the level starts at a checkpoint, set the position and the direction of the player accordingly.
    /// </summary>
    private void SetPositionAndDirection()
    {
        if (GameManager.Instance.StartAtCheckPoint)
        {
            this.transform.position = GameManager.Instance.LastCheckpoint;
            this.initialDirection = GameManager.Instance.CheckpointStartDirection;
        }
    }
    private void InitializeCallbacks()
    {
        //callback initialization
        _playerInputAction = new PlayerInputAction();
        _stateManager = GetComponent<IceCubeStateManager>();
        _stateManager.InitStateManager(_playerInputAction, parameters);
        
        _playerInputAction.Jump.Enable();
        _playerInputAction.Jump.Jump.started += JumpStarted;
        _playerInputAction.OnGround.Acceleration.started += AccelerationStarted;
        
        if (canGroundPound)
            _playerInputAction.OnAir.GroundPound.started += GroundPoundStarted;
        if (canDash)
            _playerInputAction.OnAir.Dash.started += DashStarted;
        
        _playerInputAction.Jump.Jump.canceled += InterruptJump;
        
        EventManager.StartListening(EventNames.GamePause, DisableInput);
        EventManager.StartListening(EventNames.GameResume, EnableInput);
    }
    
    /// <summary>
    /// Set the current Direction and trigger the event associated
    /// </summary>
    /// <param name="newD"></param>
    private void SetCurrentDirection(Vector2 newD)
    {
        if (_currentDirection == newD) return;
        _currentDirection = newD;
        Vector3 newScale = transform.localScale;
        newScale.x = newD.x * MathF.Abs(newScale.x);
        transform.localScale = newScale;
        EventManager.TriggerEvent(EventNames.ChangedDirection, newD);
    }
    

    #endregion

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
         // Ignore if collided with a breakable while dashing
         if (other.gameObject.CompareTag("Breakable")&& _stateManager.GetCurrentState().GetEnumState() == IceCubeStatesEnum.IsDashing)
            return;
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
                if (_currentDirection != Vector2.left)
                {
                    _rigidbody2D.velocity = Vector2.zero;
                    SetCurrentDirection(Vector2.left);
                    _rigidbody2D.AddForce(Mathf.Abs(_prevFrameVelocity.x) * Vector2.left, ForceMode2D.Impulse);

                    // preserves the vertical speed
                    _rigidbody2D.AddForce(_prevFrameVelocity.y * Vector2.up, ForceMode2D.Impulse);
                }
            }
            else if ((normal - Vector2.right).magnitude < Epsilon)
            {
                isPlayerOnWall = true;
                if (_currentDirection != Vector2.right)
                {
                    _rigidbody2D.velocity = Vector2.zero;
                    SetCurrentDirection(Vector2.right);
                    _rigidbody2D.AddForce(Mathf.Abs(_prevFrameVelocity.x) * Vector2.right, ForceMode2D.Impulse);

                    // preserves the vertical speed
                    _rigidbody2D.AddForce(_prevFrameVelocity.y * Vector2.up, ForceMode2D.Impulse);
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
    
    private void SendCollisionEvent()
    {
        if (_collisionCoolDownTimer <= 0.0f)
        {
            EventManager.TriggerEvent(EventNames.CollisionWithGround);
            _collisionCoolDownTimer = _collisionCoolDown;
        }
    }

    private void HandleCollisionTimer()
    {
        _collisionCoolDownTimer -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        HandleCollisions(other);
        SendCollisionEvent();
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

    #region Input Handling

    private void DisableInput()
    {
        _playerInputAction.Disable();
    }
    
    private void EnableInput()
    {
        _playerInputAction.Enable();
    }
    
    
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
            // possibility to dash resets
            _dashCounter = 0;
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
        if (canWallJump && _wallJumpCounter < parameters.maxWallJumpsNumber && _wallCoyoteTimeCounter > 0.0f &&
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
    
    #endregion
    
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
        if (_dashCounter < parameters.maxDashesNumber)
        {
            _stateManager.SetNextState(IceCubeStatesEnum.IsDashing);
            _dashCounter += 1;
        }
    }

    /// <summary>
    /// JumpStarted get called when the jump button is pressed. It doesn't automatically mean that the player will jump.
    /// It will just set the jump buffer counter to the max value.
    /// </summary>
    /// <param name="value"></param>
    private void JumpStarted(InputAction.CallbackContext value)
    {
        _wallJumpBufferCounter = parameters.maxWallJumpBufferTime;
        _jumpBufferCounter = parameters.maxJumpBufferTime;
    }

    private void InterruptJump(InputAction.CallbackContext value)
    {
        // only interrupt jump if the jump is not a wall jump
        if (gameObject.activeSelf && _wallJumpCounter == 0)
            StartCoroutine(InterruptJumpCoroutine());    
    }
    
    private IEnumerator InterruptJumpCoroutine()
    {
        float jumpReleaseTimer = parameters.jumpReleaseTime;

        while (true)
        {
            Vector2 velocity = _rigidbody2D.velocity;
            if (jumpReleaseTimer <= 0 && velocity.y > 0)
            {
                _rigidbody2D.velocity = new Vector2(velocity.x, velocity.y/5);
                break;
            }
            // if velocity.y <= this jump has ended, stop updating timer
            if (velocity.y <= 0)
            {
                break;
            }
            
            yield return new WaitForEndOfFrame();
            jumpReleaseTimer -= Time.deltaTime;
        }
    }
    #endregion
    
}
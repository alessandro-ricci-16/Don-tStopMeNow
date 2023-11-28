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


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(TrailRenderer))]
public class MovingObstaclesInput : MonoBehaviour
{
    public Direction initialDirection;
    [SerializeField] protected MovingObstaclesParameters parameters;

    // maximum tolerance for normals in collision handling
    private const float Epsilon = 0.1f;

    private bool _onGround;
    private bool _onWall;
    private float _jumpBufferCounter;
    private float _coyoteTimeCounter;
    private float _wallJumpBufferCounter;
    private float _wallCoyoteTimeCounter;


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

    #region Setup and Initialization

    private void Start()
    {
        //class initialization
        _onGround = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _stateManager = GetComponent<IceCubeStateManager>();
        TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.enabled = false;
        _rigidbody2D.gravityScale = 9.81f;
        _rigidbody2D.freezeRotation = false;

        if (initialDirection == Direction.Left)
            _currentDirection = Vector2.left;
        else
            _currentDirection = Vector2.right;

        trailRenderer.enabled = true;

        InitializeCallbacks();
    }

    private void InitializeCallbacks()
    {
        //callback initialization
        _playerInputAction = new PlayerInputAction();
        _stateManager = GetComponent<IceCubeStateManager>();
        _playerInputAction.OnGround.Acceleration.started += AccelerationStarted;
    }

    private void OnDestroy()
    {
        // Unsubscribe from events and clean up resources
        if (_playerInputAction != null)
        {
            // Clean up the _playerInputAction object
            _playerInputAction.Dispose();
        }
    }

    #endregion

    private void Update()
    {
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
                if (_currentDirection != Vector2.left)
                {
                    _rigidbody2D.velocity = Vector2.zero;
                    _currentDirection = Vector2.left;
                    _rigidbody2D.AddForce(Mathf.Abs(_prevFrameVelocity.x) * Vector2.left, ForceMode2D.Impulse);
                }
            }
            else if ((normal - Vector2.right).magnitude < Epsilon)
            {
                isPlayerOnWall = true;
                if (_currentDirection != Vector2.right)
                {
                    _rigidbody2D.velocity = Vector2.zero;
                    _currentDirection = Vector2.right;
                    _rigidbody2D.AddForce(Mathf.Abs(_prevFrameVelocity.x) * Vector2.right, ForceMode2D.Impulse);
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

    private void AccelerationStarted(InputAction.CallbackContext value)
    {
        _stateManager.SetNextState(IceCubeStatesEnum.IsAccelerating);
    }

    #endregion
}
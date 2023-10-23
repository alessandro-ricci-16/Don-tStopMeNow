using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class IceCubePhysics : MonoBehaviour
{
    [SerializeField] private bool debug = true;
    
    [Header("Movement")]
    [SerializeField] private float horizontalSpeed = 5.0f;
    [Tooltip("Set max vertical speed to avoid breaking colliders")]
    [SerializeField] private float maxVerticalSpeed = 20.0f;
    
    [Header("Jump")]
    [Tooltip("Max height reached by jump")]
    [SerializeField] private float jumpHeight = 2.5f;
    [SerializeField] private float upwardGravityMultiplier = 3.0f;
    [SerializeField] private float downwardGravityMultiplier = 6.0f;
    [SerializeField] private float defaultGravityMultiplier = 1.0f;
    [SerializeField] private float maxJumpBufferTime = 0.5f;
    [SerializeField] private float maxCoyoteTime = 0.5f;

    private const float Epsilon = 0.1f;
    
    private bool _onGround;
    private float _jumpBufferCounter;
    private float _coyoteTimeCounter;
    private float _jumpCounter;
    private Vector2 _currentDirection;
    
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    
    void Start()
    {
        _onGround = false;
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _rigidbody2D = this.GetComponent<Rigidbody2D>();
        _rigidbody2D.gravityScale = defaultGravityMultiplier;
        // _rigidbody2D.freezeRotation = true;
        _currentDirection = Vector2.right;
        // TODO set angular drag, mass,...
    }
    
    void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        Move();
        if (debug)
            _spriteRenderer.color = _onGround ? Color.green : Color.red;
    }

    #region Input
    
    /// <summary>
    /// Handles jump input. To be called inside Update.
    /// This function also handles coyote time and jump buffer time.
    /// </summary>
    private void HandleInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _jumpBufferCounter = maxJumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }

        if (_onGround)
        {
            _coyoteTimeCounter = maxCoyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }

        _jumpCounter -= Time.deltaTime;

        if (_coyoteTimeCounter > 0.0f && _jumpBufferCounter > 0.0f && _jumpCounter < 0.0f)
        {
            Jump();
            _jumpBufferCounter = 0.0f;
            _coyoteTimeCounter = 0.0f;
        }
    }

    #endregion

    #region Movement
    
    /// <summary>
    /// Updates the rigidbody velocity and the gravity scale.
    /// x axis -> the direction is kept the same but the norm is set
    /// equal to the speed variable.
    /// y axis -> the value is kept the same.
    /// The value on the y axis is clamped to avoid excessive speeds which
    /// might break the colliders.
    /// </summary>
    private void Move()
    {
        Vector2 velocity = new Vector2();
        
        // set horizontal speed
        velocity.x = horizontalSpeed * Mathf.Sign(_currentDirection.x);
        
        // if cube is not on the ground, adjust gravity scale
        if (!_onGround)
        {
            float velocityY = _rigidbody2D.velocity.y;
            if (velocityY > 0)
                _rigidbody2D.gravityScale = upwardGravityMultiplier;
            else if (velocityY < 0)
                _rigidbody2D.gravityScale = downwardGravityMultiplier;
            else if (velocityY == 0)
                _rigidbody2D.gravityScale = defaultGravityMultiplier;
        }
   
        velocity = new Vector2(velocity.x, _rigidbody2D.velocity.y);
        velocity.y = Mathf.Clamp(velocity.y, -maxVerticalSpeed, maxVerticalSpeed);
        
        _rigidbody2D.velocity = velocity;
    }
    
    /// <summary>
    /// The function computes the jump speed necessary to reach the 
    /// standard jumpHeight and sets the rigidbody y velocity to that value.
    /// IMPORTANT: the function does NOT check if the player is on the ground.
    /// </summary>
    private void Jump()
    {
        // compute jump speed to reach specified jump height
        float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * 
                                     upwardGravityMultiplier * jumpHeight);
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpSpeed);
        _onGround = false;
        _jumpCounter = maxCoyoteTime + Mathf.Epsilon;
    }
    
    #endregion
    
    #region Collisions
    
    /// <summary>
    /// Function to be called inside OnCollisionEnter2D and OnCollisionStay2D.
    /// Takes as input the Collision2D and checks all normals of collision points.
    /// If the normal is Vector2.left or Vector2.right, it adjust the variable
    /// _currentDirection accordingly.
    /// If the normal is Vector2.up, it sets _onGround to true.
    /// </summary>
    /// <param name="other"></param> parameter from OnCollisionEnter2D or
    /// OnCollisionStay2D
    private void HandleCollisions(Collision2D other)
    {
        // get the contacts from the collision
        int contactsNumber = other.contactCount;
        ContactPoint2D[] contacts = new ContactPoint2D[contactsNumber];
        other.GetContacts(contacts);
        
        // assume I am not on the ground
        bool stillGrounded = false;
        
        // iterate and check normals
        foreach (ContactPoint2D c in contacts)
        {
            Vector2 normal = c.normal;
            if ((normal - Vector2.left).magnitude < Epsilon)
            {
                _currentDirection = Vector2.left;
            }
            else if ((normal - Vector2.right).magnitude < Epsilon)
            {
                _currentDirection = Vector2.right;
            }
            else if ((normal - Vector2.up).magnitude < Epsilon)
            {
                if (_rigidbody2D.velocity.y < Epsilon)
                    stillGrounded = true;
            }
        }
        _onGround = stillGrounded;
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
        _onGround = false;
    }
    
    #endregion
}

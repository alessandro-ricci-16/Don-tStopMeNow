using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/*
 * TODO:
 * - handle jumpCounter better (currently there to avoid double jumping in
 *   the coyote time timeframe) -> integer jump counter? -> also for cancel jump
 * - make sure the player actually jumped before calling cancel jump?
 */

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class IceCubePhysics : MonoBehaviour
{
    [SerializeField] private bool debug = true;
    public Vector2 prevVelocity;
    
    [Header("Movement")]
    [SerializeField] private float defaultSpeed = 5.0f;
    [SerializeField] private float slowSpeed = 3.5f;
    [SerializeField] private float fastSpeed = 7.0f;
    [SerializeField] private float acceleration = 20.0f;
    [SerializeField] private float deceleration = 20.0f;
    
    [Header("Jump")]
    [Tooltip("Max height reached by jump")]
    [SerializeField] private float maxJumpHeight = 4.0f;
    [SerializeField] private float upwardGravityMultiplier = 6.0f;
    [SerializeField] private float downwardGravityMultiplier = 8.0f;
    [SerializeField] private float defaultGravityMultiplier = 1.0f;
    [SerializeField] protected float maxJumpBufferTime = 0.1f;
    [SerializeField] protected float maxCoyoteTime = 0.1f;
    
    // maximum tolerance for normals in collision handling
    private const float Epsilon = 0.1f;
    
    protected bool OnGround;
    protected bool ShouldJump;
    protected float JumpCounter;
    
    // should be Vector2.left or Vector2.right;
    // does not take into account vertical movement
    private Vector2 _currentDirection;
    // updated using the horizontal axis
    protected float XInput;
    
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    
    void Start()
    {
        OnGround = false;
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _rigidbody2D = this.GetComponent<Rigidbody2D>();
        _rigidbody2D.gravityScale = defaultGravityMultiplier;
        // _rigidbody2D.freezeRotation = true;
        _currentDirection = Vector2.right;
        XInput = 0.0f;
    }
    
    protected virtual void Update()
    {
        // if (debug)
        //     _spriteRenderer.color = OnGround ? Color.green : Color.red;
    }

    private void FixedUpdate()
    {
        Move();
        if (ShouldJump)
        {
            Jump();
            ShouldJump = false;
        }
    }

    #region Movement
    
    /// <summary>
    /// Updates the rigidbody velocity and the gravity scale.
    /// x axis -> the speed is updated according to _xInput
    /// y axis -> the value is clamped to avoid excessive speeds which
    /// might break the colliders.
    /// The gravity scale is updated to reflect whether the character is going
    /// upwards or downwards.
    /// </summary>
    private void Move()
    {
        prevVelocity = _rigidbody2D.velocity;
        
        // update horizontal speed
        //speedInput is >0 if the input and the velocity are coherent
        float speedInput = XInput * Mathf.Sign(_currentDirection.x);
        // case 1: xInput in the current direction of the cube
        // increase speed to match fast speed
        if (speedInput > 0.0f)
        {
            if (Mathf.Abs(prevVelocity.x) < fastSpeed)
                _rigidbody2D.AddForce(acceleration * _currentDirection, ForceMode2D.Force);
        }
        // case 2: xInput in opposite direction of the cube
        // decrease speed to match slow speed
        else if (speedInput < 0.0f)
        {
            if (Mathf.Abs(prevVelocity.x) > slowSpeed)
                _rigidbody2D.AddForce(- deceleration * _currentDirection, ForceMode2D.Force);
        }
        // case 3: no input
        // modify speed to match normal speed
        else
        {
            if (Mathf.Abs(prevVelocity.x) < defaultSpeed - Epsilon)
            {
                // Debug.Log("Accelerating");
                _spriteRenderer.color = Color.magenta;
                _rigidbody2D.AddForce(acceleration * _currentDirection, ForceMode2D.Force);
            }
            else if (Mathf.Abs(prevVelocity.x) > defaultSpeed + Epsilon)
            {
                // Debug.Log("Decelerating");
                _spriteRenderer.color = Color.yellow;
                _rigidbody2D.AddForce(- deceleration * _currentDirection, ForceMode2D.Force);
            }
            else
            {
                _spriteRenderer.color = Color.white;
            }
        }
        
        // if cube is not on the ground, adjust gravity scale
        if (!OnGround)
        {
            if (prevVelocity.y > 0)
                _rigidbody2D.gravityScale = upwardGravityMultiplier;
            else if (prevVelocity.y < 0)
                _rigidbody2D.gravityScale = downwardGravityMultiplier;
            else if (prevVelocity.y == 0)
                _rigidbody2D.gravityScale = defaultGravityMultiplier;
        }
    }
    
    /// <summary>
    /// The function computes the jump speed necessary to reach the 
    /// standard maxJumpHeight and sets the rigidbody y velocity to that value.
    /// The jump can be canceled with CancelJump().
    /// IMPORTANT: the function does NOT check if the player is on the ground.
    /// </summary>
    private void Jump()
    {
        // compute jump speed to reach maxJumpHeight
        float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * 
                                     upwardGravityMultiplier * maxJumpHeight);
        // _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpSpeed);
        _rigidbody2D.AddForce(jumpSpeed*Vector2.up, ForceMode2D.Impulse);
        OnGround = false;
        JumpCounter = maxCoyoteTime + Mathf.Epsilon;
    }
    
    /// <summary>
    /// Cancels a jump. This function is called when the player releases the
    /// "jump" button.
    /// If the vertical velocity is > 0, it divides it by 2.
    /// </summary>
    protected void CancelJump()
    {
        Vector2 prevVelocity = _rigidbody2D.velocity;
        if (prevVelocity.y > 0)
        {
            _rigidbody2D.velocity = new Vector2(prevVelocity.x, prevVelocity.y / 2);
        }
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
        Vector2 prevVelocity = _rigidbody2D.velocity;
        
        // assume I am not on the ground
        bool stillGrounded = false;
        
        // iterate and check normals
        foreach (ContactPoint2D c in contacts)
        {
            Vector2 normal = c.normal;
            if ((normal - Vector2.left).magnitude < Epsilon)
            {
                // direction check: avoid applying force multiple times for diff contact points
                if (prevVelocity.x >= -Mathf.Epsilon && _currentDirection != Vector2.left)
                {
                    _currentDirection = Vector2.left;
                    _rigidbody2D.AddForce(defaultSpeed*Vector2.left, ForceMode2D.Impulse);
                }
            }
            else if ((normal - Vector2.right).magnitude < Epsilon)
            {
                if (prevVelocity.x <= Mathf.Epsilon && _currentDirection != Vector2.right)
                {
                    _currentDirection = Vector2.right;
                    _rigidbody2D.AddForce(defaultSpeed*Vector2.right, ForceMode2D.Impulse);
                }
            }
            else if ((normal - Vector2.up).magnitude < Epsilon)
            {
                // if velocity.y > 0, then I'm jumping and I'm leaving the platform
                // (problem with spamming jump button during coyoteTime)
                // TODO: check if this actually changes anything
                if (_rigidbody2D.velocity.y < Epsilon)
                    stillGrounded = true;
            }
        }
        OnGround = stillGrounded;
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
        OnGround = false;
    }
    
    #endregion
}

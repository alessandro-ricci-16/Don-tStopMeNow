using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class IceCubePhysics : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float horizontalSpeed = 5.0f;
    
    [Header("Jump")]
    [SerializeField] private float jumpHeight = 2.0f;
    [SerializeField] private float upwardGravityMultiplier = 1.7f;
    [SerializeField] private float downwardGravityMultiplier = 3.0f;
    [SerializeField] private float defaultGravityMultiplier = 1.0f;

    private const float Epsilon = 0.1f;
    
    private Vector2 _velocity;
    private bool _grounded;
    
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    
    void Start()
    {
        _grounded = false;
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _rigidbody2D = this.GetComponent<Rigidbody2D>();
        _rigidbody2D.gravityScale = 0.0f;
    }
    
    void Update()
    {
        if (Input.GetButton("Jump"))
            Jump();
    }

    private void FixedUpdate()
    {
        Move();
        _spriteRenderer.color = _grounded ? Color.green : Color.red;
    }

    #region Movement
    
    private void Move()
    {
        _velocity = ComputeVelocity();
        _rigidbody2D.velocity = _velocity;
    }

    private void Jump()
    {
        if (_grounded)
        {
            float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * upwardGravityMultiplier * jumpHeight);
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpSpeed);
            _grounded = false;
        }
    }

    private Vector2 ComputeVelocity()
    {
        Vector2 velocity = new Vector2();
        Vector2 prevVelocity = _rigidbody2D.velocity;
        
        // set horizontal speed
        velocity.x = horizontalSpeed * Mathf.Sign(prevVelocity.x);
        
        // if cube is not on the ground, adjust gravity scale
        // TODO clamp the downward velocity to a max value
        if (!_grounded)
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
        return velocity;
    }
    
    #endregion
    
    #region Collisions
    
    private void HandleCollisions(Collision2D other)
    {
        int contactsNumber = other.contactCount;
        ContactPoint2D[] contacts = new ContactPoint2D[contactsNumber];
        other.GetContacts(contacts);
        bool stillGrounded = false;
        
        foreach (ContactPoint2D c in contacts)
        {
            Vector2 normal = c.normal;
            Vector2 prevVelocity = _rigidbody2D.velocity;
            
            if ((normal - Vector2.left).magnitude < Epsilon)
            {
                if (prevVelocity.x > 0)
                    _rigidbody2D.velocity = new Vector2(-prevVelocity.x, prevVelocity.y);
            }
            else if ((normal - Vector2.right).magnitude < Epsilon)
            {
                if (prevVelocity.x < 0)
                    _rigidbody2D.velocity = new Vector2(-prevVelocity.x, prevVelocity.y);
            }
            else if ((normal - Vector2.up).magnitude < Epsilon)
            {
                stillGrounded = true;
            }
        }
        _grounded = stillGrounded;
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
        _grounded = false;
    }
    
    #endregion
}

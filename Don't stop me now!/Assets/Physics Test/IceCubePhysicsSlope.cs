using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class IceCubePhysicsSlope : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float horizontalSpeed = 5.0f;
    [Tooltip("min y value of a surface normal for it to be considered ground")]
    [SerializeField] private float minSlopeY = 0.9f;
    [SerializeField] private float rotationSpeed = 2.0f;
    
    [Header("Jump")]
    [SerializeField] private float jumpHeight = 2.0f;
    [SerializeField] private float upwardGravityMultiplier = 1.7f;
    [SerializeField] private float downwardGravityMultiplier = 3.0f;
    [SerializeField] private float defaultGravityMultiplier = 1.0f;

    private const float Epsilon = 0.1f;

    private Vector2 _currentDirection;
    private Vector2 _groundNormal;
    private Vector2 _velocity;
    private bool _grounded;
    
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    
    void Start()
    {
        _currentDirection = Vector2.right;
        _groundNormal = Vector2.up;
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
    }

    #region Movement
    
    private void Move()
    {
        _velocity = ComputeVelocity();
        // _rigidbody2D.MovePosition(_rigidbody2D.position + velocity * Time.deltaTime);
        _rigidbody2D.velocity = _velocity;
    }

    private void Jump()
    {
        if (_grounded)
        {
            float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
            // TODO compute this velocity in ComputeVelocity
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpSpeed);
            _grounded = false;
        }
    }

    private Vector2 ComputeVelocity()
    {
        Vector2 velocity = new Vector2();
        
        // if cube is not on the ground, set horizontal velocity
        // and decrease y velocity by gravity
        // TODO clamp the downward velocity to a max value
        if (!_grounded)
        {
            velocity.x = horizontalSpeed * _currentDirection.x;
            float velocityY = _rigidbody2D.velocity.y;
            _spriteRenderer.color = Color.red;
            float deltaVelocity = Time.deltaTime * Physics2D.gravity.y;
            if (velocityY > 0)
                deltaVelocity *= upwardGravityMultiplier;
            else if (velocityY < 0)
                deltaVelocity *= downwardGravityMultiplier;
            else if (velocityY == 0)
                deltaVelocity *= defaultGravityMultiplier;
            velocity.y = velocityY + deltaVelocity;
        }
        // if cube is grounded, set velocity parallel to the ground
        else if (_grounded)
        {
            Vector2 velocityDirection = new Vector2();
            if ((_groundNormal - Vector2.up).magnitude < Epsilon)
                velocityDirection = _currentDirection;
            else
            {
                if (_currentDirection == Vector2.right)
                {
                    velocityDirection = new Vector2(_groundNormal.y, -_groundNormal.x);
                }
                else if (_currentDirection == Vector2.left)
                {
                    velocityDirection = new Vector2(-_groundNormal.y, _groundNormal.x);
                }
            }
            velocity = horizontalSpeed * velocityDirection;
            _spriteRenderer.color = Color.green;
        }
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
            if ((normal - Vector2.left).magnitude < Epsilon)
            {
                _currentDirection = Vector2.left;
            }
            else if ((normal - Vector2.right).magnitude < Epsilon)
            {
                _currentDirection = Vector2.right;
            }
            else if (normal.y >= minSlopeY) // on a slope or the ground
            {
                stillGrounded = true;
                _groundNormal = normal;
            }
            else if ((normal - Vector2.down).magnitude < Epsilon)
            {
                if (_rigidbody2D.velocity.y > 0)
                    _velocity.y = -_velocity.y;
                    // _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, -_rigidbody2D.velocity.y);
            }
            else
            {
                Debug.Log("Unrecognised normal: (" + normal.x + ", " + normal.y + ")");
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

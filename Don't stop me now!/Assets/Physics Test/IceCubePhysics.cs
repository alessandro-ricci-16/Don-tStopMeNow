using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class IceCubePhysics : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpHeight = 2.0f;
    [SerializeField] private float upwardGravityMultiplier = 1.7f;
    [SerializeField] private float downwardGravityMultiplier = 3.0f;
    [SerializeField] private float defaultGravityMultiplier = 1.0f;

    [SerializeField] private Vector2 _currentDirection;
    private bool _grounded;
    
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    
    void Start()
    {
        _currentDirection = Vector2.right;
        _grounded = false;
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _rigidbody2D = this.GetComponent<Rigidbody2D>();
        _rigidbody2D.gravityScale = 0.0f;
        Debug.Log("Right: " + Vector2.right);
    }
    
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 velocity = new Vector2();
        velocity.x = speed * _currentDirection.x;
        
        if (Input.GetButton("Jump") && _grounded)
            Jump();
        
        if (!_grounded)
        {
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
        else if (_grounded)
        {
            velocity.y = 0.0f;
            _spriteRenderer.color = Color.green;
        }
        
        _rigidbody2D.velocity = velocity;
    }

    void Jump()
    {
        float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpSpeed);
        _grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        int contactsNumber = other.contactCount;
        ContactPoint2D[] contacts = new ContactPoint2D[contactsNumber];
        other.GetContacts(contacts);
        bool stillGrounded = false;
        
        foreach (ContactPoint2D c in contacts)
        {
            Vector2 normal = c.normal;
            if (normal == Vector2.left)
                _currentDirection = Vector2.left;
            else if (normal == Vector2.right)
                _currentDirection = Vector2.right;
            else if (normal == Vector2.up)
                stillGrounded = true;
            else if (normal == Vector2.down)
            {
                if (_rigidbody2D.velocity.y > 0)
                    _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, -_rigidbody2D.velocity.y);
            }
        }
        _grounded = stillGrounded;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        int contactsNumber = other.contactCount;
        ContactPoint2D[] contacts = new ContactPoint2D[contactsNumber];
        other.GetContacts(contacts);
        bool stillGrounded = false;
        // Debug.Log(contactsNumber);
        foreach (ContactPoint2D c in contacts)
        {
            Vector2 normal = c.normal;
            if (normal == Vector2.left)
            {
                _currentDirection = Vector2.left;
            }
            else if (normal == Vector2.right)
            {
                _currentDirection = Vector2.right;
                Debug.Log("Turning right");
            }
            else if (normal == Vector2.up)
                stillGrounded = true;
            else if (normal == Vector2.down)
            {
                if (_rigidbody2D.velocity.y > 0)
                    _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, -_rigidbody2D.velocity.y);
            }
            else
            {
                Debug.Log("Unrecognised normal: " + normal);
                
            }
        }
        _grounded = stillGrounded;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        _grounded = false;
    }
}

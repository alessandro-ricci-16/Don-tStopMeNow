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
[RequireComponent(typeof(Collider2D))]
public class MovingObstacles : MonoBehaviour
{
    public Direction initialDirection;
    public float defaultSpeed;

    private float maxFallingSpeed = 69.0f;
    private float acceleration = 40.0f;
    private float gravityScale = 30.0f;
    
    private float _epsilon = 0.1f;
    // should be Vector2.left or Vector2.right;
    // does not take into account vertical movement by design
    private Vector2 _currentDirection;

    // velocity at the previous frame
    private Vector2 _prevFrameVelocity;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.gravityScale = gravityScale;
        
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (initialDirection == Direction.Left)
            _currentDirection = Vector2.left;
        else
            _currentDirection = Vector2.right;

    }
    private void Update()
    {
        _spriteRenderer.flipX = _currentDirection == Vector2.left;
    }

    private void OnEnable()
    {
        if (initialDirection == Direction.Left)
            _currentDirection = Vector2.left;
        else
            _currentDirection = Vector2.right;
    }

    private void FixedUpdate()
    {
        _prevFrameVelocity = _rigidbody2D.velocity;
        _epsilon = Time.fixedDeltaTime * acceleration;
        
        if (Mathf.Abs(_prevFrameVelocity.x) < defaultSpeed - _epsilon)
        {
            _rigidbody2D.AddForce(acceleration * _currentDirection, ForceMode2D.Force);
        }
        else if (Mathf.Abs(_prevFrameVelocity.x) > defaultSpeed + _epsilon)
        {
            _rigidbody2D.AddForce(-acceleration * _currentDirection, ForceMode2D.Force);
        }
        
        if (Mathf.Abs(_prevFrameVelocity.y) > maxFallingSpeed)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, Mathf.Sign(_prevFrameVelocity.y) * maxFallingSpeed);
        }
    }

    #region Collisions

    /// <summary>
    /// Function to be called inside OnCollisionEnter2D and OnCollisionStay2D.
    /// Takes as input the Collision2D and checks all normals of collision points.
    /// If the normal is Vector2.left or Vector2.right, it adjust the variable
    /// _currentDirection accordingly and applies an impulse to the body in order
    /// to change direction.
    /// </summary>
    /// <param name="other"></param> parameter from OnCollisionEnter2D or
    /// OnCollisionStay2D
    private void HandleCollisions(Collision2D other)
    {
        // get the contacts from the collision
        int contactsNumber = other.contactCount;
        ContactPoint2D[] contacts = new ContactPoint2D[contactsNumber];
        other.GetContacts(contacts);
        
        // iterate and check normals
        foreach (ContactPoint2D c in contacts)
        {
            Vector2 normal = c.normal;
            if ((normal - Vector2.left).magnitude < _epsilon)
            {
                // direction check: avoid applying force multiple times for different contact points
                if (_currentDirection != Vector2.left)
                {
                    _rigidbody2D.velocity = Vector2.zero;
                    _currentDirection = Vector2.left;
                    _rigidbody2D.AddForce(Mathf.Abs(_prevFrameVelocity.x) * Vector2.left, ForceMode2D.Impulse);
                }
            }
            else if ((normal - Vector2.right).magnitude < _epsilon)
            {
                if (_currentDirection != Vector2.right)
                {
                    _rigidbody2D.velocity = Vector2.zero;
                    _currentDirection = Vector2.right;
                    _rigidbody2D.AddForce(Mathf.Abs(_prevFrameVelocity.x) * Vector2.right, ForceMode2D.Impulse);
                }
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        HandleCollisions(other);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        HandleCollisions(other);
    }
    
    #endregion
}
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Collider2D _collider;
    public float speed;
    public float bounceIntensity;
    private PlayerInputAction _inputActions;

    private bool _shouldJump;
    private bool _isGrounded;
    private Vector2 _size;

    private void OnEnable()
    {
        _inputActions = new PlayerInputAction();
        _inputActions.Gameplay.Enable();
        _inputActions.Gameplay.Jump.started += JumpStarted;
        _inputActions.Gameplay.Start.started += StartVelocity;
    }
    
    private void OnDisable()
    {
        _inputActions.Gameplay.Disable();
        _inputActions.Gameplay.Jump.started -= JumpStarted;
        _inputActions = null;
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _size = _collider.bounds.size;
    }

    void Update()
    {
    }
    void FixedUpdate()
    {
        //TODO put the buffer
        if (_shouldJump && _isGrounded)
        {
            _rb.AddForce(Vector2.up * bounceIntensity, ForceMode2D.Impulse);
            _shouldJump = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        /*if (other.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
        /*if (other.gameObject.CompareTag("Bouncy"))
        {
            Debug.Log("Entering bounce collision");
            // Calculate the bounce direction based on the incoming contact angle.
            //TODO probably needs to change this because this is not the real surface normal of the place that I hit
            Vector2 contactNormal = other.contacts[0].normal;
            Debug.Log(contactNormal);
            Vector2 newVelocity = Vector2.Reflect(other.relativeVelocity, contactNormal);
            Debug.Log("velocity: " + other.relativeVelocity + ", reflected_velocity: " + newVelocity);
            // Apply the new velocity to bounce away.
            _rb.velocity = newVelocity;
        }*/
        //TODO probably will not work if we rotate the cube.
        Vector2 pos = transform.position;
        Vector2 contactPoint = other.GetContact(0).point;
        Debug.Log("This.point:"+ pos+" ContactPoint:"+ contactPoint);
        //This check is needed so we can see if the segments joining the two dots is greater than 0 (on the ground) or bigger(hit with a upper platform)
        bool checkYAxis = pos.y - contactPoint.y > 0f;
        //If the width/2 is equal to the distance between the contact point and the anchor then we just collide with something that is not on the ground;
        bool checkXAxis = !Mathf.Approximately(_size.x / 2 ,Math.Abs(pos.x - contactPoint.x));
        
        if (checkYAxis && checkXAxis)
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
        
    }
    private void StartVelocity(InputAction.CallbackContext context)
    {
        _rb.AddForce(speed*Vector2.right, ForceMode2D.Impulse);
        _inputActions.Gameplay.Start.started -= StartVelocity;
    }
    private void JumpStarted(InputAction.CallbackContext context)
    {
        if (_isGrounded)
            _shouldJump = true;
    }
}
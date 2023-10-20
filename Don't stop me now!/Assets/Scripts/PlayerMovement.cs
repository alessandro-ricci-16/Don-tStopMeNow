using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    
    public float speed;
    public float bounceIntensity;
    public float maxAngle;
    
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private PlayerInputAction _inputActions;
    private SpriteRenderer _sprite;
    private Vector2 _size;
    private bool _shouldJump;
    private bool _isGrounded;

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
        _sprite = GetComponent<SpriteRenderer>();
        _size = _collider.bounds.size;
    }

    void Update()
    {
        _sprite.color = _isGrounded ? Color.green : Color.red;
    }
    void FixedUpdate()
    {
        Vector2 pos = transform.position ;
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down);
        if (hit.collider is not null)
        {
            float distance = pos.y - hit.point.y;
            _isGrounded = distance - _size.y/2 <= 0.1f;
        }
        
        if (_isGrounded)
        {
            float dirX = Mathf.Sign(_rb.velocity.x);
            _rb.velocity = new Vector2(speed * dirX,.0f);
            //TODO put the buffer
            if (_shouldJump)
            {
                _rb.AddForce(Vector2.up * bounceIntensity, ForceMode2D.Impulse);
                _shouldJump = false;
                _isGrounded = false;
            }
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
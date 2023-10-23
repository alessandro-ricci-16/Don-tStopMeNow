using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
// using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    
    public float speed;
    public float bounceIntensity;
    public float maxAngle;
    public float coyoteTime = 0.25f;
    
    private Rigidbody2D _rb;
    private Collider2D _collider;
    // private PlayerInputAction _inputActions;
    private SpriteRenderer _sprite;
    private Vector2 _size;
    private bool _shouldJump;
    private bool _isGrounded;
    private bool _inCojoneTime;

    private void OnEnable()
    {
        /*_inputActions = new PlayerInputAction();
        _inputActions.Gameplay.Enable();
        _inputActions.Gameplay.Jump.started += JumpStarted;
        _inputActions.Gameplay.Start.started += StartVelocity;*/
    }
    
    private void OnDisable()
    {
        /*_inputActions.Gameplay.Disable();
        _inputActions.Gameplay.Jump.started -= JumpStarted;
        _inputActions = null;*/
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
        //Debug.Log(_inCojoneTime);
        bool conditionCojone = _inCojoneTime && _rb.velocity.y < 0;
        if(Input.GetKeyDown(KeyCode.Space) && (_isGrounded||conditionCojone))
            _shouldJump = true;
    }
    void FixedUpdate()
    {
        Vector2 pos = transform.position;
        // max distance for the raycast to check the hit
        float groundDistance = _size.y / 2 + 0.01f;
        float raycastOffset = _size.x / 2 - 0.01f;
        // raycast on the left edge
        bool hitRayLeft = Physics2D.Raycast(pos + Vector2.left*raycastOffset, Vector2.down, groundDistance).collider is not null;
        // raycast on the right edge
        bool hitRayRight = Physics2D.Raycast(pos + Vector2.right*raycastOffset, Vector2.down, groundDistance).collider is not null;
        // grounded if both ray cast hits or if just one hits and it's already grounded
        if ((hitRayRight && hitRayLeft ) || ((hitRayRight||hitRayLeft) &&_rb.velocity.y<=0.0f))
        {
            _isGrounded = true;
        }
        else
        {
            // wait for coyote time before disabling jump
            if (_isGrounded && !_inCojoneTime)
            {
                StartCoroutine(CoyoneTimer());
            }
            _isGrounded = false;
        }
        
        // keep y velocity to 0 while on ground
        if (_isGrounded)
        {
            float dirX = Mathf.Sign(_rb.velocity.x);
            _rb.velocity = new Vector2(speed * dirX,.0f);
        }
        // jump
        if (_shouldJump)
        {
            _rb.AddForce(Vector2.up * bounceIntensity, ForceMode2D.Impulse);
            _shouldJump = false;
            _isGrounded = false;
        }
    }
    /*private void StartVelocity(InputAction.CallbackContext context)
    {
        _rb.AddForce(speed*Vector2.right, ForceMode2D.Impulse);
        _inputActions.Gameplay.Start.started -= StartVelocity;
    }
    private void JumpStarted(InputAction.CallbackContext context)
    {
        if (_isGrounded)
            _shouldJump = true;
    }*/
    
    private IEnumerator CoyoneTimer()
    {
        _inCojoneTime = true;
        yield return new WaitForSeconds(coyoteTime);
        _inCojoneTime = false;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Ice_Cube.States;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

/*
 * TODO:
 * - handle jumpCounter better (currently there to avoid double jumping in
 *   the coyote time timeframe) -> integer jump counter? -> also for interrupt jump
 * - make sure the player actually jumped before calling interrupt jump?
 */

/*
 * NOTES
 * - ice cube and platforms must have a physics material 2D with friction = 0 and bounciness = 0
 */
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class IceCubePhysics : MonoBehaviour
{
    [SerializeField] protected IceCubeParameters parameters;
    
    // maximum tolerance for normals in collision handling
    private const float Epsilon = 0.1f;
    
    protected bool OnGround;
    protected bool OnWall;
    protected bool ShouldJump;
    protected bool ShouldGroundPound;
    protected bool IsGroundPounding;
    // updated using the horizontal axis
    protected float XInput;
    
    // should be Vector2.left or Vector2.right;
    // does not take into account vertical movement by design
    private Vector2 _currentDirection;
    // velocity at the previous frame
    private Vector2 _prevFrameVelocity;
    
    private Rigidbody2D _rigidbody2D;
    
    // TODO delete this variable (only here for debugging)
    protected SpriteRenderer SpriteRenderer;
    //TODO this is just put here for implementing and showing result: it can probably be deleted later
    private IceCubeStateManager _iceCubeStateManager;
    
    void Start()
    {
        OnGround = false;
        SpriteRenderer = this.GetComponent<SpriteRenderer>();
        _rigidbody2D = this.GetComponent<Rigidbody2D>();
        _iceCubeStateManager = this.GetComponent<IceCubeStateManager>();
        _rigidbody2D.gravityScale = parameters.downwardGravityScale;
        _rigidbody2D.freezeRotation = true;
        _currentDirection = Vector2.right;
        XInput = 0.0f;
    }
    private void FixedUpdate()
    {
        _prevFrameVelocity = _rigidbody2D.velocity;
        _iceCubeStateManager.GetCurrentState().PerformPhysicsAction(_rigidbody2D, parameters, _currentDirection);
        // TODO THIS IS HERE JUST FOR TESTING IF THE STATES WOKR BUT IT'S REALLY BAD
        if (_iceCubeStateManager.GetCurrentState().GetEnumState() == IceCubeStatesEnum.IsJumping)
        {
            SetGrounded(false);
        }
        

        
        if (ShouldGroundPound)
        {
            GroundPound();
            ShouldGroundPound = false;
        }
        
        // UPDATE GROUND POUNDING STATE
        // when I get on the ground I'm not ground pounding anymore
        if (OnGround)
        {
            IsGroundPounding = false;
        }
        
        // UPDATE MOVEMENT
        // if ice cube is ground pounding, it should follow different movement rules
        if (!IsGroundPounding)
        {
            //HorizontalMovement();
            AdjustGravityScale();
        }

        // adjust sprite to current direction
        SpriteRenderer.flipX = _currentDirection == Vector2.left;
    }

    #region Movement
    
    /// <summary>
    /// Updates the horizontal movement of the rigidbody. Depending on the current direction and player input,
    /// forces are applied on the horizontal axis to match the target horizontal velocity.
    /// This function should be called only if the ice cube is not ground pounding. If the ice cube is ground pounding,
    /// no forces should be added on the horizontal axis until it reaches the ground.
    /// </summary>
    
    
    /// <summary>
    /// This function updates the gravity scale according to whether the ice cube is going upwards or downwards.
    /// It should not be called when the ice cube is ground pounding; if the ice cube is ground pounding, its vertical
    /// velocity should be constant for the whole way down.
    /// </summary>
    private void AdjustGravityScale()
    {
        // if cube is not on the ground, adjust gravity scale
        if (!OnGround)
        {
            if (_prevFrameVelocity.y > 0)
                _rigidbody2D.gravityScale = parameters.upwardGravityScale;
            else if (_prevFrameVelocity.y <= 0)
                _rigidbody2D.gravityScale = parameters.downwardGravityScale;
        }
    }
    
    
    
    /// <summary>
    /// Interrupts a jump. This function is called when the player releases the
    /// "jump" button.
    /// If the vertical velocity is > 0, it divides it by 2.
    /// </summary>
    protected void InterruptJump()
    {
        if (_prevFrameVelocity.y > 0)
        {
            _rigidbody2D.velocity = new Vector2(_prevFrameVelocity.x, _prevFrameVelocity.y / 2);
        }
    }

    protected void GroundPound()
    {
        IsGroundPounding = true;
        // set the gravity scale to zero so only the vertical force affects the rigidbody
        _rigidbody2D.gravityScale = 0.0f;
        // temporarily reset velocity
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.AddForce(parameters.groundPoundSpeed * Vector2.down, ForceMode2D.Impulse);
    }
    
    #endregion
    
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
                if (_prevFrameVelocity.x >= -Mathf.Epsilon && _currentDirection != Vector2.left)
                {
                    _currentDirection = Vector2.left;
                    _rigidbody2D.AddForce(parameters.defaultSpeed*Vector2.left, ForceMode2D.Impulse);
                }
            }
            else if ((normal - Vector2.right).magnitude < Epsilon)
            {
                isPlayerOnWall = true;
                if (_prevFrameVelocity.x <= Mathf.Epsilon && _currentDirection != Vector2.right)
                {
                    _currentDirection = Vector2.right;
                    _rigidbody2D.AddForce(parameters.defaultSpeed*Vector2.right, ForceMode2D.Impulse);
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
        OnWall = isPlayerOnWall;
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
        OnWall = false;
    }
    
    #endregion
    private void SetGrounded(bool grounded)
    {
        if (OnGround != grounded)
        {
            OnGround = grounded;
            EventManager.TriggerEvent(EventNames.OnGround, grounded);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D target;
    
    [Header("Movement parameters")]
    public float defaultAcceleration = 40;
    public float maxAcceleration = 60;
    
    [Header("Offset")]
    public float offsetX = 2.0f;
    
    [Header("Soft and dead zones")]
    public float deadZoneX = 2;
    public float deadZoneY = 8;
    public float softZoneX = 4;
    public float softZoneY = 10;
    
    public Direction _targetDirection;
    public Vector2 _currentSpeed;
    
    private void Update()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        // compute target position
        Vector3 targetPosition = ComputeTargetPosition();
        Vector3 currentPosition = this.transform.position;
        
        // debug
        DrawPosition(targetPosition, Color.red);
        DrawPosition(currentPosition, Color.yellow);
        
        // compute difference
        Vector3 difference = targetPosition - currentPosition;
        
        // update speed and compute delta
        UpdateSpeedX(difference);
        UpdateSpeedY(difference);
        Vector3 delta = new Vector3(_currentSpeed.x, _currentSpeed.y, 0) * Time.deltaTime;
        
        DebugDrawSpeed();
        
        // compute new position
        Vector3 newPosition = currentPosition + delta;
        this.transform.position = newPosition;
    }

    private Vector3 ComputeTargetPosition()
    {
        Vector3 targetPosition = target.position;
        float targetVelocityX = target.velocity.x;
        
        // update direction: if velocity is 0, keep the current direction
        if (targetVelocityX > 0)
        {
            _targetDirection = Direction.Right;
        }
        else if (targetVelocityX < 0)
        {
            _targetDirection = Direction.Left;
        }
        
        float sign = _targetDirection == Direction.Right ? 1 : -1;
        targetPosition.x += offsetX * sign;
        return targetPosition;
    }
    
    /// <summary>
    /// Updates the speed in the X axis
    /// </summary>
    /// <param name="difference"></param> should be target - current
    private void UpdateSpeedX(Vector3 difference)
    {
        float absDifferenceX = Mathf.Abs(difference.x);

        if (absDifferenceX < deadZoneX)
        {
            // do nothing
        }
        // if the target is still in the soft zone, modify the speed by the default acceleration
        else if (absDifferenceX < softZoneX)
        {
            _currentSpeed.x += defaultAcceleration * Mathf.Sign(difference.x) * Time.deltaTime;
        }
        // else if the target is out of the soft zone, modify the speed by the max acceleration
        else
        {
            _currentSpeed.x += maxAcceleration * Mathf.Sign(difference.x) * Time.deltaTime;
        }
    }
    
    /// <summary>
    /// Updates the speed in the Y axis
    /// </summary>
    /// <param name="difference"></param> should be target - current
    private void UpdateSpeedY(Vector3 difference)
    {
        float absDifferenceY = Mathf.Abs(difference.y);
        
        // if the target is in the dead zone, slow down to zero
        if (absDifferenceY < deadZoneY)
        {
            // if current speed is not already zero
            if (_currentSpeed.y != 0)
            {
                _currentSpeed.y -= defaultAcceleration * Mathf.Sign(_currentSpeed.y) * Time.deltaTime;
                // if speed is slow, cut it to zero
                if (Mathf.Abs(_currentSpeed.y) < defaultAcceleration * Time.deltaTime)
                {
                    _currentSpeed.y = 0;
                }
            }
        }
        // if the target is in the soft zone, modify the velocity by the default acceleration
        else if (absDifferenceY < softZoneY)
        {
            _currentSpeed.y += defaultAcceleration * Mathf.Sign(difference.y) * Time.deltaTime;
        }
        // else if the target is out of the soft zone, modify the velocity by the max acceleration
        else
        {
            _currentSpeed.y += maxAcceleration * Mathf.Sign(difference.y) * Time.deltaTime;
        }
    }

    #region Debug drawings
    
    private void DrawPosition(Vector3 position, Color color)
    {
        Debug.DrawLine(position + Vector3.up * 0.5f, position + Vector3.down * 0.5f, color);
        Debug.DrawLine(position + Vector3.left * 0.5f, position + Vector3.right * 0.5f, color);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(deadZoneX * 2, deadZoneY * 2, 0));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(softZoneX * 2, softZoneY * 2, 0));
    }
    
    private void DebugDrawSpeed()
    {
        Vector3 currentPosition = this.transform.position;
        Vector3 speed = new Vector3(_currentSpeed.x, _currentSpeed.y, 0);
        Debug.DrawLine(currentPosition, currentPosition + speed, Color.magenta);
    }
    
    #endregion
}

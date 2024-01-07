using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JumpType
{
    Jump,
    WallJump
}

public class IceCubeJumpAnimation : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Vector2 _currentDirection;
    public JumpType _jumpType;

    private void Awake()
    {
        gameObject.SetActive(false);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        EventManager.StartListening(EventNames.ChangedDirection,
            direction => _currentDirection = new Vector2(direction.x, direction.y));
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        if (_jumpType == JumpType.Jump)
            _spriteRenderer.flipX = _currentDirection == Vector2.left;
        if (_jumpType == JumpType.WallJump)
            _spriteRenderer.flipY = !(_currentDirection == Vector2.left);
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventNames.ChangedDirection,
            direction => _currentDirection = new Vector2(direction.x, direction.y));
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCubeJumpAnimation : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Vector2 _currentDirection;
    private void Awake()
    {
        gameObject.SetActive(false);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        EventManager.StartListening(EventNames.ChangedDirection, direction => _currentDirection= new Vector2(direction.x,direction.y) );
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        _spriteRenderer.flipX = _currentDirection == Vector2.left;
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventNames.ChangedDirection, direction => _currentDirection= new Vector2(direction.x,direction.y));
    }
}

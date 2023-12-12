using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAnimation : MonoBehaviour
{
    public Vector2 translationShift;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _animator.Play("Dashing",0,0);
        transform.position += (Vector3) translationShift;
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}
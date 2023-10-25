using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatedPlatform : MonoBehaviour
{
    //we have to create a function that whne the player enters the heated platform, it will start a timer. As long as it stays in contact with the platform the timer will enhance,
    //when it reaches a max value the player will die

    [SerializeField] private float maxTime = 5f;
    [SerializeField]
    [Range(0.01f, float.MaxValue)] // Set a minimum value of 0.01, which is greater than 0
    private float recoveryScale = 1f;
    private float _timer;
    private bool _isPlayerOn;

    private void Update()
    {
        if (_isPlayerOn)
        {
            _timer += Time.deltaTime;
            
        }
        else if (_timer > 0)
        {
            _timer -= recoveryScale*Time.deltaTime;
            _timer = Mathf.Clamp(_timer, 0, maxTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision");
        if (other.gameObject.CompareTag("Player"))
        {
            _isPlayerOn = true;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (_timer >= maxTime)
        {
            GameManager.Instance.Die(other.transform.position);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _isPlayerOn = false;
        }
    }
}
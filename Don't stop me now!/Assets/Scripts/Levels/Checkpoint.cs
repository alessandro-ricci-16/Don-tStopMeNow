using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    [SerializeField] private bool isFinal = false;
    [SerializeField] private Direction startDirection = Direction.Right;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isFinal)
            {
                EventManager.TriggerEvent(EventNames.LevelPassed, SceneManager.GetActiveScene().name);
            }
            else
            {
                EventManager.TriggerEvent(EventNames.CheckpointPassed, this.transform.position, startDirection);
            }
        }
    }
}

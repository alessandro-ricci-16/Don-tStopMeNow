using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    [Tooltip("if isFinal is true, then this checkpoint triggers the end of the level")]
    [SerializeField] private bool isFinal = false;
    [Tooltip("Index of the checkpoint in the level. -1 signals invalid index.")]
    [SerializeField] private int checkpointIndex = -1;
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
                EventManager.TriggerEvent(EventNames.CheckpointFeedback, SceneManager.GetActiveScene().name, checkpointIndex);
            }
        }
    }
}

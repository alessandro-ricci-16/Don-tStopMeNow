using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    [SerializeField] private bool isFinal = false;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isFinal)
        {
            EventManager.TriggerEvent(EventNames.LevelPassed, SceneManager.GetActiveScene().name);
        }
        else
        {
            EventManager.TriggerEvent(EventNames.CheckpointPassed, this.transform.position);
        }
    }
}

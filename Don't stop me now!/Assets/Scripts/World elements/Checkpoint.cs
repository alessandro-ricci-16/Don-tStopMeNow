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
    
    private bool feedbackSent = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isFinal && !feedbackSent)
            {
                EventManager.TriggerEvent(EventNames.LevelPassed, SceneManager.GetActiveScene().name);
                
                int buildIndex = SceneManager.GetActiveScene().buildIndex + 1;
                
                // send feedback that the level has been completed, only if the next scene is a level
                // (the very last scene is the end of game scene)
                if (buildIndex <= SceneManager.sceneCountInBuildSettings - 1)
                {
                    string sceneName = SceneUtility.GetScenePathByBuildIndex(buildIndex);
                    // Extract the scene name from the full path
                    sceneName = System.IO.Path.GetFileNameWithoutExtension(sceneName);
                    
                    EventManager.TriggerEvent(EventNames.LevelStarted, sceneName);
                }
                
                feedbackSent = true;
            }
            else if (!isFinal)
            {
                EventManager.TriggerEvent(EventNames.CheckpointPassed, this.transform.position, startDirection);
                EventManager.TriggerEvent(EventNames.CheckpointPassed, SceneManager.GetActiveScene().name, checkpointIndex);
            }
        }
    }
}

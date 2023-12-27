using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.HID;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectionUIManager : MonoBehaviour
{
    public Button[] world1LevelsButtons;
    public Button[] world2LevelsButtons;

    private void Start()
    {
        int world1Offset = 3;
        int world2Offset = world1Offset + world1LevelsButtons.Length;
        
        for (int i = 0; i < world1LevelsButtons.Length; i++)
        {
            int index = i;
            world1LevelsButtons[i].onClick.AddListener(() => LoadLevel(index + world1Offset));
        }
        
        for (int i = 0; i < world2LevelsButtons.Length; i++)
        {
            int index = i;
            world2LevelsButtons[i].onClick.AddListener(() => LoadLevel(index + world2Offset));
        }
    }

    private void LoadLevel(int levelIndex)
    {
        string sceneName = SceneUtility.GetScenePathByBuildIndex(levelIndex);
        // Extract the scene name from the full path
        sceneName = System.IO.Path.GetFileNameWithoutExtension(sceneName);
        
        EventManager.TriggerEvent(EventNames.LevelStarted, sceneName);
        
        GameManager.Instance.LoadLevel(levelIndex);
    }
    
    public void LoadMainMenuScreen()
    {
        GameManager.Instance.LoadMainMenuScene();
    }
    
    public void LoadFeedbackScreen()
    {
        GameManager.Instance.LoadFeedbackScene();
    }
    
    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}

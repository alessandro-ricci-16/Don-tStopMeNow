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
    public Button quitGameButton;
    public Button giveFeedbackButton;
    public Button[] levelsButtons;

    private UnityAction<string> _loadLevelAction;

    private void Start()
    {
        quitGameButton.onClick.AddListener(QuitGame);
        giveFeedbackButton.onClick.AddListener(LoadFeedbackScreen);

        for (int i = 0; i < levelsButtons.Length; i++)
        {
            int index = i;
            levelsButtons[i].onClick.AddListener(() => LoadLevel(index+3));
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
    
    private void LoadFeedbackScreen()
    {
        GameManager.Instance.LoadFeedbackScene();
    }
    
    private void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}

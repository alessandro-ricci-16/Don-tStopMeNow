using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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
    public CinemachineVirtualCamera world1Camera;
    public CinemachineVirtualCamera world2Camera;


    private void Start()
    {
        int world1Offset = GameManager.Instance.initialScenesOffset;
        int world2Offset = world1Offset + GameManager.Instance.world1LevelsNumber +
                           GameManager.Instance.world2ScreenOffset;

        for (int i = 0; i < world1LevelsButtons.Length && i < GameManager.Instance.world1LevelsNumber; i++)
        {
            int index = i;
            world1LevelsButtons[i].onClick.AddListener(() => LoadLevel(index + world1Offset + 1));
        }

        for (int i = 0; i < world2LevelsButtons.Length && i < GameManager.Instance.world2LevelsNumber; i++)
        {
            int index = i;
            world2LevelsButtons[i].onClick.AddListener(() => LoadLevel(index + world2Offset + 1));
        }

        world1Camera.enabled = true;
        world2Camera.enabled = false;
    }

    private void LoadLevel(int levelIndex)
    {
        Cursor.visible = false;
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

    public void LoadWorld1Screen()
    {
        world1Camera.enabled = true;
        world2Camera.enabled = false;
    }

    public void LoadWorld2Screen()
    {
        world1Camera.enabled = false;
        world2Camera.enabled = true;
    }
}
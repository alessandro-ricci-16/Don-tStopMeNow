using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    public CinemachineVirtualCamera mainMenuCamera;
    public CinemachineVirtualCamera creditsCamera;
    public CinemachineVirtualCamera settingsCamera;

    private void Start()
    {
        mainMenuCamera.enabled = true;
        creditsCamera.enabled = false;
        settingsCamera.enabled = false;
    }

    public void LoadLevelSelectionScene()
    {
        GameManager.Instance.LoadLevelSelectionScene();
    }

    public void Play()
    {
        GameManager.Instance.LoadLevel(GameManager.Instance.initialScenesOffset + 1);
    }

    public void LoadFeedbackScene()
    {
        GameManager.Instance.LoadFeedbackScene();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }

    public void LoadCreditsScene()
    {
        mainMenuCamera.enabled = false;
        creditsCamera.enabled = true;
        settingsCamera.enabled = false;
    }

    public void LoadSettingsScene()
    {
        mainMenuCamera.enabled = false;
        creditsCamera.enabled = false;
        settingsCamera.enabled = true;
    }

    public void LoadMainMenuScene()
    {
        mainMenuCamera.enabled = true;
        creditsCamera.enabled = false;
        settingsCamera.enabled = false;
    }
}
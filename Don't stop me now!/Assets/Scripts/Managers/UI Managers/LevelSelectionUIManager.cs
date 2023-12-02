using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.HID;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelSelectionUIManager : MonoBehaviour
{
    public Button BackToMainMenuButton;
    public Button GiveFeedbackButton;
    public Button[] LevelsButtons;

    private UnityAction<string> _loadLevelAction;

    private void Start()
    {
        BackToMainMenuButton.onClick.AddListener(LoadMainMenu);
        GiveFeedbackButton.onClick.AddListener(LoadFeedbackScreen);

        for (int i = 0; i < LevelsButtons.Length; i++)
        {
            int index = i;
            LevelsButtons[i].onClick.AddListener(() => LoadLevel(index+3));
        }
    }

    private void LoadMainMenu()
    {
        GameManager.Instance.LoadMainMenuScene();
    }

    private void LoadLevel(int levelIndex)
    {
        GameManager.Instance.LoadLevel(levelIndex);
    }
    
    private void LoadFeedbackScreen()
    {
        GameManager.Instance.LoadFeedbackScene();
    }
}

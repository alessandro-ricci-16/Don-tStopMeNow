using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelSelectionUIManager : MonoBehaviour
{
    public Button BackToMainMenuButton;
    public Button[] LevelsButtons;

    private UnityAction<string> _loadLevelAction;

    private void Start()
    {
        BackToMainMenuButton.onClick.AddListener(LoadMainMenu);

        for (int i = 0; i < LevelsButtons.Length; i++)
        {
            int index = i;
            LevelsButtons[i].onClick.AddListener(() => LoadLevel(index+2));
        }
    }

    private void LoadMainMenu()
    {
        GameManager.Instance.LoadMainMenuScene();
    }

    private void LoadLevel(int levelIndex)
    {
        Debug.Log("Loading scene " + levelIndex);
        GameManager.Instance.LoadLevel(levelIndex);
    }
}

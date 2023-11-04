using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelSelectionUIManager : MonoBehaviour
{
    public Button BackToMainMenuButton;
    public ButtonSceneName[] ButtonSceneNames;

    private UnityAction<string> _loadLevelAction;

    private void Start()
    {
        BackToMainMenuButton.onClick.AddListener(LoadMainMenu);

        foreach (ButtonSceneName bsn in ButtonSceneNames)
        {
            bsn.Button.onClick.AddListener(() => LoadLevel(bsn.SceneName));
        }
    }

    private void LoadMainMenu()
    {
        GameManager.Instance.LoadMainMenuScene();
    }

    private void LoadLevel(string levelName)
    {
        GameManager.Instance.LoadLevel(levelName);
    }
}

[Serializable]
public class ButtonSceneName
{
    public Button Button;
    public string SceneName;
}

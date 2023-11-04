using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionUIManager : MonoBehaviour
{
    public Button BackToMainMenuButton;
    public ButtonSceneName[] ButtonSceneNames;

    private void Start()
    {
        BackToMainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    private void LoadMainMenu()
    {
        GameManager.Instance.LoadMainMenuScene();
    }

    private void LoadLevel(string levelName)
    {
        
    }
}

[Serializable]
public class ButtonSceneName
{
    public Button Button;
    public string SceneName;
}

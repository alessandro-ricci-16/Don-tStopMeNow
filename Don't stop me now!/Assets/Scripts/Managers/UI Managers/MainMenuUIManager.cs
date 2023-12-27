using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    public void LoadLevelSelectionScene()
    {
        GameManager.Instance.LoadLevelSelectionScene();
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
        GameManager.Instance.LoadCreditsScene();
    }
    
    public void LoadSettingsScene()
    {
        GameManager.Instance.LoadSettingsScene();
    }
}

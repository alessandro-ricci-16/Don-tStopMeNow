using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackScreenUIManager : MonoBehaviour
{
    public void LoadMainMenuScreen()
    {
        GameManager.Instance.LoadMainMenuScene();
    }
    
    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}

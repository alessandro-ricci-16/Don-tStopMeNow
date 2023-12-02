using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScenesData
{
    private static string _mainMenuSceneName = "Main Menu";
    private static string _levelSelectionSceneName = "Level Selection";
    private static string _feedbackSceneName = "Give Feedback Screen";

    public static string MainMenuSceneName()
    {
        return _mainMenuSceneName;
    }

    public static string LevelSelectionSceneName()
    {
        return _levelSelectionSceneName;
    }
    
    public static string FeedbackSceneName()
    {
        return _feedbackSceneName;
    }
}

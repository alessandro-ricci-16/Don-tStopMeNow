using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScenesData
{
    private static string _mainMenuSceneName = "Main Menu";
    private static string _levelSelectionSceneName = "Level Selection";
    
    // progression of levels scene names
    private static List<string> _levels = new() {"Level1.1", "Level1.2"};

    public static string MainMenuSceneName()
    {
        return _mainMenuSceneName;
    }

    public static string LevelSelectionSceneName()
    {
        return _levelSelectionSceneName;
    }
    
    public static string NextLevelSceneName(string levelName)
    {
        if (!_levels.Contains(levelName))
        {
            Debug.LogError("Invalid scene name: " + levelName + " is not contained in LevelData");
            return null;
        }
        else
        {
            return _levels[_levels.IndexOf(levelName)+1];
        }
    }

    public static string FirstLevelSceneName()
    {
        return _levels[0];
    }
}

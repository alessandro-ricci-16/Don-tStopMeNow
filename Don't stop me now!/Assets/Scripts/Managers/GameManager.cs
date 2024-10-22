using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private readonly float _timeScaleDeath = 1.0f;

    public readonly int initialScenesOffset = 2;
    public readonly int world1LevelsNumber = 25;
    public readonly int world2ScreenOffset = 1;
    public readonly int world2LevelsNumber = 25;

    public bool StartAtCheckPoint { get; private set; }
    public Vector3 LastCheckpoint { get; private set; }
    public Direction CheckpointStartDirection { get; private set; }
    public int DeathCounter { get; private set; }

    private Dictionary<int, bool> _varForCheckpoints = new Dictionary<int, bool>();
    private Animator _canvasAnimator;
    private Vignette _vignetteEffect;
    private Action _functionToPlay;

    // Method to retrieve a Vector3 using a gameUid


    protected override void Awake()
    {
        //invoke the awake method of the singleton class
        base.Awake();

        // PROCESSING
        // get animator component
        _canvasAnimator = GetComponent<Animator>();
        // try to get the vignette component of the post processing volume
        Volume postProcessing = GetComponentInChildren<Volume>();
        if (postProcessing != null && postProcessing.profile.TryGet<Vignette>(out var tmp))
            _vignetteEffect = tmp;
        else
            Debug.Log("Cannot get vignette effect");

        // DEATH
        // add Die function to the Death event
        EventManager.StartListening(EventNames.Death, Die);

        // checkpoint passed
        FlushForCheckpoint();
        EventManager.StartListening(EventNames.CheckpointPassed, CheckpointPassed);

        // level passed 
        EventManager.StartListening(EventNames.LevelPassed, LevelPassed);
    }
    public void SetCheckpointsVariable(int id, bool value)
    {
        _varForCheckpoints[id] = value;
    }
    public bool GetCheckpointsVariable(int id)
    {
        return _varForCheckpoints[id];
    }

    public int GetLastUnlockedLevel()
    {
        int lastUnlockedLevel = PlayerPrefs.GetInt("LastUnlockedLevel", initialScenesOffset + 1);

        // TODO: rework scene indexes in case more worlds are added
        // just in the case the lastUnlockedLevel scene index corresponds to a load scene in between the 2 worlds -> set it to the first level of world2
        int world2Offset = initialScenesOffset + world1LevelsNumber + world2ScreenOffset;
        if (lastUnlockedLevel > initialScenesOffset + world1LevelsNumber && lastUnlockedLevel <= world2Offset)
            lastUnlockedLevel = world2Offset + 1;
        
        return lastUnlockedLevel;
    }

    #region Events functions

    private void Die(String levelName, Vector3 playerPosition)
    {
        // update the time scale
        Time.timeScale = _timeScaleDeath;

        // update the death counter
        DeathCounter++;
        EventManager.TriggerEvent(EventNames.UpdateDeathCounter);

        // center the vignette animation on the passed position
        if (Camera.main != null)
        {
            Vector2 vignetteCenter = Camera.main.WorldToViewportPoint(playerPosition);
            _vignetteEffect.center.value = vignetteCenter;
        }

        // play the die state of the animator that will trigger the ReloadScene() method at the end
        _functionToPlay = ReloadScene;
        _canvasAnimator.Play("Die");
    }

    private void CheckpointPassed(Vector3 checkpointPosition, Direction direction)
    {
        StartAtCheckPoint = true;
        LastCheckpoint = checkpointPosition;
        CheckpointStartDirection = direction;
    }
    private void FlushForCheckpoint()
    {
        StartAtCheckPoint = false;
        _varForCheckpoints.Clear();
    }

    private void LevelPassed(string levelName)
    {
        FlushForCheckpoint();
        // play the fade in state of the animator that will trigger the LoadNextScene() method at the end
        _functionToPlay = LoadNextScene;
        _canvasAnimator.Play("FadeIn");
    }

    #endregion

    public void AnimationCallbackHandler()
    {
        // play the function set in the attribute, use this at the end of an animation
        _functionToPlay();
    }


    #region Scene Loading

    private void ReloadScene()
    {
        ChangeScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void LoadNextScene()
    {
        // call me to load the next index
        int index = SceneManager.GetActiveScene().buildIndex + 1;

        // reset the checkpoint
        FlushForCheckpoint();
        DeathCounter = 0;

        if (index <= SceneManager.sceneCountInBuildSettings)
            ChangeScene(index);
        else
        {
            LoadLevelSelectionScene();
        }
    }

    public void LoadMainMenuScene()
    {
        ChangeScene(0);
        DeathCounter = 0;
        FlushForCheckpoint();
    }

    public void LoadLevelSelectionScene()
    {
        ChangeScene(1);
        DeathCounter = 0;
        FlushForCheckpoint();
    }

    public void LoadFeedbackScene()
    {
        ChangeScene(2);
        DeathCounter = 0;
        FlushForCheckpoint();
    }

    public void LoadFirstLevel()
    {
        ChangeScene(initialScenesOffset + 1);
        DeathCounter = 0;
        FlushForCheckpoint();
        
        string sceneName = SceneUtility.GetScenePathByBuildIndex(initialScenesOffset + 1);
        // Extract the scene name from the full path
        sceneName = System.IO.Path.GetFileNameWithoutExtension(sceneName);
        
        EventManager.TriggerEvent(EventNames.LevelStarted, sceneName);
    }


    public void LoadLevel(int levelIndex)
    {
        if (levelIndex <= SceneManager.sceneCountInBuildSettings)
            ChangeScene(levelIndex);
        else
            Debug.Log("Level index out of range");
    }

    private void ChangeScene(int scene)
    {
        StartCoroutine(LoadAsyncScene(scene));
    }

    private IEnumerator LoadAsyncScene(int index)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            // float progress = asyncLoad.progress;
            yield return null;
        }

        // play the fadeout animation once the scene is loaded
        _canvasAnimator.Play("FadeOut");
        // reset the time scale to normal
        Time.timeScale = 1.0f;

        EventManager.TriggerEvent(EventNames.NewSceneLoaded);
    }

    #endregion

    public void QuitGame()
    {
        Application.Quit();
    }


    #region Level Parameters

    public bool SceneIsLevel(int index)
    {
        return index > initialScenesOffset && index != initialScenesOffset + world1LevelsNumber + world2ScreenOffset &&
               index <= initialScenesOffset + world1LevelsNumber + world2ScreenOffset + world2LevelsNumber;
    }

    public bool SceneIsLevel()
    {
        return SceneIsLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public bool SceneIsWorld1(int index)
    {
        return index > initialScenesOffset && index <= initialScenesOffset + world1LevelsNumber;
    }

    public bool SceneIsWorld1()
    {
        return SceneIsWorld1(SceneManager.GetActiveScene().buildIndex);
    }

    public bool SceneIsWorld2(int index)
    {
        return index > initialScenesOffset + world1LevelsNumber + world2ScreenOffset &&
               index <= initialScenesOffset + world1LevelsNumber + world2ScreenOffset + world2LevelsNumber;
    }

    public bool SceneIsWorld2()
    {
        return SceneIsWorld2(SceneManager.GetActiveScene().buildIndex);
    }

    public bool SceneIsWorld2Screen(int index)
    {
        return index == initialScenesOffset + world1LevelsNumber + world2ScreenOffset;
    }

    public bool SceneIsWorld2Screen()
    {
        return SceneIsWorld2Screen(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion
}
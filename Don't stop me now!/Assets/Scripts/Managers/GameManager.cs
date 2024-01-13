using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private readonly float _timeScaleDeath = 1.0f;

    [Header("Level Number Parameters")] 
    public int initialScenesOffset = 2;
    public int world1LevelsNumber = 25;
    public int world2ScreenOffset = 1;
    public int world2LevelsNumber = 25;
    
    public bool StartAtCheckPoint { get; private set; }
    public Vector3 LastCheckpoint { get; private set; }
    public Direction CheckpointStartDirection { get; private set; }
    public int DeathCounter { get; private set; }
    
    private Animator _canvasAnimator;
    private Vignette _vignetteEffect;
    private Action _functionToPlay;
    
    protected override void Awake() 
    { 
        //invoke the awake method of the singleton class
        base.Awake();
        
        // PROCESSING
        // get animator component
        _canvasAnimator = GetComponent<Animator>();
        // try to get the vignette component of the post processing volume
        Volume postProcessing = GetComponentInChildren<Volume>();
        if(postProcessing != null && postProcessing.profile.TryGet<Vignette>( out var tmp ) )
            _vignetteEffect = tmp;
        else
            Debug.Log("Cannot get vignette effect");
        
        // DEATH
        // add Die function to the Death event
        EventManager.StartListening(EventNames.Death, Die);
        
        // checkpoint passed
        StartAtCheckPoint = false;
        EventManager.StartListening(EventNames.CheckpointPassed, CheckpointPassed);
        
        // level passed 
        EventManager.StartListening(EventNames.LevelPassed, LevelPassed);
    }

    #region Events functions
    
    private void Die(String levelName, Vector3 playerPosition)
    {
        // update the time scale
        Time.timeScale = _timeScaleDeath;
        
        // update the death counter
        DeathCounter++;
        
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

    private void LevelPassed(string levelName)
    {
        StartAtCheckPoint = false;
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
    
    public void ReloadScene()
    {
        ChangeScene(SceneManager.GetActiveScene().buildIndex);
    }

    
    public void LoadNextScene()
    {
        // call me to load the next index
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        
        // reset the checkpoint
        StartAtCheckPoint = false;
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
    }

    public void LoadLevelSelectionScene()
    {
        ChangeScene(1);
    }
    
    public void LoadFeedbackScene()
    {
        ChangeScene(2);
    }
    
    public void LoadCreditsScene()
    {
        Debug.Log("Credits scene not implemented yet");
    }
    public void LoadSettingsScene()
    {
        Debug.Log("Settings scene not implemented yet");
    }
    
    
    public void LoadEndOfGameScene()
    {
        ChangeScene(SceneManager.sceneCountInBuildSettings - 1);
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelUIManager : Singleton<LevelUIManager>
{
    public Image backgroundImage;

    [Header("Start Level Graphics")] 
    public TextMeshProUGUI levelText;

    [Header("Pause")] public GameObject pauseMenuCanvas;
    public TextMeshProUGUI pauseLevelText;
    public GameObject pauseButtons;

    [Header("Feedback")] public GameObject feedbackMenuCanvas;
    // public TextMeshProUGUI feedbackTitleText;

    [Header("Settings")] 
    public GameObject settingsMenuCanvas;

    [Header("Commands")] 
    public GameObject commandsMenuCanvas;
    
    [Header("DeathCounter")]
    public TextMeshProUGUI deathCounterText;
    public GameObject deathCounterCanvas;
    
    
    private readonly float _fadeDelay = 0f;
    private readonly float _fadeInTime = 0.75f;
    private readonly float _fadeOutTime = 0.5f;
    
    private Animator _levelTextAnimator;
    private Animator _backgroundAnimator;

    private readonly float _backgroundAlpha = 0.8f;
    private bool _paused;

    private int _prevSceneIndex = -1;
    private int _currentLevelIndex;
    private UIInputAction _uiInputAction;

    private void Start()
    {
        _levelTextAnimator = levelText.GetComponent<Animator>();
        _backgroundAnimator = backgroundImage.GetComponent<Animator>();
        
        UpdateUI();
        EventManager.StartListening(EventNames.NewSceneLoaded, UpdateUI);
        EventManager.StartListening(EventNames.UpdateDeathCounter, () => StartCoroutine(UpdateDeathCounterCoroutine()));
        _uiInputAction = new UIInputAction();
        _uiInputAction.Enable();
        _uiInputAction.UI.Pause.started += ctx => PressedEsc();
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventNames.NewSceneLoaded, UpdateUI);
        EventManager.StopListening(EventNames.UpdateDeathCounter, () => StartCoroutine(UpdateDeathCounterCoroutine()));
    }

    private void PressedEsc()
    {
        if (!GameManager.Instance.SceneIsLevel()) return;

        if (!_paused)
        {
            _paused = true;
            CallPause();
        }
        else
        {
            _paused = false;
            Resume();
        }
    }

    #region UpdateUI

    private void UpdateUI()
    {
        pauseMenuCanvas.SetActive(false);

        int i = SceneManager.GetActiveScene().buildIndex;
        bool isLevel = GameManager.Instance.SceneIsLevel(i);
        
        if (isLevel)
        {
            deathCounterCanvas.gameObject.SetActive(true);
            UpdateDeathCounter();
        }
        else
        {
            deathCounterCanvas.gameObject.SetActive(false);
        }
        
        // sometimes the black background is still active
        DeactivatePauseMenu();

        // expensive method invocation -> only update if the scene changed
        if (i != _prevSceneIndex && isLevel)
        {
            _prevSceneIndex = i;
            _currentLevelIndex = CalculateLevelIndex();
            
            levelText.text = "Level " + _currentLevelIndex;
            pauseLevelText.text = "Level " + _currentLevelIndex;
            // feedbackTitleText.text = "Feedback about level " + _currentLevelIndex;
            
            StartCoroutine(FadeInOutLevelText());
        }

    }

    private void UpdateDeathCounter()
    {
        deathCounterText.text = "x " + GameManager.Instance.DeathCounter;
    }

    private IEnumerator UpdateDeathCounterCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        UpdateDeathCounter();
    }

    private int CalculateLevelIndex()
    {
        int i = SceneManager.GetActiveScene().buildIndex;

        if (GameManager.Instance.SceneIsWorld1(i))
        {
            return i - GameManager.Instance.initialScenesOffset;
        }
        else
        {
            return i - (GameManager.Instance.initialScenesOffset + GameManager.Instance.world1LevelsNumber
                                                                 + GameManager.Instance.world2ScreenOffset);
        }
    }

    #endregion

    #region Pause

    private void CallPause()
    {
        EventManager.TriggerEvent(EventNames.GamePause);
        Time.timeScale = 0;
        
        // if calling pause while level text fade in/out is still happening
        StopAllCoroutines();
        levelText.gameObject.SetActive(false);
        // reset background color
        backgroundImage.color = new Color(0, 0, 0, _backgroundAlpha);
        
        backgroundImage.gameObject.SetActive(true);
        pauseMenuCanvas.SetActive(true);
        pauseButtons.SetActive(true);
    }

    public void Resume()
    {
        _paused = false;
        EventManager.TriggerEvent(EventNames.GameResume);
        DeactivatePauseMenu();
        Time.timeScale = 1;
    }

    private void DeactivatePauseMenu()
    {
        pauseMenuCanvas.SetActive(false);
        pauseButtons.SetActive(false);
        levelText.gameObject.SetActive(false);
        backgroundImage.gameObject.SetActive(false);
        settingsMenuCanvas.SetActive(false);
        commandsMenuCanvas.SetActive(false);
        feedbackMenuCanvas.SetActive(false);
    }
    
    public void BackToPauseMenu()
    {
        settingsMenuCanvas.SetActive(false);
        commandsMenuCanvas.SetActive(false);
        feedbackMenuCanvas.SetActive(false);

        pauseMenuCanvas.SetActive(true);
        pauseButtons.SetActive(true);
    }

    #endregion

    #region Feedback

    public void EnableFeedbackScreen()
    {
        pauseButtons.SetActive(false);
        commandsMenuCanvas.SetActive(false);
        settingsMenuCanvas.SetActive(false);
        feedbackMenuCanvas.SetActive(true);
    }


    #endregion

    #region Load Scenes

    public void BackToLevelSelection()
    {
        // DeactivatePauseMenu();
        GameManager.Instance.LoadLevelSelectionScene();
    }
    
    public void BackToMainMenu()
    {
        // DeactivatePauseMenu();
        GameManager.Instance.LoadMainMenuScene();
    }

    public void SkipLevel()
    {
        // DeactivatePauseMenu();
        FeedbackManager.Instance.SendFeedback(SceneManager.GetActiveScene().name, "Level Skipped", "");
        GameManager.Instance.LoadNextScene();
    }

    #endregion

    #region Graphics
    
    private IEnumerator FadeInOutLevelText()
    {
        // Time.timeScale = 0;
        
        // reset background color
        backgroundImage.color = new Color(0, 0, 0, _backgroundAlpha);
        levelText.gameObject.SetActive(true);
        backgroundImage.gameObject.SetActive(true);
        
        _levelTextAnimator.SetFloat(Animator.StringToHash("FadeInScale"), 1 / _fadeInTime);
        _levelTextAnimator.SetFloat(Animator.StringToHash("FadeOutScale"), 1 / _fadeOutTime);
        _backgroundAnimator.SetFloat(Animator.StringToHash("FadeOutScale"), 1 / _fadeOutTime);
        
        _levelTextAnimator.Play("FadeInText");

        yield return new WaitForSecondsRealtime(_fadeInTime + _fadeDelay);
        
        // Time.timeScale = 1;
        
        _levelTextAnimator.Play("FadeOutText");
        _backgroundAnimator.Play("FadeOutBackground");
        
        yield return new WaitForSecondsRealtime(_fadeOutTime);
        
        levelText.gameObject.SetActive(false);
        backgroundImage.gameObject.SetActive(false);
    }
    
    #endregion

    #region Settings

    public void CallSettings()
    {
        commandsMenuCanvas.SetActive(false);
        feedbackMenuCanvas.SetActive(false);
        
        pauseButtons.SetActive(false);
        settingsMenuCanvas.SetActive(true);
    }

    #endregion

    #region Commands

    public void CallCommands()
    {
        pauseButtons.SetActive(false);
        feedbackMenuCanvas.SetActive(false);
        settingsMenuCanvas.SetActive(false);
        commandsMenuCanvas.SetActive(true);
    }

    #endregion
}
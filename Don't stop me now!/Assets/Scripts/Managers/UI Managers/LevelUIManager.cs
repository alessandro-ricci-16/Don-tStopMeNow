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

    [Header("Start Level Graphics")] public TextMeshProUGUI levelText;
    
    [Header("Pause")]
    public GameObject pauseMenuCanvas;
    public TextMeshProUGUI pauseLevelText;
    public GameObject pauseButtons;

    [Header("Feedback")]
    public GameObject feedbackMenuCanvas;
    public TextMeshProUGUI feedbackTitleText;
    
    [Header("Settings")]
    public GameObject settingsMenuCanvas;
    
    [Header("Commands")]
    public GameObject commandsMenuCanvas;

    private readonly float _fadeDelay = 0.5f;
    private readonly float _fadeInTime = 0.25f;
    private readonly float _fadeOutTime = 0.75f;
    private Color _backgroundColor;
    private bool _paused = false;

    private int _currentSceneIndex;
    private int _currentLevelIndex;

    private void Start()
    {
        _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        _currentLevelIndex = CalculateLevelIndex();
        UpdateUI();
        EventManager.StartListening(EventNames.NewSceneLoaded, UpdateUI);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventNames.NewSceneLoaded, UpdateUI);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PressedEsc();
        }
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
    
    public void UpdateUI()
    {
        pauseMenuCanvas.SetActive(false);

        int i = SceneManager.GetActiveScene().buildIndex;
        
        // expensive method invocation -> only update if the scene changed
        if (i != _currentSceneIndex)
        {
            _currentSceneIndex = i;
            _currentLevelIndex = CalculateLevelIndex();
            levelText.text = "Level " + _currentLevelIndex;
            pauseLevelText.text = "Level " + _currentLevelIndex;
            feedbackTitleText.text = "Feedback about level " + _currentLevelIndex;
            feedbackMenuCanvas.SetActive(false);
        }

        if (GameManager.Instance.SceneIsLevel())
        {
            levelText.gameObject.SetActive(true);
            _backgroundColor = backgroundImage.color;
            backgroundImage.gameObject.SetActive(true);
            StartCoroutine(FadeLevelText());
        }
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

    public void CallPause()
    {
        EventManager.TriggerEvent(EventNames.GamePause);
        Time.timeScale = 0;
        pauseMenuCanvas.SetActive(true);
        pauseButtons.SetActive(true);
        backgroundImage.color = _backgroundColor;
        backgroundImage.gameObject.SetActive(true);
    }

    public void Resume()
    {
        _paused = false;
        EventManager.TriggerEvent(EventNames.GameResume);
        DeactivateEverything();
        Time.timeScale = 1;
    }

    private void DeactivateEverything()
    {
        pauseMenuCanvas.SetActive(false);
        backgroundImage.gameObject.SetActive(false);
        settingsMenuCanvas.SetActive(false);
        commandsMenuCanvas.SetActive(false);
        feedbackMenuCanvas.SetActive(false);
    }
    
    #endregion

    #region Feedback

    public void EnableFeedbackScreen()
    {
        pauseMenuCanvas.SetActive(false);
        feedbackMenuCanvas.SetActive(true);
    }

    public void BackToPauseMenuFromFeedback()
    {
        feedbackMenuCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(true);
    }

    #endregion

    #region Load Scenes

    public void BackToLevelSelection()
    {
        Time.timeScale = 1;
        DeactivateEverything();
        GameManager.Instance.LoadLevelSelectionScene();
    }
    
    public void SkipLevel()
    {
        Time.timeScale = 1;
        DeactivateEverything();
        FeedbackManager.Instance.SendFeedback(SceneManager.GetActiveScene().name, "Level Skipped", "");
        GameManager.Instance.LoadNextScene();
    }


    #endregion

    #region Graphics

    private IEnumerator FadeLevelText()
    { 
        float elapsedTime = 0;
        Color textColor = levelText.color;
        Color textStartColor = new Color(textColor.r, textColor.g, textColor.b, 1);
        Color textEndColor = new Color(textColor.r, textColor.g, textColor.b, 0);
        Color backgroundStartColor = backgroundImage.color;
        Color backgroundEndColor = new Color(backgroundStartColor.r, backgroundStartColor.g, backgroundStartColor.b, 0);
        
        levelText.color = textEndColor;
        backgroundImage.color = backgroundStartColor;
        
        levelText.gameObject.SetActive(true);
        backgroundImage.gameObject.SetActive(true);
        
        // fade in text
        while (elapsedTime < _fadeInTime)
        {
            levelText.color = Color.Lerp(textEndColor, textStartColor, elapsedTime / _fadeInTime);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        elapsedTime = 0;
        
        yield return new WaitForSeconds(_fadeDelay);
        
        levelText.color = textStartColor;
        
        // fade out
        while (elapsedTime < _fadeOutTime)
        {
            levelText.color = Color.Lerp(textStartColor, textEndColor, elapsedTime / _fadeOutTime);
            backgroundImage.color = Color.Lerp(backgroundStartColor, backgroundEndColor, elapsedTime / _fadeOutTime);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        levelText.gameObject.SetActive(false);
        backgroundImage.gameObject.SetActive(false);
        backgroundImage.color = _backgroundColor;
        levelText.color = textStartColor;
    }

    #endregion
    
    #region Settings
    public void CallSettings()
    {
        pauseButtons.SetActive(false);
        settingsMenuCanvas.SetActive(true);
    }
    public void BackToPauseMenuFromSettings()
    {
        pauseButtons.SetActive(true);
        settingsMenuCanvas.SetActive(false);
    }
    #endregion
    
    #region Commands
    public void CallCommands()
    {
        pauseButtons.SetActive(false);
        commandsMenuCanvas.SetActive(true);
    }
    public void BackToPauseMenuFromCommands()
    {
        pauseButtons.SetActive(true);
        commandsMenuCanvas.SetActive(false);
    }
    #endregion
}

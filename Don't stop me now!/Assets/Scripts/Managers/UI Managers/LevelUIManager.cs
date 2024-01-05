using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelUIManager : Singleton<LevelUIManager>
{
    public float fadeTime = 1f;
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
    
    
    private Color _backgroundColor;
    private bool _paused = false;

    private void Start()
    {
        UpdateUI();
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

        int levelIndex = CalculateLevelIndex();
        levelText.text = "Level " + levelIndex;
        pauseLevelText.text = "Level " + levelIndex;
        feedbackTitleText.text = "Feedback about level " + levelIndex;
        feedbackMenuCanvas.SetActive(false);

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
        Color textStartColor = levelText.color;
        Color textEndColor = new Color(textStartColor.r, textStartColor.g, textStartColor.b, 0);
        Color backgroundStartColor = backgroundImage.color;
        Color backgroundEndColor = new Color(backgroundStartColor.r, backgroundStartColor.g, backgroundStartColor.b, 0);
        
        while (elapsedTime < fadeTime)
        {
            levelText.color = Color.Lerp(textStartColor, textEndColor, elapsedTime / fadeTime);
            backgroundImage.color = Color.Lerp(backgroundStartColor, backgroundEndColor, elapsedTime / fadeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        levelText.gameObject.SetActive(false);
        backgroundImage.gameObject.SetActive(false);
        backgroundImage.color = _backgroundColor;
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

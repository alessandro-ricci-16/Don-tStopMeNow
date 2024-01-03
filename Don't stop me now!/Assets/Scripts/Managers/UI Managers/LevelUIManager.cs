using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelUIManager : MonoBehaviour
{
    public float fadeTime = 1f;
    public Image backgroundImage;
    
    [Header("Pause")]
    public GameObject pauseMenuCanvas;
    public TextMeshProUGUI levelText;
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
        pauseMenuCanvas.SetActive(false);

        int levelIndex = SceneManager.GetActiveScene().buildIndex - 2;
        levelText.text = "Level " + levelIndex;
        pauseLevelText.text = "Level " + levelIndex;
        feedbackTitleText.text = "Feedback about level " + levelIndex;
        feedbackMenuCanvas.SetActive(false);
        
        levelText.gameObject.SetActive(true);
        _backgroundColor = backgroundImage.color;
        backgroundImage.gameObject.SetActive(true);
        
        StartCoroutine(FadeLevelText());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
    }

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
        pauseMenuCanvas.SetActive(false);
        backgroundImage.gameObject.SetActive(false);
        Time.timeScale = 1;
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
        GameManager.Instance.LoadLevelSelectionScene();
    }
    
    public void SkipLevel()
    {
        Time.timeScale = 1;
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

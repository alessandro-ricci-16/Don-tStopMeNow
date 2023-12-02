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
    public GameObject pauseMenuCanvas;
    public Image backgroundImage;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI pauseLevelText;
    
    public float fadeTime = 1f;
    
    private Color backgroundColor;

    private void Start()
    {
        pauseMenuCanvas.SetActive(false);

        int levelIndex = SceneManager.GetActiveScene().buildIndex - 2;
        levelText.text = "Level " + levelIndex;
        pauseLevelText.text = "Level " + levelIndex;
        
        levelText.gameObject.SetActive(true);
        backgroundColor = backgroundImage.color;
        backgroundImage.gameObject.SetActive(true);
        
        StartCoroutine(FadeLevelText());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CallPause();
    }

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
        backgroundImage.color = backgroundColor;
    }

    public void CallPause()
    {
        Time.timeScale = 0;
        pauseMenuCanvas.SetActive(true);
        backgroundImage.color = backgroundColor;
        backgroundImage.gameObject.SetActive(true);
    }

    public void Resume()
    {
        pauseMenuCanvas.SetActive(false);
        backgroundImage.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void BackToLevelSelection()
    {
        Time.timeScale = 1;
        GameManager.Instance.LoadLevelSelectionScene();
    }

}

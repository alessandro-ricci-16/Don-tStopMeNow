using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelUIManager : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    public GameObject backgroundCanvas;

    private void Start()
    {
        pauseMenuCanvas.SetActive(false);
        backgroundCanvas.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CallPause();
    }

    public void CallPause()
    {
        Time.timeScale = 0;
        pauseMenuCanvas.SetActive(true);
        backgroundCanvas.SetActive(true);
    }

    public void Resume()
    {
        pauseMenuCanvas.SetActive(false);
        backgroundCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    public void BackToLevelSelection()
    {
        Time.timeScale = 1;
        GameManager.Instance.LoadLevelSelectionScene();
    }

}

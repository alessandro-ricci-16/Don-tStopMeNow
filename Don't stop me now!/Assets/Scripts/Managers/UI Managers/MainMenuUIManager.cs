using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    public Button PlayButton;

    private void Awake()
    {
        PlayButton.onClick.AddListener(LoadLevelSelectionScene);
    }

    private void LoadLevelSelectionScene()
    {
        GameManager.Instance.LoadLevelSelectionScene();
    }
}

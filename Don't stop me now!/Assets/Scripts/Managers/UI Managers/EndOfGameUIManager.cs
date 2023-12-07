using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfGameUIManager : MonoBehaviour
{
    public void LoadFeedbackScreen()
    {
        GameManager.Instance.LoadFeedbackScene();
    }
    
    public void LoadLevelSelectionScreen()
    {
        GameManager.Instance.LoadLevelSelectionScene();
    }
}

﻿using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class SendToGoogle : MonoBehaviour {
    
    [SerializeField] private TextMeshProUGUI FeedbackText;
    
    public void SendFeedback()
    {
        string feedback = FeedbackText.text;
        FeedbackText.text = "";
        
        if (string.IsNullOrEmpty(feedback) || string.IsNullOrWhiteSpace(feedback))
        {
            return;
        }
        
        Feedback feedbackObject = new Feedback("Feedback", "Feedback", feedback);
        FeedbackManager.Instance.SendFeedback(feedbackObject);
        
        if (FeedbackManager.Instance.feedbackType != "Development")
            StartCoroutine(PostFeedback(feedback));
    }

    public void GoBack()
    {
        GameManager.Instance.LoadLevelSelectionScene();
    }

    public void SendLevelFeedback()
    {
        if (string.IsNullOrEmpty(FeedbackText.text) || string.IsNullOrWhiteSpace(FeedbackText.text))
        {
            return;
        }
        
        int levelIndex = SceneManager.GetActiveScene().buildIndex - 2;
        string feedback = "Level " + levelIndex + ": " + FeedbackText.text;
        FeedbackText.text = "";
        
        Feedback feedbackObject = new Feedback(SceneManager.GetActiveScene().name, "Feedback", feedback);
        FeedbackManager.Instance.SendFeedback(feedbackObject);
        
        if (FeedbackManager.Instance.feedbackType != "Development")
            StartCoroutine(PostFeedback(feedback));
    }
    
    IEnumerator PostFeedback(string feedback) 
    {
        // https://docs.google.com/forms/d/e/1FAIpQLSdyQkpRLzqRzADYlLhlGJHwhbKZvKJILo6vGmMfSePJQqlZxA/viewform?usp=pp_url&entry.631493581=Simple+Game&entry.1313960569=Very%0AGood!

        string URL =
            "https://docs.google.com/forms/d/e/1FAIpQLSdyQkpRLzqRzADYlLhlGJHwhbKZvKJILo6vGmMfSePJQqlZxA/formResponse";
        
        WWWForm form = new WWWForm();

        form.AddField("entry.631493581", "DontStopMeNow!");
        form.AddField("entry.1313960569", feedback);

        UnityWebRequest www = UnityWebRequest.Post(URL, form);

        yield return www.SendWebRequest();

        print(www.error);
        
        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }
}
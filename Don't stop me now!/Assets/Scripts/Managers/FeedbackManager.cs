using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

// TODO: figure out a way to call levelStarted only on the first run of the level

public class FeedbackManager : Singleton<FeedbackManager>
{
    // TODO: change when deployed
    // change back to EXACTLY "Development" after exporting
    // (in deployment feedback is not sent to the google form)
    private string _feedbackType = "Development";

    private void Start()
    {
        EventManager.StartListening(EventNames.LevelPassed, LevelPassedFeedback);
        EventManager.StartListening(EventNames.CheckpointPassed, CheckpointPassedFeedback);
        EventManager.StartListening(EventNames.Death, DeathFeedback);
        EventManager.StartListening(EventNames.LevelStarted, LevelStartedFeedback);
    }

    public void SendFeedback(Feedback feedback)
    {
        if (_feedbackType != "Development")
            StartCoroutine(PostFeedback(feedback));
    }
    
    IEnumerator PostFeedback(Feedback feedback) 
    {
        string URL =
            "https://docs.google.com/forms/d/e/1FAIpQLSdU9YdFRnmF5euNHEk2HK1j5qgxsicIJNIWEnGo-AYxlQ8Ahw/formResponse";
        
        WWWForm form = new WWWForm();
        
        // LevelName
        form.AddField("entry.809409108", feedback.LevelName);
        // EventName
        form.AddField("entry.1707045091", feedback.EventName);
        // Parameter
        form.AddField("entry.1933450375", feedback.Parameter);
        // PlayerID
        form.AddField("entry.916745743", SystemInfo.deviceUniqueIdentifier);
        // feedback type
        form.AddField("entry.2049634247", _feedbackType);
        

        UnityWebRequest www = UnityWebRequest.Post(URL, form);

        yield return www.SendWebRequest();

        print(www.error);
        
        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }

    private void LevelPassedFeedback(string levelName)
    {
        Feedback feedback = new Feedback(levelName, "Level Passed", "");
        SendFeedback(feedback);
    }
    
    private void CheckpointPassedFeedback(string levelName, int checkpointIndex)
    {
        Feedback feedback = new Feedback(levelName, "Checkpoint Passed", checkpointIndex.ToString());
        SendFeedback(feedback);
    }
    
    private void DeathFeedback(string levelName)
    {
        Feedback feedback = new Feedback(levelName, "Death", "");
        SendFeedback(feedback);
    }
    
    private void LevelStartedFeedback(string levelName)
    {
        Feedback feedback = new Feedback(levelName, "Level Started", "");
        SendFeedback(feedback);
    }
}

public class Feedback
{
    public string LevelName;
    public string EventName;
    public string Parameter;
    
    public Feedback(string levelName, string eventName, string parameter)
    {
        this.LevelName = levelName;
        this.EventName = eventName;
        this.Parameter = parameter;
    }
}
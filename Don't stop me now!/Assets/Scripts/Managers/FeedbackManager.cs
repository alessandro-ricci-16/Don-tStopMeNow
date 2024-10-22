using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class FeedbackManager : Singleton<FeedbackManager>
{
    private readonly string _version = "1.0.4";
    
#if UNITY_EDITOR
    public readonly string feedbackType = "Development";
#elif UNITY_STANDALONE_WIN
    public readonly string FeedbackType = "Windows v" + _version;
#elif UNITY_STANDALONE_OSX
    public readonly string FeedbackType = "MacOS v" + _version;
#elif UNITY_STANDALONE_LINUX
    public readonly string FeedbackType = "Linux v" + _version;
#elif UNITY_WEBGL
    public readonly string FeedbackType = "WebGL v" + _version;
#endif

    private string _runID;

    private void Start()
    {
        _runID = GenerateRunID(20);
        
        EventManager.StartListening(EventNames.LevelPassed, LevelPassedFeedback);
        EventManager.StartListening(EventNames.CheckpointPassed, CheckpointPassedFeedback);
        EventManager.StartListening(EventNames.Death, DeathFeedback);
        EventManager.StartListening(EventNames.LevelStarted, LevelStartedFeedback);
    }

    public void SendFeedback(Feedback feedback)
    {
        if (feedbackType != "Development")
            StartCoroutine(PostFeedback(feedback));
    }
    
    public void SendFeedback(string levelName, string eventName, string parameter)
    {
        Feedback feedback = new Feedback(levelName, eventName, parameter);
        SendFeedback(feedback);
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
        // RunID
        form.AddField("entry.2041744485", _runID);
        // feedback type
        form.AddField("entry.2049634247", feedbackType);
        

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
    
    private void DeathFeedback(string levelName, Vector3 position)
    {
        Feedback feedback = new Feedback(levelName, "Death", position.ToString());
        SendFeedback(feedback);
    }
    
    private void LevelStartedFeedback(string levelName)
    {
        Feedback feedback = new Feedback(levelName, "Level Started", "");
        SendFeedback(feedback);
    }
    
    private string GenerateRunID(int length)
    {
        string runID = "";
        for (int i = 0; i < length; i++)
        {
            runID += UnityEngine.Random.Range(0, 10);
        }

        return runID;
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
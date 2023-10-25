using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


class IntUnityEvent : UnityEvent<int>
{
}

class FloatUnityEvent : UnityEvent<float>
{
}

class StringUnityEvent : UnityEvent<string>
{
}

public class EventManager : Singleton<EventManager>
{
    private Dictionary<string, UnityEvent> _simpleEventDictionary;
    private Dictionary<string, UnityEvent<int>> _intEventDictionary;
    private Dictionary<string, UnityEvent<float>> _floatEventDictionary;
    private Dictionary<string, UnityEvent<string>> _stringEventDictionary;


    private static EventManager _eventManager;
    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    void Init()
    {
        if (_simpleEventDictionary == null)
        {
            _simpleEventDictionary = new Dictionary<string, UnityEvent>();
        }

        if (_intEventDictionary == null)
        {
            _intEventDictionary = new Dictionary<string, UnityEvent<int>>();
        }

        if (_floatEventDictionary == null)
        {
            _floatEventDictionary = new Dictionary<string, UnityEvent<float>>();
        }

        if (_stringEventDictionary == null)
        {
            _stringEventDictionary = new Dictionary<string, UnityEvent<string>>();
        }
    }

    #region StartListening

    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (Instance._simpleEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Instance._simpleEventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StartListening(string eventName, UnityAction<int> listener)
    {
        UnityEvent<int> thisEvent = null;
        if (Instance._intEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new IntUnityEvent();
            thisEvent.AddListener(listener);
            Instance._intEventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StartListening(string eventName, UnityAction<float> listener)
    {
        UnityEvent<float> thisEvent = null;

        if (Instance._floatEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new FloatUnityEvent();
            thisEvent.AddListener(listener);
            Instance._floatEventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StartListening(string eventName, UnityAction<string> listener)
    {
        UnityEvent<string> thisEvent = null;

        if (Instance._stringEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new StringUnityEvent();
            thisEvent.AddListener(listener);
            Instance._stringEventDictionary.Add(eventName, thisEvent);
        }
    }

    #endregion

    #region StopListening

    public static void StopListening(string eventName, UnityAction listener)
    {
        if (_eventManager == null) return;
        UnityEvent thisEvent = null;
        if (Instance._simpleEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void StopListening(string eventName, UnityAction<int> listener)
    {
        if (_eventManager == null) return;
        UnityEvent<int> thisEvent = null;
        if (Instance._intEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void StopListening(string eventName, UnityAction<float> listener)
    {
        if (_eventManager == null) return;
        UnityEvent<float> thisEvent = null;
        if (Instance._floatEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void StopListening(string eventName, UnityAction<string> listener)
    {
        if (_eventManager == null) return;
        UnityEvent<string> thisEvent = null;
        if (Instance._stringEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    #endregion

    #region TriggerEvent

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (Instance._simpleEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }

    public static void TriggerEvent(string eventName, int value)
    {
        UnityEvent<int> thisEvent = null;
        if (Instance._intEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(value);
        }
    }

    public static void TriggerEvent(string eventName, float value)
    {
        UnityEvent<float> thisEvent = null;
        if (Instance._floatEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(value);
        }
    }

    public static void TriggerEvent(string eventName, string value)
    {
        UnityEvent<string> thisEvent = null;
        if (Instance._stringEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(value);
        }
    }

    #endregion
}
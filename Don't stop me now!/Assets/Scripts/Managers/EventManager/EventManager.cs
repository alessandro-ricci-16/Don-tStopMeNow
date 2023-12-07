using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Ice_Cube.States;

public class EventManager : Singleton<EventManager>
{
    private Dictionary<EventNames, UnityEvent> _simpleEventDictionary;
    private Dictionary<EventNames, UnityEvent<int>> _intEventDictionary;
    private Dictionary<EventNames, UnityEvent<float>> _floatEventDictionary;
    private Dictionary<EventNames, UnityEvent<string>> _stringEventDictionary;
    private Dictionary<EventNames, UnityEvent<Vector3>> _vector3EventDictionary;
    private Dictionary<EventNames, UnityEvent<bool>>_boolEventDictionary;
    private Dictionary<EventNames, UnityEvent<Vector3, Direction>> _vector3DirectionEventDictionary;
    private Dictionary<EventNames, UnityEvent<string, int>> _stringIntEventDictionary;
    private Dictionary<EventNames, UnityEvent<IceCubeStatesEnum,IceCubeStatesEnum>> _iceCubeStateEventDictionary;
    private Dictionary<EventNames, UnityEvent<string, Vector3>> _stringVector3EventDictionary;
    private static EventManager _eventManager;
    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    
    /// <summary>
    /// Initialize all dictionaries.
    /// </summary>
    void Init()
    {
        if (_simpleEventDictionary == null)
        {
            _simpleEventDictionary = new Dictionary<EventNames, UnityEvent>();
        }

        if (_intEventDictionary == null)
        {
            _intEventDictionary = new Dictionary<EventNames, UnityEvent<int>>();
        }

        if (_floatEventDictionary == null)
        {
            _floatEventDictionary = new Dictionary<EventNames, UnityEvent<float>>();
        }

        if (_stringEventDictionary == null)
        {
            _stringEventDictionary = new Dictionary<EventNames, UnityEvent<string>>();
        }
        
        if (_vector3EventDictionary == null)
        {
            _vector3EventDictionary = new Dictionary<EventNames, UnityEvent<Vector3>>();
        }
        if( _boolEventDictionary == null)
        {
            _boolEventDictionary = new Dictionary<EventNames, UnityEvent<bool>>();
        }

        if (_vector3DirectionEventDictionary == null)
        {
            _vector3DirectionEventDictionary = new Dictionary<EventNames, UnityEvent<Vector3, Direction>>();
        }

        if (_stringIntEventDictionary == null)
        {
            _stringIntEventDictionary = new Dictionary<EventNames, UnityEvent<string, int>>();
        }
        if (_iceCubeStateEventDictionary== null)
        {
            _iceCubeStateEventDictionary = new Dictionary<EventNames, UnityEvent<IceCubeStatesEnum, IceCubeStatesEnum>>();
        }
        
        if (_stringVector3EventDictionary == null)
        {
            _stringVector3EventDictionary = new Dictionary<EventNames, UnityEvent<string, Vector3>>();
        }
    }

    #region StartListening

    public static void StartListening(EventNames eventName, UnityAction listener)
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
    public static void StartListening(EventNames eventName, UnityAction<bool> listener)
    {
        UnityEvent<bool> thisEvent = null;
        if (Instance._boolEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent<bool>();
            thisEvent.AddListener(listener);
            Instance._boolEventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StartListening(EventNames eventName, UnityAction<int> listener)
    {
        UnityEvent<int> thisEvent = null;
        if (Instance._intEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent<int>();
            thisEvent.AddListener(listener);
            Instance._intEventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StartListening(EventNames eventName, UnityAction<float> listener)
    {
        UnityEvent<float> thisEvent = null;
        if (Instance._floatEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent<float>();
            thisEvent.AddListener(listener);
            Instance._floatEventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StartListening(EventNames eventName, UnityAction<string> listener)
    {
        UnityEvent<string> thisEvent = null;

        if (Instance._stringEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent<string>();
            thisEvent.AddListener(listener);
            Instance._stringEventDictionary.Add(eventName, thisEvent);
        }
    }
    
    public static void StartListening(EventNames eventName, UnityAction<string, int> listener)
    {
        UnityEvent<string, int> thisEvent = null;

        if (Instance._stringIntEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent<string, int>();
            thisEvent.AddListener(listener);
            Instance._stringIntEventDictionary.Add(eventName, thisEvent);
        }
    }
    
    public static void StartListening(EventNames eventName, UnityAction<Vector3> listener)
    {
        UnityEvent<Vector3> thisEvent = null;
        
        if (Instance._vector3EventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent<Vector3>();
            thisEvent.AddListener(listener);
            Instance._vector3EventDictionary.Add(eventName, thisEvent);
        }
    }
    
    public static void StartListening(EventNames eventName, UnityAction<Vector3, Direction> listener)
    {
        UnityEvent<Vector3, Direction> thisEvent = null;
        
        if (Instance._vector3DirectionEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent<Vector3, Direction>();
            thisEvent.AddListener(listener);
            Instance._vector3DirectionEventDictionary.Add(eventName, thisEvent);
        }
    }
    public static void StartListening(EventNames eventName, UnityAction<IceCubeStatesEnum, IceCubeStatesEnum> listener)
    {
        UnityEvent<IceCubeStatesEnum, IceCubeStatesEnum> thisEvent = null;
        
        if (Instance._iceCubeStateEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent<IceCubeStatesEnum, IceCubeStatesEnum>();
            thisEvent.AddListener(listener);
            Instance._iceCubeStateEventDictionary.Add(eventName, thisEvent);
        }
    }
    
    public static void StartListening(EventNames eventName, UnityAction<string, Vector3> listener)
    {
        UnityEvent<string, Vector3> thisEvent = null;
        
        if (Instance._stringVector3EventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent<string, Vector3>();
            thisEvent.AddListener(listener);
            Instance._stringVector3EventDictionary.Add(eventName, thisEvent);
        }
    }

    #endregion

    #region StopListening

    public static void StopListening(EventNames eventName, UnityAction listener)
    {
        if (_eventManager == null) return;
        UnityEvent thisEvent = null;
        if (Instance._simpleEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }
    public static void StopListening(EventNames eventName, UnityAction<bool> listener)
    {
        if (_eventManager == null) return;
        UnityEvent<bool> thisEvent = null;
        if (Instance._boolEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void StopListening(EventNames eventName, UnityAction<int> listener)
    {
        if (_eventManager == null) return;
        UnityEvent<int> thisEvent = null;
        if (Instance._intEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void StopListening(EventNames eventName, UnityAction<float> listener)
    {
        if (_eventManager == null) return;
        UnityEvent<float> thisEvent = null;
        if (Instance._floatEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void StopListening(EventNames eventName, UnityAction<string> listener)
    {
        if (_eventManager == null) return;
        UnityEvent<string> thisEvent = null;
        if (Instance._stringEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }
    
    public static void StopListening(EventNames eventName, UnityAction<string, int> listener)
    {
        if (_eventManager == null) return;
        UnityEvent<string, int> thisEvent = null;
        if (Instance._stringIntEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }
    
    public static void StopListening(EventNames eventName, UnityAction<Vector3> listener)
    {
        if (_eventManager == null) return;
        UnityEvent<Vector3> thisEvent = null;
        if (Instance._vector3EventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }
    
    public static void StopListening(EventNames eventName, UnityAction<Vector3, Direction> listener)
    {
        if (_eventManager == null) return;
        UnityEvent<Vector3, Direction> thisEvent = null;
        if (Instance._vector3DirectionEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }
    public static void StopListening(EventNames eventName, UnityAction<IceCubeStatesEnum, IceCubeStatesEnum> listener)
    {
        if (_eventManager == null) return;
        UnityEvent<IceCubeStatesEnum, IceCubeStatesEnum> thisEvent = null;
        if (Instance._iceCubeStateEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }
    
    public static void StopListening(EventNames eventName, UnityAction<string, Vector3> listener)
    {
        if (_eventManager == null) return;
        UnityEvent<string, Vector3> thisEvent = null;
        if (Instance._stringVector3EventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }
    
    
    #endregion

    #region TriggerEvent

    public static void TriggerEvent(EventNames eventName)
    {
        UnityEvent thisEvent = null;
        if (Instance._simpleEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
    public static void TriggerEvent(EventNames eventName, bool value)
    {
        UnityEvent<bool> thisEvent = null;
        if (Instance._boolEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(value);
        }
    }
    public static void TriggerEvent(EventNames eventName, int value)
    {
        UnityEvent<int> thisEvent = null;
        if (Instance._intEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(value);
        }
    }

    public static void TriggerEvent(EventNames eventName, float value)
    {
        UnityEvent<float> thisEvent = null;
        if (Instance._floatEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(value);
        }
    }

    public static void TriggerEvent(EventNames eventName, string value)
    {
        UnityEvent<string> thisEvent = null;
        if (Instance._stringEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(value);
        }
    }
    
    public static void TriggerEvent(EventNames eventName, string value, int number)
    {
        UnityEvent<string, int> thisEvent = null;
        if (Instance._stringIntEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(value, number);
        }
    }
    
    public static void TriggerEvent(EventNames eventName, Vector3 value)
    {
        UnityEvent<Vector3> thisEvent = null;
        if (Instance._vector3EventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(value);
        }
    }
    
    public static void TriggerEvent(EventNames eventName, Vector3 vector, Direction direction)
    {
        UnityEvent<Vector3, Direction> thisEvent = null;
        if (Instance._vector3DirectionEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(vector, direction);
        }
    }
    public static void TriggerEvent(EventNames eventName, IceCubeStatesEnum currentState, IceCubeStatesEnum nextState)
    {
        UnityEvent<IceCubeStatesEnum, IceCubeStatesEnum> thisEvent = null;
        if (Instance._iceCubeStateEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(currentState, nextState);
        }
    }
    
    public static void TriggerEvent(EventNames eventName, string value, Vector3 vector)
    {
        UnityEvent<string, Vector3> thisEvent = null;
        if (Instance._stringVector3EventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(value, vector);
        }
    }

    #endregion
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// Standard event manager for the pub/sub design pattern
public class EventManager
{
    private static EventManager instance;
    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventManager();

                instance.Init();
            }
            return instance;
        }
    }

    private Dictionary<string, Action> eventDictionary;

    private EventManager() { }

    private void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, Action>();
        }
    }

    public void StartListening(string eventName, Action listener)
    {

        Action thisListener = null;

        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] += listener;
        }
        else
        {

            thisListener += listener;
            Instance.eventDictionary.Add(eventName, thisListener);
        }
    }

    public void StopListening(string eventName, Action listener)
    {
        if (instance == null)
        {
            return;
        }

        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] -= listener;
        }
    }

    public void TriggerEvent(string eventName)
    {
        if (Instance.eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName]?.Invoke();
        }
    }
}

// WITH ONE PARAMENTERS
public class EventManagerOneParam<T>
{
    private static EventManagerOneParam<T> instance;
    public static EventManagerOneParam<T> Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventManagerOneParam<T>();

                instance.Init();
            }
            return instance;
        }
    }

    private Dictionary<string, Action<T>> paramEventDictionary;

    private EventManagerOneParam() { }

    private void Init()
    {
        if (paramEventDictionary == null)
        {
            paramEventDictionary = new Dictionary<string, Action<T>>();
        }
    }

    public void StartListening(string eventName, Action<T> listener)
    {
        Action<T> thisListener = null;

        if (paramEventDictionary.ContainsKey(eventName))
        {
            paramEventDictionary[eventName] += listener;

        }
        else
        {
            thisListener += listener;
            paramEventDictionary.Add(eventName, thisListener);
        }
    }

    public void StopListening(string eventName, Action<T> listener)
    {
        if (instance == null)
        {
            return;
        }

        if (paramEventDictionary.ContainsKey(eventName))
        {
            paramEventDictionary[eventName] -= listener;
        }
    }

    public void TriggerEvent(string eventName, T data)
    {
        if (paramEventDictionary.ContainsKey(eventName))
        {
            paramEventDictionary[eventName]?.Invoke(data);
        }
    }
}

// WITH TWO PARAMETERS
public class EventManagerTwoParams<T1, T2>
{
    private static EventManagerTwoParams<T1, T2> instance;
    public static EventManagerTwoParams<T1, T2> Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventManagerTwoParams<T1, T2>();

                instance.Init();
            }
            return instance;
        }
    }

    private Dictionary<string, Action<T1, T2>> paramEventDictionary;

    private EventManagerTwoParams() { }

    private void Init()
    {
        if (paramEventDictionary == null)
        {
            paramEventDictionary = new Dictionary<string, Action<T1, T2>>();
        }
    }

    public void StartListening(string eventName, Action<T1, T2> listener)
    {
        Action <T1, T2> thisListener = null;

        if (paramEventDictionary.ContainsKey(eventName))
        {
            paramEventDictionary[eventName] += listener;

        }
        else
        {
            thisListener += listener;
            paramEventDictionary.Add(eventName, thisListener);
        }
    }

    public void StopListening(string eventName, Action<T1, T2> listener)
    {
        if (instance == null)
        {
            return;
        }

        if (paramEventDictionary.ContainsKey(eventName))
        {
            paramEventDictionary[eventName] -= listener;
        }
    }

    public void TriggerEvent(string eventName, T1 dataOne, T2 dataTwo)
    {
        if (paramEventDictionary.ContainsKey(eventName))
        {
            paramEventDictionary[eventName]?.Invoke(dataOne, dataTwo);
        }
    }
}


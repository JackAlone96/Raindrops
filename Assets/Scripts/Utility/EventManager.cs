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
public class EventManagerTwoParams<T, A>
{
    private static EventManagerTwoParams<T, A> instance;
    public static EventManagerTwoParams<T, A> Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventManagerTwoParams<T, A>();

                instance.Init();
            }
            return instance;
        }
    }

    private Dictionary<string, Action<T, A>> paramEventDictionary;

    private EventManagerTwoParams() { }

    private void Init()
    {
        if (paramEventDictionary == null)
        {
            paramEventDictionary = new Dictionary<string, Action<T, A>>();
        }
    }

    public void StartListening(string eventName, Action<T, A> listener)
    {
        Action<T, A> thisListener = null;

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

    public void StopListening(string eventName, Action<T, A> listener)
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

    public void TriggerEvent(string eventName, T dataOne, A dataTwo)
    {
        if (paramEventDictionary.ContainsKey(eventName))
        {
            paramEventDictionary[eventName]?.Invoke(dataOne, dataTwo);
        }
    }
}


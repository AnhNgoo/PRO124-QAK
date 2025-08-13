using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : Singleton<GameEvent>
{
    private Dictionary<string, System.Action> eventDictionary = new Dictionary<string, System.Action>();

    public void RegisterEvent(string eventName, System.Action action)
    {
        if (!eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = action;
        }
        else
        {
            eventDictionary[eventName] += action;
        }
    }

    public void UnregisterEvent(string eventName, System.Action action)
    {
        if (!eventDictionary.ContainsKey(eventName))
        {
            Debug.LogWarning($"Event '{eventName}' not found.");
            return;
        }

        eventDictionary[eventName] -= action;
        if (eventDictionary[eventName] == null)
            eventDictionary.Remove(eventName);
    }

    public void TriggerEvent(string eventName)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName]?.Invoke();
        }
    }
}


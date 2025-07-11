using UnityEngine;
using UnityEngine.Events;

public class OnEnableEvent : MonoBehaviour
{
    public UnityEvent onEnabled;

    void OnEnable()
    {
        if (onEnabled != null)
            onEnabled.Invoke();
    }
}
using UnityEngine;
using UnityEngine.Events;

public class OnEnableEvent : MonoBehaviour
{
    public UnityEvent onEnabled;

    //LoadShop mỗi khi bật
    void OnEnable()
    {
        if (onEnabled != null)
            onEnabled.Invoke();
    }
}
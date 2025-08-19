using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{     
    public static T Instance { get;  set; }

    private void Awake()
    {   
        Instance = this as T;
    }
}

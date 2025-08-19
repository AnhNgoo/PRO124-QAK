using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTracker : Singleton<DistanceTracker>
{
    public float distanceTraveled { get; set; }
    public bool isStopped { get; set; } = false;
    private void Start()
    {
        GameEvent.Instance.RegisterEvent("PlayerDeath", StopTracking);
    }
    private void Update()
    {
        if (CutSceneBlocker.Instance.isCutSceneActive || isStopped) return;

        distanceTraveled += MapSpawner.Instance.scrollSpeed * Time.deltaTime;
    }

    public void ResetDistance()
    {
        distanceTraveled = 0f;
    }

    public void StopTracking()
    {
        isStopped = true;
    }


}

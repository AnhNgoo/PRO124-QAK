using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTracker : Singleton<DistanceTracker>
{
    public float distanceTraveled { get; set; }

    private void Update()
    {
        if (CutSceneBlocker.Instance.isCutSceneActive) return;

        distanceTraveled += MapSpawner.Instance.scrollSpeed * Time.deltaTime;
    }

    public void ResetDistance()
    {
        distanceTraveled = 0f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneBlocker : Singleton<CutSceneBlocker>
{
    public GameObject CanvasBlocker;

    public bool isCutSceneActive { get; set; } = false;

    private void Update()
    {
        Block();
    }
    private void Block()
    {
        if (isCutSceneActive)
        {
            CanvasBlocker.SetActive(true);
        }
        else
        {
            CanvasBlocker.SetActive(false);
        }
    }
}

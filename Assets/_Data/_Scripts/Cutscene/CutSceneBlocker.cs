using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneBlocker : Singleton<CutSceneBlocker>
{
    public GameObject CanvasBlocker; //Chặn người chơi nhấn vào màn hình

    public bool isCutSceneActive { get; set; } = false; //Chặn người chơi nhấn phím

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

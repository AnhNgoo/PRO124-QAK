using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGameObjectInPVP : MonoBehaviour
{
    public enum ModeAction { Enable, Disable }

    [Header("Chọn hành động cho từng chế độ")]
    public ModeAction normalModeAction = ModeAction.Disable;
    public ModeAction pvpModeAction = ModeAction.Enable;

    private void Awake()
    {
        GameEvent.Instance.RegisterEvent("GameMode", ApplyAction);
    }

    // private void OnDisable()
    // {
    //     GameEvent.Instance.UnregisterEvent("GameMode", ApplyAction);
    // }

    private void ApplyAction()
    {
        Debug.Log($"Current Game Mode: {GameManager.Instance.gameMode}");
        switch (GameManager.Instance.gameMode)
        {
            case GameManager.GameMode.Normal:
                Apply(normalModeAction);
                break;
            case GameManager.GameMode.PVP:
                Apply(pvpModeAction);
                break;
        }
    }

    private void Apply(ModeAction action)
    {
        switch (action)
        {
            case ModeAction.Enable:
                gameObject.SetActive(true);
                break;
            case ModeAction.Disable:
                gameObject.SetActive(false);
                break;
        }
    }
}

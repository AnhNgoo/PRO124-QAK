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

    //Kiểm tra chế độ game hiện tại
    private void ApplyAction()
    {
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

    //Áp dụng tắt/bật gameobject tuỳ theo chế độ
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

//Ẩn hiện các gameobject mong muốn theo từng chế độ
//Ví dụ normalModeAction = ModeAction.Disable;  // Tắt gameobject trong chế độ Normal
//      pvpModeAction = ModeAction.Enable;  // Bật gameobject trong chế độ PVP
//Khi ta chơi ở chế độ PVP, gameobject sẽ được bật
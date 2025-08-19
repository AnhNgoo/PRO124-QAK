using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PauseUI : MainUI
{
    // Ghi đè Start() để không phát nhạc MainTheme khi mở pause panel
    void Start()
    {
        // Không làm gì cả - không phát nhạc MainTheme như MainUI
        // Giữ nguyên nhạc đang phát (InGame)
    }

    protected override void HideThisPanel(System.Action onHidden = null)
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => UIManager.Instance.pausePanelDQ.DOPlayBackwards());
        seq.AppendInterval(0.4f);
        seq.AppendCallback(() => UIManager.Instance.PausePanelGameobject.SetActive(false));
        if (onHidden != null)
            seq.AppendCallback(() => onHidden());

        seq.SetUpdate(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1; // Tiếp tục thời gian khi đóng Pause Panel
        // Chỉ phát SFX button press, không đổi nhạc
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("ButtonPress");
        }

        HideThisPanel(() => UIManager.Instance.InGamePanelGameobject.SetActive(true));

    }

    public void _OpenSettingPanel()
    {
        // Chỉ phát SFX button press, không đổi nhạc
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("ButtonPress");
        }

        HideThisPanel(() => UIManager.Instance.SettingPanelGameobject.SetActive(true));
        Time.timeScale = 0;
    }

    public void Home()
    {
        Time.timeScale = 1; // Tiếp tục thời gian khi về Home
        if (GameManager.Instance.sessionState == GameManager.SessionState.InProgress)
        {
            GameManager.Instance.UpdateProperties();
            GameManager.Instance.sessionState = GameManager.SessionState.Finished; // Đánh dấu phiên chơi đã kết thúc
        }


        // Phát SFX button press
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("ButtonPress");
        }

        HideThisPanel(() => SceneLoader.Instance.ReloadSceneWithLoading());
    }
}

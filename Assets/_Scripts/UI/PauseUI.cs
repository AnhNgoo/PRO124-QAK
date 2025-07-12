using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PauseUI : MainUI
{
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
        HideThisPanel(() => UIManager.Instance.InGamePanelGameobject.SetActive(true));
        Time.timeScale = 1; // Tiếp tục thời gian khi đóng Pause Panel
    }

    public void _OpenSettingPanel()
    {
        HideThisPanel(() => UIManager.Instance.SettingPanelGameobject.SetActive(true));
        Time.timeScale = 0;
    }

    public void Home()
    {
        HideThisPanel(() => SceneLoader.Instance.ReloadSceneWithLoading());
        Time.timeScale = 1; // Tiếp tục thời gian khi về Home
    }
}

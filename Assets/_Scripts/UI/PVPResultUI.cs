using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PVPResultUI : MainUI
{
    protected override void HideThisPanel(System.Action onHidden = null)
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => UIManager.Instance.pvpResultPanelDQ.DOPlayBackwards());
        seq.AppendInterval(0.4f);
        seq.AppendCallback(() => UIManager.Instance.PVPResultPanelGameobject.SetActive(false));
        if (onHidden != null)
            seq.AppendCallback(() => onHidden());

        seq.SetUpdate(true);
    }

    public void ReplayGame()
    {
        HideThisPanel(() => SceneLoader.Instance.ReloadSceneWithLoading(true));
        Time.timeScale = 1; // Tiếp tục thời gian khi chơi lại
    }

    public void Home()
    {
        HideThisPanel(() => SceneLoader.Instance.ReloadSceneWithLoading());
        Time.timeScale = 1; // Tiếp tục thời gian khi về Home
    }
}
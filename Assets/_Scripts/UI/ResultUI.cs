using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ResultUI : MainUI
{
    protected override void HideThisPanel(System.Action onHidden = null)
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => UIManager.Instance.resultPanelDQ.DOPlayBackwards());
        seq.AppendInterval(0.4f);
        seq.AppendCallback(() => UIManager.Instance.ResultPanelGameobject.SetActive(false));
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

    public void OpenShop()
    {
        HideThisPanel(() => UIManager.Instance.ShopPanelGameobject.SetActive(true));
        Time.timeScale = 0; // Dừng thời gian khi mở Shop Panel
    }
}
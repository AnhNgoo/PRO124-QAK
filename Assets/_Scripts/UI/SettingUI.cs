using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SettingUI : MainUI
{
    protected override void HideThisPanel(Action onHidden = null)
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => UIManager.Instance.settingPanelDQ.DOPlayBackwards());
        seq.AppendInterval(0.4f);
        seq.AppendCallback(() => UIManager.Instance.SettingPanelGameobject.SetActive(false));
        if (onHidden != null)
            seq.AppendCallback(() => onHidden());
        seq.SetUpdate(true); // Quan trọng: Set update mode để ignore timeScale
    }

    public void Back()
    {
        // Phát SFX button press
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("ButtonPress");
        }
        
        if (UIManager.Instance.statePanel == UIManager.StatePanel.Pause)
        {
            HideThisPanel(() => UIManager.Instance.PausePanelGameobject.SetActive(true));
            Time.timeScale = 0;
        }
        else if (UIManager.Instance.statePanel == UIManager.StatePanel.Main)
        {
            HideThisPanel(() => UIManager.Instance.MainPanelGameobject.SetActive(true));
        }

    }
}

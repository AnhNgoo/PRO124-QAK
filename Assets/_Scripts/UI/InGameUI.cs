using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MainUI
{
    protected override void HideThisPanel(System.Action onHidden = null)
    {
        UIManager.Instance.InGamePanelGameobject.SetActive(false);
        if (onHidden != null)
            onHidden();
    }

    public void OpenPausePanel()
    {
        HideThisPanel(() => UIManager.Instance.PausePanelGameobject.SetActive(true));
        Time.timeScale = 0; // Dừng thời gian khi mở Pause Panel
        UIManager.Instance.statePanel = UIManager.StatePanel.Pause; // Cập nhật trạng thái panel
    }
}

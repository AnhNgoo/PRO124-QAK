using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InGameUI : MainUI
{

    private void Start()
    {
        GameEvent.Instance.RegisterEvent("PlayerDeath", OpenResultPanelByMode);
    }


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


    private void OpenResultPanelByMode()
    {
        if (GameManager.Instance.playerTotal == 1)
        {
            OpenResultPanel();
        }
        else
        {
            OpenResultPanelForPVP();
        }
    }
    //Mở Result Panel khi người chơi chết
    private void OpenResultPanel()
    {
        // Update properties ngay lập tức
        if (GameManager.Instance.sessionState == GameManager.SessionState.InProgress)
        {
            GameManager.Instance.UpdateProperties();
            GameManager.Instance.sessionState = GameManager.SessionState.Finished; // Đánh dấu phiên chơi đã kết thúc
            SaveManager.Instance.Save(); // Lưu dữ liệu ngay khi người chơi chết
        }


        // Delay 2s rồi hiện ResultPanel
        DOVirtual.DelayedCall(2f, () =>
        {
            HideThisPanel(() =>
            {
                UIManager.Instance.ResultPanelGameobject.SetActive(true);
                Time.timeScale = 1; // Consistent với PausePanel
                UIManager.Instance.statePanel = UIManager.StatePanel.Result;
            });
        });
    }

    private void OpenResultPanelForPVP()
    {
        // Delay 2s rồi hiện PVPResultPanel
        DOVirtual.DelayedCall(2f, () =>
        {
            HideThisPanel(() =>
            {
                UIManager.Instance.PVPResultPanelGameobject.SetActive(true);
                Time.timeScale = 1; // Consistent với PausePanel
            });
        });
    }
}

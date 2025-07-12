using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InGameUI : MainUI
{

    private PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerController.playerDeath.deathEvent += OpenResultPanel;
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

    //Mở Result Panel khi người chơi chết
    private void OpenResultPanel()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(2);
        seq.AppendCallback(() => HideThisPanel(() =>
        {
            UIManager.Instance.ResultPanelGameobject.SetActive(true);
            Time.timeScale = 0;
            UIManager.Instance.statePanel = UIManager.StatePanel.Result; // Cập nhật trạng thái panel
        }));
        seq.SetUpdate(true);

    }
}

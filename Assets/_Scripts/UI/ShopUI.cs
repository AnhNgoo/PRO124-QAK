using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShopUI : MainUI
{
    protected override void HideThisPanel(System.Action onHidden = null)
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => UIManager.Instance.menuShopDQ.DOPlayBackwards());
        seq.AppendCallback(() => UIManager.Instance.showItemPanelDQ.DOPlayBackwards());
        seq.AppendCallback(() => UIManager.Instance.itemStatusDQ.DOPlayBackwards());
        seq.AppendInterval(0.4f);
        seq.AppendCallback(() => UIManager.Instance.ShopPanelGameobject.SetActive(false));
        if (onHidden != null)
            seq.AppendCallback(() => onHidden());
        seq.SetUpdate(true);
    }

    public void Back()
    {
        if (UIManager.Instance.statePanel == UIManager.StatePanel.Main)
            HideThisPanel(() => UIManager.Instance.MainPanelGameobject.SetActive(true));
        else if (UIManager.Instance.statePanel == UIManager.StatePanel.Result)
            HideThisPanel(() => UIManager.Instance.ResultPanelGameobject.SetActive(true));
    }
}

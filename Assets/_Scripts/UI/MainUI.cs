using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainUI : MonoBehaviour
{
    // Truyền thêm callback để làm gì đó sau khi ẩn panel xong (ví dụ mở Shop, Setting, hay chạy Cutscene)
    protected virtual void HideThisPanel(System.Action onHidden = null)
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => UIManager.Instance.quitButtonDQ.DOPlayBackwards());
        seq.AppendInterval(0.2f);
        seq.AppendCallback(() => UIManager.Instance.settingButtonDQ.DOPlayBackwards());
        seq.AppendInterval(0.2f);
        seq.AppendCallback(() => UIManager.Instance.shopButtonDQ.DOPlayBackwards());
        seq.AppendInterval(0.2f);
        seq.AppendCallback(() => UIManager.Instance.playButtonDQ.DOPlayBackwards());
        seq.AppendInterval(0.4f);
        seq.AppendCallback(() => UIManager.Instance.MainPanelGameobject.SetActive(false));
        if (onHidden != null)
            seq.AppendCallback(() => onHidden());
    }

    public void PlayGame()
    {
        HideThisPanel(() => StartGameCutScene.Instance.StartCutScene());
    }

    public void OpenShopPanel()
    {
        HideThisPanel(() => UIManager.Instance.ShopPanelGameobject.SetActive(true));
    }

    public void OpenSettingPanel()
    {
        HideThisPanel(() => UIManager.Instance.SettingPanelGameobject.SetActive(true));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
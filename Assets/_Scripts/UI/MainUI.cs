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

    void Start()
    {
        // Phát nhạc chủ đề khi ở Main Panel
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic("MainTheme");
        }
        
        // Phát SFX khi mở game
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("OpenGame");
        }
    }

    public void PlayGame()
    {
        // Phát SFX button press
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("ButtonPress");
        }
        
        // Dừng nhạc chủ đề và phát nhạc trong game
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayMusic("InGame");
        }
        
        HideThisPanel(() => StartGameCutScene.Instance.StartCutScene());
    }

    public void OpenShopPanel()
    {
        // Phát SFX button press
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("ButtonPress");
        }
        
        HideThisPanel(() => UIManager.Instance.ShopPanelGameobject.SetActive(true));
    }

    public void OpenSettingPanel()
    {
        // Phát SFX button press
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("ButtonPress");
        }
        
        HideThisPanel(() => UIManager.Instance.SettingPanelGameobject.SetActive(true));
    }

    public void QuitGame()
    {
        // Phát SFX close game và đợi âm thanh kết thúc
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("CloseGame");
            // Đợi âm thanh kết thúc trước khi quit
            StartCoroutine(QuitAfterSound());
        }
        else
        {
            // Nếu không có AudioManager thì quit luôn
            DoQuit();
        }
    }
    
    private IEnumerator QuitAfterSound()
    {
        // Đợi 2 giây (hoặc thời gian âm thanh CloseGame)
        yield return new WaitForSeconds(2f);
        
        DoQuit();
    }
    
    private void DoQuit()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
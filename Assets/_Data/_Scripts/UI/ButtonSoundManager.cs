using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundManager : MonoBehaviour
{
    void Start()
    {
        // Tự động thêm âm thanh cho tất cả button trong scene
        AddSoundToAllButtons();
    }
    
    void AddSoundToAllButtons()
    {
        // Tìm tất cả button trong scene
        Button[] buttons = FindObjectsOfType<Button>();
        
        foreach (Button button in buttons)
        {
            // Thêm listener cho mỗi button
            button.onClick.AddListener(() => PlayButtonSound());
        }
    }
    
    void PlayButtonSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("ButtonPress");
        }
    }
}

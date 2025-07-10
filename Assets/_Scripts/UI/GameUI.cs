using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject MainPanel;
    public GameObject SettingPanel;
    public GameObject PausePanel;
    
    [Header("Game UI Elements")]
    public GameObject PauseButton; // Nút pause khi đang chơi
    
    [Header("Setting UI Elements")]
    public Toggle musicToggle;
    public Toggle sfxToggle;
    
    private bool isSettingPanelOpen = false;
    private bool isPauseState = false; 
    
    // Start is called before the first frame update
    void Start()
    {
        InitializeSettingPanel();
        InitializePauseSystem();
        SetupUIEvents();
    }

    // Update is called once per frame
    void Update()
    {
        // Kiểm tra phím ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Ưu tiên đóng setting panel trước
            if (isSettingPanelOpen)
            {
                CloseSettingPanel();
            }
            // Sau đó mới xử lý resume game nếu đang pause
            else if (isPauseState)
            {
                ResumeGame();
            }
        }
    }
    
    // Khởi tạo setting panel
    private void InitializeSettingPanel()
    {
        if (SettingPanel != null)
        {
            SettingPanel.SetActive(false);
            isSettingPanelOpen = false;
        }
        
        // Khởi tạo giá trị từ AudioManager
        if (AudioManager.Instance != null)
        {
            if (musicToggle != null)
                musicToggle.isOn = AudioManager.Instance.isMusicEnabled;
            if (sfxToggle != null)
                sfxToggle.isOn = AudioManager.Instance.isSFXEnabled;
        }
    }
    
    // Khởi tạo pause system
    private void InitializePauseSystem()
    {
        // Ẩn pause panel khi khởi tạo
        if (PausePanel != null)
        {
            PausePanel.SetActive(false);
            isPauseState = false;
        }
        
        // Ẩn nút pause khi chưa bắt đầu game
        if (PauseButton != null)
        {
            PauseButton.SetActive(false);
        }
        
        // Đảm bảo time scale về bình thường
        Time.timeScale = 1f;
    }
    
    // Thiết lập events cho UI
    private void SetupUIEvents()
    {
        // Setup toggle events
        if (musicToggle != null)
            musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        if (sfxToggle != null)
            sfxToggle.onValueChanged.AddListener(OnSFXToggleChanged);
    }
    
    // Mở setting panel
    public void OpenSettingPanel()
    {
        if (SettingPanel != null)
        {
            // Nếu đang trong trạng thái pause, ẩn pause panel
            if (isPauseState && PausePanel != null)
            {
                PausePanel.SetActive(false);
            }
            
            SettingPanel.SetActive(true);
            isSettingPanelOpen = true;
            
            // Phát âm thanh click
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX("ButtonClick");
        }
    }
    
    // Đóng setting panel
    public void CloseSettingPanel()
    {
        if (SettingPanel != null)
        {
            SettingPanel.SetActive(false);
            isSettingPanelOpen = false;
            
            // Nếu đang trong trạng thái pause, hiện lại pause panel
            if (isPauseState && PausePanel != null)
            {
                PausePanel.SetActive(true);
            }
            
            // Phát âm thanh click
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX("ButtonClick");
        }
    }
    
    // Toggle setting panel (cho nút setting)
    public void ToggleSettingPanel()
    {
        if (isSettingPanelOpen)
        {
            CloseSettingPanel();
        }
        else
        {
            OpenSettingPanel();
        }
    }
    
    // Events cho setting controls
    private void OnMusicToggleChanged(bool value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ToggleMusic(value);
            AudioManager.Instance.PlaySFX("ButtonClick");
        }
    }
    
    private void OnSFXToggleChanged(bool value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ToggleSFX(value);
            // Chỉ phát âm thanh nếu SFX được bật
            if (value)
                AudioManager.Instance.PlaySFX("ButtonClick");
        }
    }
    
    // === PAUSE GAME FUNCTIONS ===
    
    // Hiển thị nút pause khi game bắt đầu
    public void ShowPauseButton()
    {
        if (PauseButton != null)
        {
            PauseButton.SetActive(true);
        }
    }
    
    // Ẩn nút pause
    public void HidePauseButton()
    {
        if (PauseButton != null)
        {
            PauseButton.SetActive(false);
        }
    }
    
    // Pause game - mở pause panel
    public void PauseGame()
    {
        if (PausePanel != null)
        {
            PausePanel.SetActive(true);
            isPauseState = true;
            
            // Dừng thời gian trong game
            Time.timeScale = 0f;
            
            // Phát âm thanh pause
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX("ButtonClick");
        }
    }
    
    // Resume game - đóng pause panel
    public void ResumeGame()
    {
        if (PausePanel != null)
        {
            PausePanel.SetActive(false);
            isPauseState = false;
            
            // Tiếp tục thời gian trong game
            Time.timeScale = 1f;
            
            // Phát âm thanh resume
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX("ButtonClick");
        }
    }
    
    // Home - quay về main panel và reset game
    public void GoHome()
    {
        // Đóng pause panel
        if (PausePanel != null)
        {
            PausePanel.SetActive(false);
            isPauseState = false;
        }
        
        // Ẩn nút pause
        HidePauseButton();
        
        // Hiện lại main panel
        if (MainPanel != null)
        {
            MainPanel.SetActive(true);
        }
        
        // Reset thời gian về bình thường
        Time.timeScale = 1f;
        
        // Phát âm thanh click
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("ButtonClick");
        
        // Reset game về trạng thái ban đầu (có thể thêm logic reset khác ở đây)
        ResetGameToInitialState();
    }
    
    // Reset game về trạng thái ban đầu
    private void ResetGameToInitialState()
    {
        // Reset các biến trạng thái
        isPauseState = false;
        
        // Có thể thêm logic reset khác như:
        // - Reset điểm số
        // - Reset vị trí player
        // - Reset enemy
        // - Reset timer
        
        Debug.Log("Game reset to initial state");
    }
    
    // Kiểm tra trạng thái pause
    public bool IsGamePaused()
    {
        return isPauseState;
    }
    
    // Method cho nút Start Game
    public void OnClickStartGame()
    {
        // Ẩn MainPanel trước khi bắt đầu cutscene
        if (MainPanel != null)
        {
            MainPanel.SetActive(false);
        }
        
        // Hiển thị nút pause khi game bắt đầu
        ShowPauseButton();
        
        // Phát âm thanh
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("GameStart");
        
        // Gọi trực tiếp qua Instance
        StartGameCutScene.Instance.StartCutScene();
    }
}
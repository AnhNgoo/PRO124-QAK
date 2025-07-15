using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioClipData
{
    public string name;
    public AudioClip clip;
    public bool isMusic = false;
    [Range(0f, 1f)]
    public float volume = 1f;
}

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    
    [Header("Audio Clips")]
    public AudioClipData[] audioClips;
    
    [Header("Settings")]
    [Range(0f, 1f)]
    public float musicVolume = 1f;
    [Range(0f, 1f)]
    public float sfxVolume = 1f;
    
    private Dictionary<string, AudioClipData> audioDict = new Dictionary<string, AudioClipData>();
    
    void Start()
    {
        InitializeAudioDictionary();
        LoadAudioSettings();
        UpdateAudioVolumes();
        SetupSliders();
        
        // Đảm bảo có tất cả âm thanh cần thiết
        ValidateAudioClips();
    }
    
    void Update()
    {
        // Đồng bộ hóa với UIManager sliders mỗi frame (tùy chọn)
        // Uncomment nếu muốn sync liên tục
        // SyncWithUIManager();
    }
    
    private void SetupSliders()
    {
        // Thiết lập slider từ UIManager
        if (UIManager.Instance != null)
        {
            if (UIManager.Instance.musicSlider != null)
            {
                // Đặt giá trị từ AudioManager lên slider
                UIManager.Instance.musicSlider.value = musicVolume;
                // Thêm listener để cập nhật AudioManager khi slider thay đổi
                UIManager.Instance.musicSlider.onValueChanged.AddListener(SetMusicVolume);
            }
            
            if (UIManager.Instance.sfxSlider != null)
            {
                // Đặt giá trị từ AudioManager lên slider
                UIManager.Instance.sfxSlider.value = sfxVolume;
                // Thêm listener để cập nhật AudioManager khi slider thay đổi
                UIManager.Instance.sfxSlider.onValueChanged.AddListener(SetSFXVolume);
            }
        }
        else
        {
            Debug.LogWarning("UIManager instance not found! Audio sliders won't be connected.");
        }
    }
    
    private void InitializeAudioDictionary()
    {
        foreach (AudioClipData audioData in audioClips)
        {
            if (!audioDict.ContainsKey(audioData.name))
            {
                audioDict.Add(audioData.name, audioData);
            }
        }
    }
    
    // Phát âm thanh SFX
    public void PlaySFX(string clipName)
    {
        if (sfxVolume <= 0) return;
        
        if (audioDict.ContainsKey(clipName))
        {
            AudioClipData data = audioDict[clipName];
            if (!data.isMusic)
            {
                sfxSource.PlayOneShot(data.clip, data.volume * sfxVolume);
            }
        }
        else
        {
            Debug.LogWarning($"Audio clip '{clipName}' not found!");
        }
    }
    
    // Phát nhạc nền
    public void PlayMusic(string clipName, bool loop = true)
    {
        if (musicVolume <= 0) return;
        
        if (audioDict.ContainsKey(clipName))
        {
            AudioClipData data = audioDict[clipName];
            if (data.isMusic)
            {
                musicSource.clip = data.clip;
                musicSource.volume = data.volume * musicVolume;
                musicSource.loop = loop;
                musicSource.Play();
            }
        }
        else
        {
            Debug.LogWarning($"Music clip '{clipName}' not found!");
        }
    }
    
    // Dừng nhạc
    public void StopMusic()
    {
        musicSource.Stop();
    }
    
    // Pause/Resume nhạc
    public void PauseMusic()
    {
        musicSource.Pause();
    }
    
    public void ResumeMusic()
    {
        musicSource.UnPause();
    }
    
    // Cập nhật volume
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        UpdateAudioVolumes();
        SaveAudioSettings();
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        UpdateAudioVolumes();
        SaveAudioSettings();
    }
    
    // Đồng bộ hóa giá trị từ UIManager sliders
    public void SyncWithUIManager()
    {
        if (UIManager.Instance != null)
        {
            if (UIManager.Instance.musicSlider != null)
            {
                musicVolume = UIManager.Instance.musicSlider.value;
            }
            
            if (UIManager.Instance.sfxSlider != null)
            {
                sfxVolume = UIManager.Instance.sfxSlider.value;
            }
            
            UpdateAudioVolumes();
            SaveAudioSettings();
        }
    }
    
    // Lấy giá trị hiện tại từ UIManager
    public float GetMusicVolumeFromUI()
    {
        if (UIManager.Instance != null && UIManager.Instance.musicSlider != null)
        {
            return UIManager.Instance.musicSlider.value;
        }
        return musicVolume;
    }
    
    public float GetSFXVolumeFromUI()
    {
        if (UIManager.Instance != null && UIManager.Instance.sfxSlider != null)
        {
            return UIManager.Instance.sfxSlider.value;
        }
        return sfxVolume;
    }
    
    private void UpdateAudioVolumes()
    {
        if (musicSource.clip != null)
        {
            // Cập nhật volume cho music source hiện tại
            musicSource.volume = musicVolume;
        }
        
        // Cập nhật slider values nếu UIManager có sẵn
        if (UIManager.Instance != null)
        {
            if (UIManager.Instance.musicSlider != null)
            {
                UIManager.Instance.musicSlider.value = musicVolume;
            }
            
            if (UIManager.Instance.sfxSlider != null)
            {
                UIManager.Instance.sfxSlider.value = sfxVolume;
            }
        }
    }
    
    // Save/Load settings
    private void SaveAudioSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }
    
    private void LoadAudioSettings()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }
    
    // Kiểm tra kết nối với UIManager
    public bool IsConnectedToUIManager()
    {
        return UIManager.Instance != null && 
               UIManager.Instance.musicSlider != null && 
               UIManager.Instance.sfxSlider != null;
    }
    
    // Debug: In thông tin kết nối
    public void LogUIManagerConnection()
    {
        if (UIManager.Instance == null)
        {
            Debug.LogWarning("UIManager.Instance is null!");
            return;
        }
        
        Debug.Log($"UIManager connected: {UIManager.Instance != null}");
        Debug.Log($"Music Slider connected: {UIManager.Instance.musicSlider != null}");
        Debug.Log($"SFX Slider connected: {UIManager.Instance.sfxSlider != null}");
        
        if (UIManager.Instance.musicSlider != null)
        {
            Debug.Log($"Music Slider value: {UIManager.Instance.musicSlider.value}");
        }
        
        if (UIManager.Instance.sfxSlider != null)
        {
            Debug.Log($"SFX Slider value: {UIManager.Instance.sfxSlider.value}");
        }
    }
    
    // Kiểm tra có đủ âm thanh cần thiết không
    private void ValidateAudioClips()
    {
        string[] requiredSFX = {
            "FootStep", "Coin", "CloseGame", "OpenGame", "ButtonPress", 
            "WarmingRocket", "WarmingBoss"
        };
        
        string[] requiredMusic = {
            "MainTheme", "InGame", "BossMusic"
        };
        
        foreach (string sfx in requiredSFX)
        {
            if (!audioDict.ContainsKey(sfx))
            {
                Debug.LogWarning($"Missing SFX: {sfx}");
            }
        }
        
        foreach (string music in requiredMusic)
        {
            if (!audioDict.ContainsKey(music))
            {
                Debug.LogWarning($"Missing Music: {music}");
            }
        }
    }
}

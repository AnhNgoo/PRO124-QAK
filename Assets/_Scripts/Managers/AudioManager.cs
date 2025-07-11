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
    public float masterVolume = 1f;
    [Range(0f, 1f)]
    public float musicVolume = 1f;
    [Range(0f, 1f)]
    public float sfxVolume = 1f;
    
    public bool isMusicEnabled = true;
    public bool isSFXEnabled = true;
    
    private Dictionary<string, AudioClipData> audioDict = new Dictionary<string, AudioClipData>();
    
    void Start()
    {
        InitializeAudioDictionary();
        LoadAudioSettings();
        UpdateAudioVolumes();
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
        if (!isSFXEnabled) return;
        
        if (audioDict.ContainsKey(clipName))
        {
            AudioClipData data = audioDict[clipName];
            if (!data.isMusic)
            {
                sfxSource.PlayOneShot(data.clip, data.volume * sfxVolume * masterVolume);
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
        if (!isMusicEnabled) return;
        
        if (audioDict.ContainsKey(clipName))
        {
            AudioClipData data = audioDict[clipName];
            if (data.isMusic)
            {
                musicSource.clip = data.clip;
                musicSource.volume = data.volume * musicVolume * masterVolume;
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
    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        UpdateAudioVolumes();
        SaveAudioSettings();
    }
    
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
    
    // Toggle music/sfx
    public void ToggleMusic(bool enabled)
    {
        isMusicEnabled = enabled;
        if (!enabled)
        {
            musicSource.volume = 0;
        }
        else
        {
            UpdateAudioVolumes();
        }
        SaveAudioSettings();
    }
    
    public void ToggleSFX(bool enabled)
    {
        isSFXEnabled = enabled;
        SaveAudioSettings();
    }
    
    private void UpdateAudioVolumes()
    {
        if (musicSource.clip != null && isMusicEnabled)
        {
            musicSource.volume = musicVolume * masterVolume;
        }
    }
    
    // Save/Load settings
    private void SaveAudioSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetInt("MusicEnabled", isMusicEnabled ? 1 : 0);
        PlayerPrefs.SetInt("SFXEnabled", isSFXEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    private void LoadAudioSettings()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        isMusicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        isSFXEnabled = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    string path = "DataGame.es3";
    public DataGame data = new DataGame();


    private void OnApplicationQuit()
    {
        if (GameManager.Instance.sessionState == GameManager.SessionState.InProgress)
        {
            GameManager.Instance.UpdateProperties();
        }
        Save();
    }
    void Start()
    {
        Debug.Log(Application.persistentDataPath);
    }
    public void Save()
    {
        SavePlayerData();
        SaveSkins();
        SaveJetpacks();
        SaveSettings();
        SaveQuests();

        // Lưu tên skin và jetpack đang được chọn
        data.currentSkinName = SkinManager.Instance.skins.currentItemName;
        data.currentJetpackEffectName = JetpackEffectManager.Instance.jetpackEffects.currentItemName;

        // Save toàn bộ các phần vào file
        ES3.Save("DataPlayer", data.playerData, path);
        ES3.Save("DataSkins", data.skins, path);
        ES3.Save("DataJetpacks", data.jetpacks, path);
        ES3.Save("DataSettings", data.settings, path);
        ES3.Save("DataQuests", data.questData, path);
        ES3.Save("CurrentSkinName", data.currentSkinName, path);
        ES3.Save("CurrentJetpackEffectName", data.currentJetpackEffectName, path);
    }

    private void SavePlayerData()
    {
        data.playerData.coinTotal = GameManager.Instance.coinTotal;
        data.playerData.distanceBest = GameManager.Instance.distanceBest;
    }

    private void SaveQuests()
    {
        if (QuestManager.Instance != null)
        {
            data.questData.activeQuests.Clear();
            
            foreach (var quest in QuestManager.Instance.activeQuests)
            {
                data.questData.activeQuests.Add(new DataQuestItem(quest));
            }
            
            data.questData.lastDailyQuestDate = QuestManager.Instance.GetLastDailyQuestDate();
        }
    }

    private void SaveSkins()
    {
        data.skins.Clear();
        foreach (var skin in SkinManager.Instance.skins.itemList)
        {
            DataSkin dataSkin = new DataSkin
            {
                skinName = skin.skinName,
                status = skin.status == Skin.Status.Unlocked ? DataSkin.Status.Unlocked : DataSkin.Status.Locked
            };
            data.skins.Add(dataSkin);
        }
    }

    private void SaveJetpacks()
    {
        data.jetpacks.Clear();
        foreach (var jetpack in JetpackEffectManager.Instance.jetpackEffects.itemList)
        {
            DataJetpack dataJetpack = new DataJetpack
            {
                jetpackEffectName = jetpack.jetpackEffectName,
                status = jetpack.status == JetpackEffect.Status.Unlocked ? DataJetpack.Status.Unlocked : DataJetpack.Status.Locked
            };
            data.jetpacks.Add(dataJetpack);
        }
    }

    private void SaveSettings()
    {
        data.settings.musicVolume = UIManager.Instance.musicSlider.value;
        data.settings.sfxVolume = UIManager.Instance.sfxSlider.value;
    }

    public void Load()
    {
        LoadPlayerData();
        LoadSkins();
        LoadJetpacks();
        LoadSettings();
        LoadQuests();
    }

    private void LoadPlayerData()
    {
        if (ES3.KeyExists("DataPlayer", path))
        {
            DataPlayer playerData = ES3.Load<DataPlayer>("DataPlayer", path);
            GameManager.Instance.coinTotal = playerData.coinTotal;
            GameManager.Instance.distanceBest = playerData.distanceBest;
        }
    }

    private void LoadSkins()
    {
        if (ES3.KeyExists("DataSkins", path))
        {
            List<DataSkin> savedSkins = ES3.Load<List<DataSkin>>("DataSkins", path);
            foreach (var skin in SkinManager.Instance.skins.itemList)
            {
                var saved = savedSkins.Find(s => s.skinName == skin.skinName);
                if (saved != null)
                    skin.status = saved.status == DataSkin.Status.Unlocked ? Skin.Status.Unlocked : Skin.Status.Locked;
            }
        }

        if (ES3.KeyExists("CurrentSkinName", path))
        {
            SkinManager.Instance.skins.currentItemName = ES3.Load<string>("CurrentSkinName", path);
        }
    }

    private void LoadJetpacks()
    {
        if (ES3.KeyExists("DataJetpacks", path))
        {
            List<DataJetpack> savedJetpacks = ES3.Load<List<DataJetpack>>("DataJetpacks", path);
            foreach (var jetpack in JetpackEffectManager.Instance.jetpackEffects.itemList)
            {
                var saved = savedJetpacks.Find(j => j.jetpackEffectName == jetpack.jetpackEffectName);
                if (saved != null)
                    jetpack.status = saved.status == DataJetpack.Status.Unlocked ? JetpackEffect.Status.Unlocked : JetpackEffect.Status.Locked;
            }
        }

        if (ES3.KeyExists("CurrentJetpackEffectName", path))
        {
            JetpackEffectManager.Instance.jetpackEffects.currentItemName = ES3.Load<string>("CurrentJetpackEffectName", path);
        }
    }

    private void LoadSettings()
    {
        if (ES3.KeyExists("DataSettings", path))
        {
            DataSettings settings = ES3.Load<DataSettings>("DataSettings", path);
            UIManager.Instance.musicSlider.value = settings.musicVolume;
            UIManager.Instance.sfxSlider.value = settings.sfxVolume;
        }
    }

    private void LoadQuests()
    {
        if (ES3.KeyExists("DataQuests", path))
        {
            DataQuest questData = ES3.Load<DataQuest>("DataQuests", path);
            data.questData = questData;
            
            // Load quest data vào QuestManager
            if (QuestManager.Instance != null)
            {
                QuestManager.Instance.LoadFromSaveData(data.questData);
            }
        }
    }

}

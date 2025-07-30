using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataGame
{
    public DataPlayer playerData = new DataPlayer();

    public List<DataSkin> skins = new List<DataSkin>();
    public string currentSkinName;

    public List<DataJetpack> jetpacks = new List<DataJetpack>();
    public string currentJetpackEffectName;

    public DataSettings settings = new DataSettings();

    public DataQuest questData = new DataQuest();
}

[System.Serializable]
public class DataPlayer
{
    public int coinTotal;
    public int distanceBest;
}

[System.Serializable]
public class DataSkin
{
    public enum Status
    {
        Unlocked,
        Locked
    }

    public string skinName;
    public Status status;
}

[System.Serializable]
public class DataJetpack
{
    public enum Status
    {
        Unlocked,
        Locked
    }

    public string jetpackEffectName;
    public Status status;
}

[System.Serializable]
public class DataSettings
{
    public float musicVolume;
    public float sfxVolume;
}

[System.Serializable]
public class DataQuest
{
    public List<DataQuestItem> activeQuests = new List<DataQuestItem>();
    public string lastDailyQuestDate; // Lưu ngày cuối cùng tạo daily quest
    public int completedQuestsToday; // Số quest đã hoàn thành hôm nay
}

[System.Serializable]
public class DataQuestItem
{
    public string questId;
    public int currentProgress;
    public bool isCompleted;
    public bool isClaimed;
    public QuestDifficulty difficulty;
    
    public DataQuestItem(Quest quest)
    {
        questId = quest.questId;
        currentProgress = quest.currentProgress;
        isCompleted = quest.isCompleted;
        isClaimed = quest.isClaimed;
        difficulty = quest.difficulty;
    }
}
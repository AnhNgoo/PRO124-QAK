using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class QuestManager : Singleton<QuestManager>
{
    [Header("Quest Settings")]
    public QuestData questData;

    [Header("Active Quests")]
    public List<Quest> activeQuests = new List<Quest>();

    [Header("Quest Limits")]
    public int maxHardQuests = 1;
    public int maxNormalQuests = 3;
    public int maxDailyQuests = 1;

    private string lastDailyQuestDate;
    

    void Start()
    {
        InitializeQuests();
    }

    private void InitializeQuests()
    {
        // Load data từ SaveManager trước
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.Load();
        }
        
        CheckDailyQuestReset();
        FillMissingQuests();
    }

    #region Quest Generation
    private void CheckDailyQuestReset()
    {
        string today = DateTime.Now.ToString("yyyy-MM-dd");

        if (lastDailyQuestDate != today)
        {
            // Reset daily quest mỗi ngày
            RemoveQuestsByDifficulty(QuestDifficulty.Daily);
            GenerateRandomQuest(QuestDifficulty.Daily);
            lastDailyQuestDate = today;

            Debug.Log("Daily quest reset for: " + today);
        }
    }

    private void FillMissingQuests()
    {
        // Đảm bảo có đủ số lượng quest theo từng loại
        int hardCount = GetActiveQuestCountByDifficulty(QuestDifficulty.Hard);
        int normalCount = GetActiveQuestCountByDifficulty(QuestDifficulty.Normal);
        int dailyCount = GetActiveQuestCountByDifficulty(QuestDifficulty.Daily);

        // Tạo quest thiếu
        for (int i = hardCount; i < maxHardQuests; i++)
        {
            GenerateRandomQuest(QuestDifficulty.Hard);
        }

        for (int i = normalCount; i < maxNormalQuests; i++)
        {
            GenerateRandomQuest(QuestDifficulty.Normal);
        }

        for (int i = dailyCount; i < maxDailyQuests; i++)
        {
            GenerateRandomQuest(QuestDifficulty.Daily);
        }
    }

    private void GenerateRandomQuest(QuestDifficulty difficulty)
    {
        List<Quest> sourceQuests = GetQuestsByDifficulty(difficulty);

        if (sourceQuests.Count == 0) return;

        // Lọc quest chưa active
        var availableQuests = sourceQuests.Where(q => !IsQuestActive(q.questId)).ToList();

        if (availableQuests.Count == 0)
        {
            Debug.LogWarning($"No available {difficulty} quests to generate!");
            return;
        }

        // Random quest
        Quest randomQuest = availableQuests[UnityEngine.Random.Range(0, availableQuests.Count)];

        // Tạo copy của quest để không ảnh hưởng đến data gốc
        Quest newQuest = CreateQuestCopy(randomQuest);
        newQuest.isActive = true;

        activeQuests.Add(newQuest);

        Debug.Log($"Generated new {difficulty} quest: {newQuest.questName}");
    }

    private Quest CreateQuestCopy(Quest original)
    {
        return new Quest
        {
            questId = original.questId,
            questName = original.questName,
            description = original.description,
            questType = original.questType,
            difficulty = original.difficulty,
            targetValue = original.targetValue,
            currentProgress = 0,
            coinReward = original.coinReward,
            isCompleted = false,
            isActive = true,
            isClaimed = false
        };
    }
    #endregion

    #region Quest Progress
    public void UpdateQuestProgress(QuestType questType, int amount = 1)
    {
        var relevantQuests = activeQuests.Where(q => q.questType == questType && q.isActive && !q.isCompleted).ToList();

        foreach (var quest in relevantQuests)
        {
            quest.currentProgress = Mathf.Min(quest.currentProgress + amount, quest.targetValue);

            if (quest.currentProgress >= quest.targetValue && !quest.isCompleted)
            {
                CompleteQuest(quest);
            }
        }

        // Tự động save sau khi update progress
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.Save();
        }
    }

    private void CompleteQuest(Quest quest)
    {
        quest.isCompleted = true;

        Debug.Log($"Quest completed: {quest.questName}");

        // Có thể thêm UI notification ở đây
        ShowQuestCompletedNotification(quest);

        // Tự động tạo quest mới nếu không phải daily quest
        if (quest.difficulty != QuestDifficulty.Daily)
        {
            StartCoroutine(GenerateNewQuestAfterDelay(quest.difficulty));
        }
    }

    private IEnumerator GenerateNewQuestAfterDelay(QuestDifficulty difficulty)
    {
        yield return new WaitForSeconds(1f); // Delay 1 giây
        GenerateRandomQuest(difficulty);
    }

    public void ClaimQuestReward(string questId)
    {
        Quest quest = activeQuests.FirstOrDefault(q => q.questId == questId);

        if (quest != null && quest.isCompleted && !quest.isClaimed)
        {
            // Thêm coin reward
            GameManager.Instance.coinTotal += quest.coinReward;
            quest.isClaimed = true;

            // Cập nhật UI coin
            if (UIManager.Instance.coinTotalText != null)
            {
                UIManager.Instance.coinTotalText.text = GameManager.Instance.coinTotal.ToString();
            }

            Debug.Log($"Claimed reward for quest: {quest.questName} (+{quest.coinReward} coins)");

            // Remove quest khỏi active list sau khi claim
            activeQuests.Remove(quest);

            // Tự động save
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.Save();
            }
        }
    }
    #endregion

    #region Utility Methods
    private List<Quest> GetQuestsByDifficulty(QuestDifficulty difficulty)
    {
        switch (difficulty)
        {
            case QuestDifficulty.Hard:
                return questData.hardQuests;
            case QuestDifficulty.Normal:
                return questData.normalQuests;
            case QuestDifficulty.Daily:
                return questData.dailyQuests;
            default:
                return new List<Quest>();
        }
    }

    private int GetActiveQuestCountByDifficulty(QuestDifficulty difficulty)
    {
        return activeQuests.Count(q => q.difficulty == difficulty && q.isActive);
    }

    private bool IsQuestActive(string questId)
    {
        return activeQuests.Any(q => q.questId == questId && q.isActive);
    }

    private void RemoveQuestsByDifficulty(QuestDifficulty difficulty)
    {
        activeQuests.RemoveAll(q => q.difficulty == difficulty);
    }

    private void ShowQuestCompletedNotification(Quest quest)
    {
        // TODO: Implement UI notification
        Debug.Log($"🎉 Quest Completed: {quest.questName} - Reward: {quest.coinReward} coins");
    }
    #endregion

    #region SaveManager Interface
    // Method để SaveManager load data vào QuestManager
    public void LoadFromSaveData(DataQuest questSaveData)
    {
        lastDailyQuestDate = questSaveData.lastDailyQuestDate;
        
        activeQuests.Clear();
        
        foreach (var savedQuest in questSaveData.activeQuests)
        {
            // Tìm quest template từ questData
            Quest template = FindQuestTemplate(savedQuest.questId, savedQuest.difficulty);
            
            if (template != null)
            {
                Quest loadedQuest = CreateQuestCopy(template);
                loadedQuest.currentProgress = savedQuest.currentProgress;
                loadedQuest.isCompleted = savedQuest.isCompleted;
                loadedQuest.isClaimed = savedQuest.isClaimed;
                
                activeQuests.Add(loadedQuest);
            }
        }
    }

    // Method để SaveManager lấy lastDailyQuestDate
    public string GetLastDailyQuestDate()
    {
        return lastDailyQuestDate;
    }

    private Quest FindQuestTemplate(string questId, QuestDifficulty difficulty)
    {
        List<Quest> sourceQuests = GetQuestsByDifficulty(difficulty);
        return sourceQuests.FirstOrDefault(q => q.questId == questId);
    }
    #endregion

    #region Public API
    public List<Quest> GetActiveQuests()
    {
        return activeQuests.ToList();
    }

    public List<Quest> GetCompletedQuests()
    {
        return activeQuests.Where(q => q.isCompleted).ToList();
    }

    public float GetQuestProgress(string questId)
    {
        Quest quest = activeQuests.FirstOrDefault(q => q.questId == questId);
        if (quest != null)
        {
            return (float)quest.currentProgress / quest.targetValue;
        }
        return 0f;
    }
    #endregion
}
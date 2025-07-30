using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "ScriptableObjects/QuestData", order = 2)]
public class QuestData : ScriptableObject
{
    [Header("Quest Lists")]
    public List<Quest> hardQuests = new List<Quest>();
    public List<Quest> normalQuests = new List<Quest>();
    public List<Quest> dailyQuests = new List<Quest>();
}

[System.Serializable]
public class Quest
{
    [Header("Quest Info")]
    public string questId;
    public string questName;
    [TextArea(2, 4)]
    public string description;
    public QuestType questType;
    public QuestDifficulty difficulty;
    
    [Header("Requirements")]
    public int targetValue; // Mục tiêu cần đạt (ví dụ: thu thập 100 coin, bay 500m, ...)
    public int currentProgress; // Tiến độ hiện tại
    
    [Header("Rewards")]
    public int coinReward; // Phần thưởng coin
    
    [Header("Status")]
    public bool isCompleted;
    public bool isActive;
    public bool isClaimed; // Đã nhận thưởng chưa
}

[System.Serializable]
public enum QuestType
{
    CollectCoins,        // Thu thập coin
    TravelDistance,      // Di chuyển khoảng cách
    UsePowerUp,          // Sử dụng power-up
    SurviveTime,         // Sống sót trong thời gian
    PlayGames,           // Chơi số lượt game
    CompleteWithoutDying, // Hoàn thành mà không chết
    CollectSpecificPowerUp, // Thu thập power-up cụ thể
    ReachScore,          // Đạt điểm số
    DefeatBoss,          // Đánh bại boss
    AvoidObstacles       // Tránh vật cản
}

[System.Serializable]
public enum QuestDifficulty
{
    Daily,    // Nhiệm vụ hằng ngày
    Normal,   // Nhiệm vụ thường
    Hard      // Nhiệm vụ khó
}
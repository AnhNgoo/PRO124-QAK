using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : Singleton<GameManager>
{
    public GameObject players;
    public int coinTotal = 0; // Tổng số coin đã thu thập
    public int coinIngame = 0; // Số lượng coin trong game
    public int distanceTraveled => (int)DistanceTracker.Instance.distanceTraveled; // Khoảng cách đã di chuyển
    public int distanceBest = 0;
    public int playerTotal { get; set; } = 0;
    public int playerCount { get; set; } = 0;
    public enum SessionState { InProgress, Finished }
    public enum GameMode { Normal = 1, PVP = 2 }

    private GameMode _gameMode = GameMode.Normal; // Mặc định là chế độ Normal
    public GameMode gameMode
    {
        get => _gameMode;
        set
        {
            _gameMode = value;
            SetNumberOfPlayers();
        }
    }
    public SessionState sessionState = SessionState.InProgress;
    public string lastDeadPlayerName { get; set; } // Lưu tên người chơi đã chết cuối cùng

    void Start()
    {
        Application.targetFrameRate = 60; // Đặt tốc độ khung hình mục tiêu
    }
    public void UpdateProperties()
    {
        coinTotal += coinIngame; // Cộng dồn coin đã thu thập vào tổng số coin
        distanceBest = Mathf.Max(distanceBest, distanceTraveled); // Cập nhật khoảng cách tốt nhất
    }

    public void OnPlayerDeath(GameObject player)
    {
        if (gameMode == GameMode.PVP)
            lastDeadPlayerName = player.name == "Player 1" ? "Player 2" : "Player 1"; // Lấy tên người chơi còn sống

        // Gọi sự kiện khi người chơi chết
        GameEvent.Instance.TriggerEvent("PlayerDeath");
    }

    public void SetNumberOfPlayers()
    {
        switch (gameMode)
        {
            case GameMode.Normal:
                playerTotal = (int)gameMode;
                break;
            case GameMode.PVP:
                playerTotal = (int)gameMode;
                break;
        }
        playerCount = 0; // Đặt lại số lượng người chơi đã chết
    }
}

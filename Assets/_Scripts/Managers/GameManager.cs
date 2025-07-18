using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int coinTotal = 0; // Tổng số coin đã thu thập
    public int coinIngame = 0; // Số lượng coin trong game
    public int distanceTraveled => (int)DistanceTracker.Instance.distanceTraveled; // Khoảng cách đã di chuyển
    public int distanceBest = 0;
    public enum SessionState { InProgress, Finished }
    public SessionState sessionState = SessionState.InProgress;


    public void UpdateProperties()
    {
        coinTotal += coinIngame; // Cộng dồn coin đã thu thập vào tổng số coin
        distanceBest = Mathf.Max(distanceBest, distanceTraveled); // Cập nhật khoảng cách tốt nhất
    }
}

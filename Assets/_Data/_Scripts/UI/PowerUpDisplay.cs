using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpDisplay : Singleton<PowerUpDisplay>
{
    public GameObject powerUpDisplay;
    public List<GameObject> powerUpSlots;

    // Dictionary để track PowerUp đã show và GameObject tương ứng
    private Dictionary<string, GameObject> activePowerUps = new();

    private void Start()
    {
        AddListPowerUpSlotsAndSetParent();
    }

    private void AddListPowerUpSlotsAndSetParent()
    {
        ObjectPooler.Instance.Add("PowerUpSlot", powerUpSlots);
        ObjectPooler.Instance.SetParents(powerUpDisplay, "PowerUpSlot");
    }

    public void ShowPowerUp(string powerUpName)
    {
        // Kiểm tra PowerUp đã được show chưa
        if (activePowerUps.ContainsKey(powerUpName))
        {
            GameObject _powerUpSlot = activePowerUps[powerUpName];
            _powerUpSlot.GetComponent<DOTweenAnimation>().DORewind(); //Reset animation nếu lỡ TimeOutWarning
            return; // Đã show rồi, không show nữa
        }

        GameObject powerUpSlot = ObjectPooler.Instance.SpawnFromQueue("PowerUpSlot", Vector3.zero, Quaternion.identity);
        if (powerUpSlot != null)
        {
            Image icon = powerUpSlot.GetComponent<Image>();
            if (icon != null)
            {
                icon.sprite = PowerUpManager.Instance.powerUpData.powerUps
                    .FirstOrDefault(powerUp => powerUp.powerUpName == powerUpName)?.icon;

                // Lưu vào dictionary để theo dõi 
                activePowerUps.Add(powerUpName, powerUpSlot);
            }
        }
    }

    // Hàm ẩn PowerUp bằng tên và destroy bằng ObjectPooler
    public void HidePowerUp(string powerUpName)
    {
        if (activePowerUps.ContainsKey(powerUpName))
        {
            GameObject powerUpSlot = activePowerUps[powerUpName];

            // Destroy bằng ObjectPooler
            ObjectPooler.Instance.DesTroy(powerUpSlot);

            // Remove khỏi dictionary
            activePowerUps.Remove(powerUpName);
        }
        else
        {
            Debug.LogWarning($"PowerUp {powerUpName} not found in active list");
        }
    }

    //Khi gần hết thời gian sẽ nhấp nháy để cảnh báo
    public void TimeOutWarning(string powerUpName)
    {
        if (!activePowerUps.ContainsKey(powerUpName)) return;

        GameObject powerUpSlot = activePowerUps[powerUpName];
        powerUpSlot.GetComponent<DOTweenAnimation>().DOPlay();
    }
    // Clear tất cả PowerUp (khi game over, restart...)
    public void ClearAllPowerUps()
    {
        foreach (var kvp in activePowerUps)
        {
            ObjectPooler.Instance.DesTroy(kvp.Value);
        }

        activePowerUps.Clear();
    }
}

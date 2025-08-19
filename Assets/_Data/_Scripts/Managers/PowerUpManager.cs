using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PowerUpManager : Singleton<PowerUpManager>
{
    public PowerUpData powerUpData;
    public float powerUpSpawnDistance = 100f; // Khoảng cách để spawn PowerUp
    public List<GameObject> powerUpList = new();

    private float nextPowerUpSpawnDistance = 0f;

    public Shield shield { get; private set; }

    private void Start()
    {
        InitComponent();
        nextPowerUpSpawnDistance = powerUpSpawnDistance;
    }
    private void Update()
    {
        SpawnPowerUpByDistance();
    }

    private void InitComponent()
    {
        shield = transform.Cast<Transform>()
                   .Select(t => t.GetComponent<Shield>())
                   .FirstOrDefault(s => s != null);
    }

    /// <summary>
    /// Kích hoạt PowerUp <br/>
    /// Khi player ăn powerup, Scripts PlayerPowerUp sẽ gọi hàm này và truyền tên powerup,gameobject của player đó<br/>
    /// Nó sẽ lấy gameobject powerUpObj đúng với tên của powerup trong powerUpList <br/>
    /// Lấy thời gian dùng power trong powerUpData <br/>
    /// Khởi tạo init và truyền các tham số vào init của powerUp
    /// </summary>
    public void ActivePowerUp(string namePowerUp, GameObject player = null)
    {
        var powerUpObj = powerUpList
                         .FirstOrDefault(powerUp => powerUp.name == namePowerUp);

        float duration = powerUpData.powerUps
            .FirstOrDefault(powerUp => powerUp.powerUpName == namePowerUp)?.duration ?? 0f;

        if (powerUpObj != null)
        {
            powerUpObj.gameObject.SetActive(false);
            powerUpObj.gameObject.SetActive(true);
            powerUpObj.GetComponent<IPowerUp>().Init(duration, player);
            PowerUpDisplay.Instance.ShowPowerUp(namePowerUp);
        }
    }

    //distanceTraveled tăng cứ cách nextPowerUpSpawnDistance = N thì sẽ gọi SpawnPowerUp của nextMap trong mapSpawner ra để tạo powerup 
    // và cộng powerUpSpawnDistance vào nextPowerUpSpawnDistance để spawn đoạn kế tiếp
    private void SpawnPowerUpByDistance()
    {
        if (DistanceTracker.Instance.distanceTraveled >= nextPowerUpSpawnDistance)
        {
            nextPowerUpSpawnDistance += powerUpSpawnDistance;
            MapSpawner.Instance.nextMap.GetComponent<ResetMap>().SpawnPowerUp();
        }
    }
}

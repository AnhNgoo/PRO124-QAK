using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PowerUpManager : Singleton<PowerUpManager>
{
    public PowerUpData powerUpData;
    public float powerUpSpawnDistance = 100f; // Khoảng cách để spawn PowerUp
    public List<GameObject> powerUpList = new();

    private PlayerPowerUp playerPowerUp;
    private float nextPowerUpSpawnDistance = 0f;

    public Shield shield { get; private set; }
    private void Start()
    {
        InitComponent();
        nextPowerUpSpawnDistance = powerUpSpawnDistance;
        playerPowerUp.powerUpEvent += ActivePowerUp;
    }
    private void Update()
    {
        SpawnPowerUpByDistance();
    }

    private void InitComponent()
    {
        playerPowerUp = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPowerUp>();
        shield = transform.Cast<Transform>()
                   .Select(t => t.GetComponent<Shield>())
                   .FirstOrDefault(s => s != null);
    }

    private void ActivePowerUp(string namePowerUp)
    {
        var powerUpObj = powerUpList
                         .FirstOrDefault(powerUp => powerUp.name == namePowerUp);

        float duration = powerUpData.powerUps
            .FirstOrDefault(powerUp => powerUp.powerUpName == namePowerUp)?.duration ?? 0f;

        if (powerUpObj != null)
        {
            powerUpObj.gameObject.SetActive(false);
            powerUpObj.gameObject.SetActive(true);
            powerUpObj.GetComponent<IPowerUp>().Init(duration);
            PowerUpDisplay.Instance.ShowPowerUp(namePowerUp);
        }
    }
    private void SpawnPowerUpByDistance()
    {
        if (DistanceTracker.Instance.distanceTraveled >= nextPowerUpSpawnDistance)
        {
            nextPowerUpSpawnDistance += powerUpSpawnDistance;
            MapSpawner.Instance.currentMap.GetComponent<ResetMap>().SpawnPowerUp();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PowerUpManager : Singleton<PowerUpManager>
{
    public List<GameObject> powerUpList = new();

    private PlayerPowerUp playerPowerUp;

    public Shield shield { get; private set; }
    private void Start()
    {
        InitComponent();
        playerPowerUp.powerUpEvent += ActivePowerUp;
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
        if (powerUpObj != null)
        {
            powerUpObj.gameObject.SetActive(false);
            powerUpObj.gameObject.SetActive(true);
        }
    }
}

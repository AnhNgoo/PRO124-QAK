using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, IPowerUp
{
    public float lifeTime = 30;

    private GameObject powerUpManager;
    private PlayerDeath playerDeath;

    void Update()
    {
        lifeTime -= Time.deltaTime;
        Disable();
    }

    //Khi player ăn được powerup, nó sẽ bật gameobject chứa script này và gọi hàm init để khởi tạo thời gian tồn tại và người chơi nhận powerup
    public void Init(float duration, GameObject player = null)
    {
        if (playerDeath != null)
        {
            playerDeath.isActiveShield = false;
            playerDeath = null;
        }

        lifeTime = duration; // Reset thời gian sống khi kích hoạt lại

        if (player != null)
        {
            playerDeath = player.GetComponent<PlayerDeath>();
            if (playerDeath != null)
            {
                playerDeath.isActiveShield = true; // Kích hoạt shield cho người chơi
            }
        }
        powerUpManager = GameObject.Find("PowerUpManager");
        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;

    }

    private void Disable()
    {
        if (lifeTime > 3 && !InRunEventsManager.Instance.isBigEventActive) return;
        PowerUpDisplay.Instance.TimeOutWarning(gameObject.name);

        if (lifeTime > 0 && !InRunEventsManager.Instance.isBigEventActive) return;

        if (playerDeath != null)
        {
            playerDeath.isActiveShield = false; // Tắt shield cho người chơi
        }
        gameObject.SetActive(false);
        gameObject.transform.SetParent(powerUpManager.transform);
        PowerUpDisplay.Instance.HidePowerUp(gameObject.name);
    }
}

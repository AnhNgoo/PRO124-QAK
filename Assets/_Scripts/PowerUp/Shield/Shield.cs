using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, IPowerUp
{
    public float lifeTime = 30;

    public bool isActive { get; private set; } = false;
    private GameObject player;
    private GameObject powerUpManager;


    void Update()
    {
        lifeTime -= Time.deltaTime;
        Disable();
    }

    public void Init(float duration)
    {
        lifeTime = duration; // Reset thời gian sống khi kích hoạt lại

        player = GameObject.FindGameObjectWithTag("Player");
        powerUpManager = GameObject.Find("PowerUpManager");

        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;
        isActive = true;
    }

    private void Disable()
    {
        if (lifeTime > 3 && !InRunEventsManager.Instance.isBigEventActive) return;
        PowerUpDisplay.Instance.TimeOutWarning(gameObject.name);

        if (lifeTime > 0 && !InRunEventsManager.Instance.isBigEventActive) return;

        // Dừng tất cả coroutines
        StopAllCoroutines();
        gameObject.SetActive(false);
        gameObject.transform.SetParent(powerUpManager.transform);
        PowerUpDisplay.Instance.HidePowerUp(gameObject.name);
        isActive = false;

    }
}

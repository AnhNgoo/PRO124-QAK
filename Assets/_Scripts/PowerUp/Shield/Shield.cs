using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float lifeTime = 30;

    public bool isActive { get; private set; } = false;
    private GameObject player;
    private GameObject powerUpManager;


    private void OnEnable()
    {
        Init();
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;
        Disable();
    }

    private void Init()
    {
        lifeTime = 30; // Reset thời gian sống khi kích hoạt lại

        player = GameObject.FindGameObjectWithTag("Player");
        powerUpManager = GameObject.Find("PowerUpManager");

        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;
        isActive = true;
    }

    private void Disable()
    {
        if (lifeTime <= 0)
        {
            gameObject.SetActive(false);
            gameObject.transform.SetParent(powerUpManager.transform);
            isActive = false;
        }
    }
}

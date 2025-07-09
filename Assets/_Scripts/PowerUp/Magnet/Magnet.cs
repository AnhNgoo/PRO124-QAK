using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public float lifeTime = 30;
    private GameObject player;
    private GameObject powerUpManager;
    private HashSet<GameObject> tweenedCoins = new();


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
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin") && !tweenedCoins.Contains(collision.gameObject))
        {
            tweenedCoins.Add(collision.gameObject);

            collision.transform
                .DOMove(player.transform.position, 0.5f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    tweenedCoins.Remove(collision.gameObject); // để có thể tween lại nếu cần
                });
        }
    }

    private void Disable()
    {
        if (lifeTime <= 0)
        {
            gameObject.SetActive(false);
            gameObject.transform.SetParent(powerUpManager.transform);
            tweenedCoins.Clear(); // Xóa tất cả các coin đã tween
        }
    }
}

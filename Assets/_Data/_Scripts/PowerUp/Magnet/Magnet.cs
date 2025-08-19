using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour, IPowerUp
{
    public float lifeTime = 20;
    private GameObject player;
    private GameObject powerUpManager;
    private Collider2D _collider;
    private HashSet<GameObject> tweenedCoins = new(); // Giúp loại bỏ trùng lặp giữa các xu
    void Update()
    {
        lifeTime -= Time.deltaTime;
        Disable();
    }

    //Khi player ăn được powerup, nó sẽ bật gameobject chứa script này và gọi hàm init để khởi tạo thời gian tồn tại và người chơi nhận powerup
    public void Init(float duration, GameObject player = null)
    {
        if (_collider == null)
        {
            _collider = GetComponent<Collider2D>();
        }
        lifeTime = duration; // Reset thời gian sống khi kích hoạt lại

        if (player != null)
            this.player = player;

        powerUpManager = GameObject.Find("PowerUpManager");
        transform.SetParent(this.player.transform);
        transform.localPosition = Vector3.zero;
        _collider.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.activeSelf == false) return;

        if (collision.CompareTag("Coin") && !tweenedCoins.Contains(collision.gameObject))
        {
            tweenedCoins.Add(collision.gameObject);
            StartCoroutine(MoveCoinToPlayer(collision.gameObject));
        }
    }

    // Bắt đâu di chuyển coin về phía player cho đến khi coin nhỏ hơn hoặc bằng 0.2
    private IEnumerator MoveCoinToPlayer(GameObject coin)
    {
        float speed = 30f; // Tốc độ di chuyển về player

        while (coin != null && player != null && Vector3.Distance(coin.transform.position, player.transform.position) > 0.2f)
        {
            // Di chuyển coin về phía player liên tục
            coin.transform.position = Vector3.MoveTowards(
                coin.transform.position,
                player.transform.position,
                speed * Time.deltaTime
            );

            yield return null;
        }

        // Khi coin đến gần player, remove khỏi list
        if (coin != null)
            tweenedCoins.Remove(coin);
    }

    private void Disable()
    {
        if (lifeTime > 3 && !InRunEventsManager.Instance.isBigEventActive) return;
        PowerUpDisplay.Instance.TimeOutWarning(gameObject.name);

        if (lifeTime > 0 && !InRunEventsManager.Instance.isBigEventActive) return;
        // Dừng tất cả coroutines
        StopAllCoroutines();

        _collider.enabled = false;
        gameObject.SetActive(false);
        gameObject.transform.SetParent(powerUpManager.transform);
        PowerUpDisplay.Instance.HidePowerUp(gameObject.name);
        tweenedCoins.Clear(); // Xóa tất cả các coin đã tween
    }
}

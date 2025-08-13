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
    private HashSet<GameObject> tweenedCoins = new();

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _collider.enabled = false;
        gameObject.SetActive(false);
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;
        Disable();
    }

    public void Init(float duration, GameObject player = null)
    {
        lifeTime = duration; // Reset thời gian sống khi kích hoạt lại

        player = GameObject.FindGameObjectWithTag("Player");
        powerUpManager = GameObject.Find("PowerUpManager");
        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;
        _collider.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D collision) // Thay đổi từ Stay2D thành Enter2D
    {
        if (collision.CompareTag("Coin") && !tweenedCoins.Contains(collision.gameObject))
        {
            tweenedCoins.Add(collision.gameObject);
            StartCoroutine(MoveCoinToPlayer(collision.gameObject));
        }
    }

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

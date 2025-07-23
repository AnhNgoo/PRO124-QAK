using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterShield : MonoBehaviour, IPowerUp
{
    [Header("CounterShield Settings")]
    public float lifeTime = 30f;
    
    [Header("Visual Effects")]
    public Color shieldColor = Color.red; // Màu của counter shield để phân biệt với shield thường
    
    public bool isActive { get; private set; } = false;
    private GameObject player;
    private GameObject powerUpManager;
    private Collider2D shieldCollider;

    void Start()
    {
        SetupShieldCollider();
    }

    void Update()
    {
        if (!isActive) return;
        
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
        
        // BotPlayer đã bị xóa - không cần clear danh sách bot nữa
        
        // Enable collider để detect collision (hiện tại không có mục tiêu)
        if (shieldCollider != null)
        {
            shieldCollider.enabled = true;
        }
        
        Debug.Log("CounterShield activated!");
    }

    private void SetupShieldCollider()
    {
        // Tạo collider cho shield nếu chưa có
        if (shieldCollider == null)
        {
            CircleCollider2D circleCollider = gameObject.AddComponent<CircleCollider2D>();
            circleCollider.isTrigger = true;
            circleCollider.radius = 1.5f; // Phạm vi shield lớn hơn player một chút
            shieldCollider = circleCollider;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra nếu không active thì không làm gì
        if (!isActive) return;
        
        // BotPlayer đã bị xóa - không cần xử lý bot nữa
        // CounterShield hiện tại không có mục tiêu để counter
    }

    // BotPlayer đã bị xóa - xóa các method liên quan đến bot

    private void Disable()
    {
        // Logic tương tự Shield
        if (lifeTime > 3 && !InRunEventsManager.Instance.isBigEventActive) return;
        PowerUpDisplay.Instance.TimeOutWarning(gameObject.name);

        if (lifeTime > 0 && !InRunEventsManager.Instance.isBigEventActive) return;

        // Dừng tất cả coroutines
        StopAllCoroutines();
        
        // Disable collider
        if (shieldCollider != null)
        {
            shieldCollider.enabled = false;
        }
        
        // BotPlayer đã bị xóa - không cần clear danh sách bot nữa
        
        gameObject.SetActive(false);
        gameObject.transform.SetParent(powerUpManager.transform);
        PowerUpDisplay.Instance.HidePowerUp(gameObject.name);
        isActive = false;
        
        Debug.Log("CounterShield deactivated!");
    }
}

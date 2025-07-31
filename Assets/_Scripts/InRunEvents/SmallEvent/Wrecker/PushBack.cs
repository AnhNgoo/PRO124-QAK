using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBack : MonoBehaviour
{
    [Header("Push Settings")]
    public Vector2 pushDirection = Vector2.right; // Hướng đẩy, chỉnh ngoài Inspector
    public float pushForce = 10f;                 // Lực đẩy

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null)
        {
            // Reset vận tốc trước khi đẩy (nếu muốn)
            rb.velocity = Vector2.zero;
            // Đẩy theo hướng đã chỉnh, giữ nguyên độ lớn
            rb.AddForce(pushDirection.normalized * pushForce, ForceMode2D.Impulse);
        }
    }
}

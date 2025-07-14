using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Phát âm thanh coin
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX("Coin");
            }
            
            //Tăng số lượng xu ở đây
            if (GameManager.Instance != null)
            {
                GameManager.Instance.coins++;
            }
            
            gameObject.SetActive(false);
        }
    }
}

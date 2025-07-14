using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{
    public delegate void PowerUp(string namePowerUp);
    public event PowerUp powerUpEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PowerUp"))
        {
            // Phát SFX pickup khi nhặt PowerUp
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX("PickUp");
            }
            
            powerUpEvent?.Invoke(collision.gameObject.name);
            Debug.Log(collision.gameObject.name);
        }
    }
}

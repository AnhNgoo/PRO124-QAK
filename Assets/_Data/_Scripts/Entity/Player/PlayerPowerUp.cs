using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PowerUp"))
        {
            // Phát SFX pickup khi nhặt PowerUp
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX("PickUp");
            }

            PowerUpManager.Instance.ActivePowerUp(collision.gameObject.name, this.gameObject);
            Debug.Log(collision.gameObject.name);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public delegate void PlayerDeath();
    public event PlayerDeath deathEvent;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Kill"))
        {
            deathEvent?.Invoke();
        }
    }
}

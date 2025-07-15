using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private GameObject smoke;
    private ParticleSystem smokeParticle;

    public delegate void PlayerDeathDelegate();
    public event PlayerDeathDelegate deathEvent;
    private void Start()
    {
        GetComponent();
        smoke.SetActive(false);
        deathEvent += Death;
    }
    private void GetComponent()
    {
        smoke = GameObject.Find("Smoke");
        smokeParticle = smoke.GetComponent<ParticleSystem>();
    }

    public void Death()
    {
        DistanceTracker.Instance.isStopped = true; // Dừng theo dõi khoảng cách khi người chơi chết
        smoke.SetActive(true);
        smoke.transform.position = transform.position;

        smokeParticle.Play();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PowerUpManager.Instance.shield.isActive) return;
        if (collision.CompareTag("Obstacle"))
        {
            deathEvent?.Invoke();
        }
    }
}

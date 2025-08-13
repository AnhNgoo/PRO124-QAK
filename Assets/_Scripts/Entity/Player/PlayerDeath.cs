using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDeath : MonoBehaviour
{
    public string playerSmokeName; //Đặt tên smoke lại khi bị lỗi
    private GameObject smoke;
    private ParticleSystem smokeParticle;

    public bool isActiveShield { get; set; } = false;

    private void Start()
    {
        GetComponent();
        smoke.SetActive(false);
    }
    private void GetComponent()
    {
        smoke = GameObject.Find(playerSmokeName);
        smokeParticle = smoke.GetComponent<ParticleSystem>();
    }

    public void Death()
    {
        smoke.SetActive(true);
        smoke.transform.position = transform.position;

        smokeParticle.Play();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActiveShield) return;
        if (collision.CompareTag("Obstacle"))
        {
            GameManager.Instance.playerCount++;
            Death();
            GameManager.Instance.OnPlayerDeath(gameObject);
        }
    }
}

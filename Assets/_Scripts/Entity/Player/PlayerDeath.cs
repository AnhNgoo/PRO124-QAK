using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private GameObject smoke;
    private ParticleSystem smokeParticle;
    private PlayerController playerController;
    private void Start()
    {
        GetComponent();
        smoke.SetActive(false);
        playerController.playerStatus.deathEvent += Death;
    }
    private void GetComponent()
    {
        playerController = GetComponent<PlayerController>();
        smoke = GameObject.Find("Smoke");
        smokeParticle = smoke.GetComponent<ParticleSystem>();
    }

    public void Death()
    {
        smoke.SetActive(true);
        smoke.transform.position = transform.position;

        smokeParticle.Play();
        gameObject.SetActive(false);
    }
}

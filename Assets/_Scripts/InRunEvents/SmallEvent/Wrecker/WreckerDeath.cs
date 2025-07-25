using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckerDeath : MonoBehaviour
{
    public GameObject wrecker;
    private GameObject smoke;
    private ParticleSystem smokeParticle;


    private void Start()
    {
        GetComponent();
    }
    private void GetComponent()
    {
        smoke = GameObject.Find("WreckerSmoke");
        smokeParticle = smoke.GetComponent<ParticleSystem>();
    }

    public void Death()
    {
        smoke.transform.position = transform.position;
        smokeParticle.Play();
        wrecker.SetActive(false);
        Debug.Log("Wrecker has died");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Debug.Log("Wrecker collided with an obstacle");
            Death();
        }
    }
}

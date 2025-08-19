using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletWrecker : MonoBehaviour
{
    public float speed = 10f; // Tốc độ di chuyển của bullet
    private GameObject blindSmoke;
    private ParticleSystem blindSmokeParticle;

    private Vector3 shootDirection;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        blindSmoke = GameObject.Find("BlindSmoke");
        blindSmokeParticle = blindSmoke.GetComponent<ParticleSystem>();
    }
    public void Init(Vector3 shootDirection)
    {
        this.shootDirection = shootDirection;
    }

    private void Update()
    {
        rb.velocity = shootDirection * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            blindSmoke.SetActive(true);
            blindSmoke.transform.position = collision.transform.position;

            blindSmokeParticle.Play();

        }
    }
}

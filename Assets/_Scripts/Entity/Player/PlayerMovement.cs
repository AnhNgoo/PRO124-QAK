using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float flyForce = 15f;
    public float maxVerticalSpeed = 8f;
    public float gravityMultiplier = 2f;

    private Rigidbody2D rb;
    private ParticleSystem jetpackParticle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jetpackParticle = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        HandleGravityBoost();
    }

    void FixedUpdate()
    {
        HandleFlying();
    }

    void HandleFlying()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (rb.velocity.y < maxVerticalSpeed)
            {
                rb.AddForce(Vector2.up * flyForce, ForceMode2D.Force);
                Debug.Log("Flying with force: " + flyForce);
            }

            // Bật hiệu ứng
           
                jetpackParticle.Play();
        }
        else
        {
         
                jetpackParticle.Stop();
        }
    }

    void HandleGravityBoost()
    {
        if (!Input.GetKey(KeyCode.Space) && rb.velocity.y > 0)
        {
            rb.velocity += Vector2.down * gravityMultiplier * Time.deltaTime;
        }
    }
}

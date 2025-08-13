using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float flyForce = 15f;
    public float maxVerticalSpeed = 8f;
    public float gravityMultiplier = 2f;
    public KeyCode flyKey = KeyCode.Space;

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
        if (CutSceneBlocker.Instance.isCutSceneActive) return;

        if (Input.GetKey(flyKey))
        {
            if (rb.velocity.y < maxVerticalSpeed)
            {
                rb.AddForce(Vector2.up * flyForce, ForceMode2D.Force);
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
        if (CutSceneBlocker.Instance.isCutSceneActive) return;
        if (!Input.GetKey(KeyCode.Space) || !Input.GetMouseButton(0) && rb.velocity.y > 0)
        {
            rb.velocity += Vector2.down * gravityMultiplier * Time.deltaTime;
        }
    }
}

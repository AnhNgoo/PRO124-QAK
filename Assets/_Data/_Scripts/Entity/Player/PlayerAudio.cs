using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Footstep Settings")]
    public float footstepInterval = 0.5f;
    
    private bool wasGrounded = false;
    private bool wasMoving = false;
    private float footstepTimer = 0f;
    private PlayerController playerController;
    private CheckGround checkGround;
    private Rigidbody2D rb;
    
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        checkGround = GetComponent<CheckGround>();
        rb = GetComponent<Rigidbody2D>();
        footstepTimer = 0f;
    }
    
    void Update()
    {
        HandleFootsteps();
    }
    
    void HandleFootsteps()
    {
        bool isGrounded = checkGround.Check();
        bool isMoving = Mathf.Abs(rb.velocity.x) > 0.1f;
        
        // Nếu nhân vật đang ở dưới mặt đất và đang di chuyển
        if (isGrounded && isMoving)
        {
            footstepTimer += Time.deltaTime;
            
            // Phát âm thanh footstep theo interval
            if (footstepTimer >= footstepInterval)
            {
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlaySFX("FootStep");
                }
                footstepTimer = 0f;
            }
        }
        else
        {
            // Reset timer khi không di chuyển hoặc không ở trên mặt đất
            footstepTimer = 0f;
        }
        
        wasGrounded = isGrounded;
        wasMoving = isMoving;
    }
}

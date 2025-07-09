using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerMovement playerMovement { get; private set; }
    public PlayerAnimation playerAnimation { get; private set; }
    public PlayerDeath playerDeath { get; set; }
    public PlayerPowerUp playerPowerUp { get; private set; }
    public CheckGround checkGround { get; private set; }
    void Start()
    {
        GetComponent();
    }

    private void GetComponent()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerDeath = GetComponent<PlayerDeath>();
        playerPowerUp = GetComponent<PlayerPowerUp>();
        checkGround = GetComponent<CheckGround>();
    }
}

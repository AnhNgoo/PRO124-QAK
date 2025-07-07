using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerMovement playerMovement { get; private set; }
    public PlayerAnimation playerAnimation { get; private set; }
    public PlayerDeath playerDeath { get; private set; }
    public PlayerStatus playerStatus { get; private set; }
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
        playerStatus = GetComponent<PlayerStatus>();
        checkGround = GetComponent<CheckGround>();
    }
}

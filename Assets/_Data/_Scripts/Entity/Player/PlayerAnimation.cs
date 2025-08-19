using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public enum PlayerState
    {
        Run,
        Fly
    }

    private Animator anim;
    private PlayerController playerController;
    private PlayerState currentState = PlayerState.Run;

    private void Start()
    {
        ChangeState(PlayerState.Run);
        GetComponent();
    }

    private void Update()
    {
        UpdateState();
    }

    private void GetComponent()
    {
        anim = gameObject.transform.Find("Skin").GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    private void UpdateState()
    {
        if (playerController.checkGround.Check())
        {
            ChangeState(PlayerState.Run);
        }
        else
        {
            ChangeState(PlayerState.Fly);
        }
    }

    private void ChangeState(PlayerState newState)
    {
        if (currentState == newState)
            return;
        currentState = newState;

        switch (newState)
        {
            case PlayerState.Run:
                anim.CrossFade("Run", 0.1f);
                break;
            case PlayerState.Fly:
                anim.CrossFade("Fly", 0.1f);
                break;

        }
    }
}

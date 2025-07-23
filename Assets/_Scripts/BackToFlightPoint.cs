using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToFlightPoint : MonoBehaviour
{
    public float timeStartBack = 0.5f;
    public float backDuration = 0.5f;

    private Transform player;
    private bool isPlayerBack = false;

    void Start()
    {
        InitComponents();
    }

    void InitComponents()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (!isPlayerBack && Mathf.Abs(player.position.x - transform.position.x) > 0.1f)
        {
            isPlayerBack = true;
            Invoke("PlayerBack", timeStartBack);
        }

    }


    private void PlayerBack()
    {
        player.DOMoveX(transform.position.x, backDuration)
               .SetEase(Ease.OutBack)
               .OnComplete(() => isPlayerBack = false);
    }
}

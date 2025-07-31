using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BackToFlightPoint : MonoBehaviour
{
    public float timeStartBack = 0.5f;
    public float backDuration = 0.5f;

    private Transform[] player;
    private bool isPlayerBack = false;

    void Start()
    {
        InitComponents();
    }

    void InitComponents()
    {
        player = GameObject.FindGameObjectsWithTag("Player").Select(p => p.transform).ToArray();
    }

    private void Update()
    {
        if (!isPlayerBack && player.Any(p => Mathf.Abs(p.position.x - transform.position.x) > 0.1f))
        {
            isPlayerBack = true;
            Invoke("PlayerBack", timeStartBack);
        }
    }


    private void PlayerBack()
    {
        StartCoroutine(MoveAllPlayersBack());
    }

    private IEnumerator MoveAllPlayersBack()
    {
        int completed = 0;
        isPlayerBack = true; // Khóa luôn khi bắt đầu

        foreach (var p in player)
        {
            p.DOMoveX(transform.position.x, backDuration)
             .SetEase(Ease.OutBack)
             .OnComplete(() =>
             {
                 completed++;
             });
        }

        // Đợi đến khi tất cả player hoàn thành
        yield return new WaitUntil(() => completed >= player.Length);
        isPlayerBack = false;
    }

}

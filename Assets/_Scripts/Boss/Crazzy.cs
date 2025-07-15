using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crazzy : MonoBehaviour
{
    public Transform crazzy;
    public Transform target;
    public Transform bullets;
    public Transform firePoint;

    private PlayerController playerController;
    private Transform player;
    private List<GameObject> bulletList = new();
    private bool isAttacking = false;
    private bool isFollowingY = false;
    private bool isStopped = false;

    void OnEnable()
    {
        GetComponent();
        StartMove();
    }

    void OnDestroy()
    {
        DOTween.Kill(this);
        DOTween.Kill(gameObject);
        DOTween.Kill(transform);
    }
    void Start()
    {
        AddBulletListToPool();
    }
    private void AddBulletListToPool()
    {
        foreach (Transform bullet in bullets)
        {
            bulletList.Add(bullet.gameObject);
            bullet.gameObject.SetActive(false);
        }
        ObjectPooler.Instance.Add("BulletCrazzy", bulletList);
    }
    private void GetComponent()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();
        playerController.playerDeath.deathEvent += Stop;
    }

    private void StartMove()
    {
        if (isStopped) return;
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() =>
        {
            crazzy.position = transform.position;
        });

        sequence.Append(crazzy
                            .DOMove(target.position, 2f)
                            .SetEase(Ease.OutBack));

        sequence.AppendInterval(1);

        sequence.AppendCallback(() =>
        {
            StartCoroutine(Attack());
        });
        sequence.JoinCallback(() =>
        {
            StartCoroutine(FollowY());
        });

        sequence.AppendInterval(16f);

        sequence.AppendCallback(() =>
        {
            isFollowingY = false;
            isAttacking = false;
        });
        sequence.AppendInterval(1f);

        sequence.AppendCallback(() =>
        {
            crazzy.DOKill();
        });
        sequence.Append(crazzy
                           .DOMove(target.position, 2f)
                           .SetEase(Ease.OutBack));

        sequence.Append(crazzy
                           .DOMove(transform.position, 1f)
                           .SetEase(Ease.InBack));
        sequence.AppendInterval(1f);
        sequence.AppendCallback(() =>
        {
            InRunEventsManager.Instance.obstacleBlock.SetActive(false);
            gameObject.SetActive(false);
            InRunEventsManager.Instance.isBigEventActive = false;
        });
    }


    IEnumerator Attack()
    {
        for (int i = 0; i < 10; i++)
        {
            isAttacking = true;
            isFollowingY = true;
            crazzy
                 .DOMoveY(player.position.y, 2f)
                 .SetEase(Ease.OutBack);
            yield return new WaitForSeconds(0.5f);
            // Lấy một viên đạn từ pool
            GameObject bullet = ObjectPooler.Instance.SpawnFromPool("BulletCrazzy", firePoint.position, Quaternion.identity, 3);

            yield return new WaitForSeconds(0.1f);
            isAttacking = false;
            yield return new WaitForSeconds(1f); // Thời gian giữa các lần bắn
        }

        isFollowingY = false;

    }
    IEnumerator FollowY()
    {
        while (isFollowingY)
        {
            if (isAttacking) yield return null;
            crazzy
                 .DOMoveY(player.position.y, 2f)
                 .SetEase(Ease.OutBack);
            yield return new WaitForSeconds(1f);
        }
    }

    private void Stop()
    {
        Sequence sequence = DOTween.Sequence();


        StopAllCoroutines();
        sequence.AppendCallback(() =>
        {
            isStopped = true;
            isAttacking = false;
            isFollowingY = false;
            crazzy.DOKill();
        });
        sequence.Append(crazzy
                           .DOMove(target.position, 2f)
                           .SetEase(Ease.OutBack));

        sequence.Append(crazzy
                           .DOMove(transform.position, 1f)
                           .SetEase(Ease.InBack));
        sequence.AppendInterval(1f);
        sequence.AppendCallback(() =>
        {
            InRunEventsManager.Instance.obstacleBlock.SetActive(false);
            gameObject.SetActive(false);
        });
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Quản lý hành vi của boss Crazzy <br/>
/// AddBulletListToPool: thêm đạn vào ObjectPooling <br/>
/// StartMove: bắt đầu di chuyển <br/>
/// Attack: tấn công <br/>
/// FollowY: theo dõi trục Y
/// </summary>
public class Crazzy : MonoBehaviour
{
    public Transform crazzy;
    public Transform target;
    public Transform bullets;
    public Transform firePoint;

    private List<Transform> player = new();
    private List<GameObject> bulletList = new();
    private bool isAttacking = false;
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

    /// <summary>
    /// Thêm đạn vào bulletList từ con của bullets <br/>
    /// Add bulletList vào ObjectPooler
    /// </summary>
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
        player = GameObject.FindGameObjectsWithTag("Player").Select(x => x.transform).ToList();
        GameEvent.Instance.RegisterEvent("PlayerDeath", Stop);
    }

    /// <summary>
    /// Cho crazzy di chuyển đến vị trí của target
    /// Chạy song song Attack và FollowY để tấn công và di chuyển theo trục Y với player
    /// </summary>
    private void StartMove()
    {
        if (isStopped) return;
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() =>
        {
            crazzy.position = transform.position;

            // Phát âm thanh cảnh báo boss
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX("WarmingBoss");
            }
        });

        //Di chuyển đến vị trí của target
        sequence.Append(crazzy
                            .DOMove(target.position, 2f)
                            .SetEase(Ease.OutBack));

        sequence.AppendInterval(1);

        sequence.AppendCallback(() =>
        {
            // Dừng nhạc trong game và phát nhạc boss
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.StopMusic();
                AudioManager.Instance.PlayMusic("BossMusic");
            }

            StartCoroutine(Attack());
        });
        sequence.JoinCallback(() =>
        {
            StartCoroutine(FollowY());
        });

        sequence.AppendInterval(16f);

        sequence.AppendCallback(() => isAttacking = false);
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

    /// <summary>
    /// Crazzy sẽ tấn công kẻ địch 10 lần <br/>
    /// isAttacking false sẽ giúp dừng tấn công để gọi FollowY trong 1 khoảng thời gian
    /// </summary>
    IEnumerator Attack()
    {
        for (int i = 0; i < 10; i++)
        {
            isAttacking = true;

            int playerIndex = Random.Range(0, player.Count);
            crazzy
                 .DOMoveY(player[playerIndex].position.y, 2f)
                 .SetEase(Ease.OutBack);

            yield return new WaitForSeconds(0.5f);
            // Lấy một viên đạn từ pool
            GameObject bullet = ObjectPooler.Instance.SpawnFromPool("BulletCrazzy", firePoint.position, Quaternion.identity, 3);

            yield return new WaitForSeconds(0.1f);
            isAttacking = false;
            yield return new WaitForSeconds(1f); // Thời gian giữa các lần bắn
        }
        isAttacking = true;
    }

    /// <summary>
    /// Crazzy sẽ di chuyển theo trục Y với player khi isAttacking là false
    /// </summary>
    IEnumerator FollowY()
    {
        while (true)
        {
            if (isAttacking) yield return null;
            int playerIndex = Random.Range(0, player.Count);
            crazzy
                 .DOMoveY(player[playerIndex].position.y, 2f)
                 .SetEase(Ease.OutBack);
            yield return new WaitForSeconds(1f);
        }
    }

    /// <summary>
    /// Hàm này được đăng ký sự kiện khi playerdeath <br/>
    /// Khi player chết, Crazzy sẽ dừng tất cả các hành động và trở về vị trí ban đầu
    /// </summary>
    private void Stop()
    {
        Sequence sequence = DOTween.Sequence();

        StopAllCoroutines();
        sequence.AppendCallback(() =>
        {
            isStopped = true;
            isAttacking = false;
            crazzy.DOKill();
        });
        sequence.Append(crazzy
                           .DOMove(target.position, 2f)
                           .SetEase(Ease.OutBack));

        sequence.Append(crazzy
                           .DOMove(transform.position, 1f)
                           .SetEase(Ease.InBack));

        // Phát lại nhạc trong game ngay lập tức khi boss bắt đầu rời đi
        sequence.AppendCallback(() =>
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.StopMusic();
                AudioManager.Instance.PlayMusic("InGame");
            }
        });

        sequence.AppendInterval(1f);
        sequence.AppendCallback(() =>
        {
            InRunEventsManager.Instance.obstacleBlock.SetActive(false);
            gameObject.SetActive(false);
        });
    }
}

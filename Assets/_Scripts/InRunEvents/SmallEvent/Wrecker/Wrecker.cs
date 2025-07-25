using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Wrecker : MonoBehaviour
{
    public GameObject wrecker;
    public GameObject bulletWreckerGameobject;
    public float raycastFindSafeFlightDistance = 100f;
    public float raycastWreckerDistance = 50f; // Khoảng cách raycast để kiểm tra obstacle

    public LayerMask obstacleLayerMask;
    public List<Transform> flightPoints;

    [Header("Attack Settings")]
    public int attackCount = 3;
    public float attackInterval = 2f; // Thời gian giữa các lần attack
    public float attackDuration = 0.5f; // Tốc độ di chuyển khi attack
    public float returnSpeed = 1f; // Tốc độ quay về

    private GameObject player;
    private bool isMoving = false;
    private bool isObstacleNearWrecker = false; // Biến kiểm tra có obstacle gần player hay không
    private bool isStopped = false;
    private Vector3 safePosition; // Lưu vị trí an toàn hiện tại
    private BulletWrecker bulletWrecker;
    private PlayerController playerController;

    void OnEnable()
    {
        GetComponent();
        StartMove();
    }

    private void Update()
    {
        CheckObstacleNearWrecker();
        StopOnMajorEvent();
    }

    void GetComponent()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bulletWrecker = bulletWreckerGameobject.GetComponent<BulletWrecker>();
        if (player == null) return;
        playerController = player.GetComponent<PlayerController>();
        playerController.playerDeath.deathEvent += Stop;
    }

    private void StopOnMajorEvent()
    {
        if (InRunEventsManager.Instance.isBigEventActive) Stop();
    }
    private void StartMove()
    {
        if (isStopped) return;
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            bulletWreckerGameobject.SetActive(false);
            wrecker.transform.position = transform.position;
            Transform safeFlightPoint = FindSafeFlight();
            if (safeFlightPoint != null)
            {
                safePosition = safeFlightPoint.position;
                wrecker.transform.DOMove(safePosition, 1f)
                    .SetEase(Ease.OutBack);
            }
        });

        sequence.AppendCallback(() =>
        {
            StartCoroutine(MainBehaviorWithCallback());
        });

    }

    private IEnumerator MainBehaviorWithCallback()
    {
        int attackDone = 0;
        while (attackDone < attackCount)
        {
            yield return StartCoroutine(AvoidancePhase());
            if (!isObstacleNearWrecker)
            {
                yield return StartCoroutine(AttackPhase());
                attackDone++;
            }
            else
            {
                Debug.Log("Bỏ qua attack vì có obstacle, không tăng attackDone");
            }
        }

        isMoving = false;
        StartExitSequence();
    }


    private void StartExitSequence()
    {
        Debug.Log("Bắt đầu thoát");
        Sequence exitSequence = DOTween.Sequence();

        exitSequence.Append(wrecker.transform.DOMove(transform.position, 1f)
                                             .SetEase(Ease.InBack));

        exitSequence.AppendInterval(1f);

        exitSequence.AppendCallback(() =>
        {
            gameObject.SetActive(false);
        });
    }



    private IEnumerator AvoidancePhase()
    {
        isMoving = true;

        float phaseTime = 0f;
        while (phaseTime < attackInterval)
        {
            DetectAndAvoidObstacle();
            yield return new WaitForSeconds(0.1f);
            phaseTime += 0.1f;
        }
    }

    private IEnumerator AttackPhase()
    {
        if (isObstacleNearWrecker)
            yield break; // Không attack nếu có obstacle gần player
        bulletWreckerGameobject.SetActive(false);
        Vector3 shootDirection = (player.transform.position - wrecker.transform.position).normalized;
        bulletWreckerGameobject.SetActive(true); // Bật bullet khi bắt đầu attack
        bulletWrecker.transform.position = wrecker.transform.position;
        bulletWrecker.Init(shootDirection);

        yield return new WaitForSeconds(attackDuration);
    }

    private void CheckObstacleNearWrecker()
    {

        RaycastHit2D hit = Physics2D.Raycast(wrecker.transform.position, Vector2.right, 20, obstacleLayerMask);

        if (hit.collider != null && hit.collider.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Khi true");
            isObstacleNearWrecker = true;
        }
        else
        {
            Debug.Log("Khi false");
            isObstacleNearWrecker = false;
        }

    }

    private void DetectAndAvoidObstacle()
    {
        if (!isMoving) return; // Không né obstacle khi đang attack

        RaycastHit2D hit = Physics2D.Raycast(wrecker.transform.position, Vector2.right, raycastWreckerDistance);

        if (hit.collider != null && hit.collider.gameObject.CompareTag("Obstacle"))
        {
            Transform safeFlightPoint = FindSafeFlight();
            if (safeFlightPoint != null &&
                Vector3.Distance(wrecker.transform.position, safeFlightPoint.position) > 0.5f)
            {
                safePosition = safeFlightPoint.position;
                wrecker.transform.DOMove(safePosition, 0.8f)
                    .SetEase(Ease.OutQuad);
            }
        }
    }

    private Transform FindSafeFlight()
    {
        foreach (var flightPoint in flightPoints)
        {
            RaycastHit2D hit = Physics2D.Raycast(flightPoint.position, Vector2.right, raycastFindSafeFlightDistance);
            if (hit.collider == null || !hit.collider.gameObject.CompareTag("Obstacle"))
                return flightPoint;
        }
        return null;
    }

    void Stop()
    {
        StopAllCoroutines();
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() =>
        {
            isStopped = true;
            isMoving = false;
            wrecker.transform.DOKill();
        });

        sequence.Append(wrecker.transform.DOMove(transform.position, 2f)
                                             .SetEase(Ease.InBack));

        sequence.AppendInterval(1f);
        sequence.AppendCallback(() =>
        {
            bulletWreckerGameobject.SetActive(false);
            gameObject.SetActive(false);
        });
    }

    private void OnDrawGizmos()
    {
        if (flightPoints == null) return;

        // 1. Vẽ flight points với raycast detection
        foreach (var flightPoint in flightPoints)
        {
            if (flightPoint == null) continue;

            RaycastHit2D hit = Physics2D.Raycast(flightPoint.position, Vector2.right, raycastFindSafeFlightDistance, obstacleLayerMask);

            if (hit.collider == null || !hit.collider.gameObject.CompareTag("Obstacle"))
            {
                // Flight point an toàn
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(flightPoint.position, 0.3f);
                Gizmos.DrawRay(flightPoint.position, Vector2.right * raycastFindSafeFlightDistance);
            }
            else
            {
                // Flight point có obstacle
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(flightPoint.position, 0.3f);
                Gizmos.DrawLine(flightPoint.position, hit.point);

                // Vẽ obstacle hit point
                Gizmos.color = Color.gray;
                Gizmos.DrawWireCube(hit.point, Vector3.one * 0.4f);
            }
        }

        // 2. Vẽ wrecker và raycast detection
        if (wrecker != null)
        {
            // Vẽ wrecker với màu theo trạng thái
            Gizmos.color = isMoving ? Color.green : Color.blue;
            Gizmos.DrawWireSphere(wrecker.transform.position, 0.6f);

            // Vẽ raycast từ wrecker để detect obstacle
            RaycastHit2D wreckerHit = Physics2D.Raycast(wrecker.transform.position, Vector2.right, raycastWreckerDistance, obstacleLayerMask);

            if (wreckerHit.collider != null && wreckerHit.collider.gameObject.CompareTag("Obstacle"))
            {
                // Wrecker detect obstacle
                Gizmos.color = Color.red;
                Gizmos.DrawLine(wrecker.transform.position, wreckerHit.point);

                // Vẽ obstacle detection point
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(wreckerHit.point, Vector3.one * 0.3f);
            }
            else
            {
                // Wrecker không detect obstacle
                Gizmos.color = Color.cyan;
                Gizmos.DrawRay(wrecker.transform.position, Vector2.right * raycastWreckerDistance);
            }

            // Vẽ safe position hiện tại
            if (safePosition != Vector3.zero)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(safePosition, 0.4f);

                // Vẽ line từ wrecker đến safe position
                Gizmos.color = Color.white;
                Gizmos.DrawLine(wrecker.transform.position, safePosition);
            }
        }

        // 3. Vẽ wrecker detection raycast (CheckObstacleNearWrecker)
        if (wrecker != null)
        {
            RaycastHit2D wreckerDetectionHit = Physics2D.Raycast(wrecker.transform.position, Vector2.right, 50, obstacleLayerMask);

            if (wreckerDetectionHit.collider != null && wreckerDetectionHit.collider.gameObject.CompareTag("Obstacle"))
            {
                // Wrecker detect obstacle trong range 50
                Gizmos.color = Color.red;
                Gizmos.DrawLine(wrecker.transform.position, wreckerDetectionHit.point);

                // Vẽ detection hit point
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(wreckerDetectionHit.point, 0.2f);
            }
            else
            {
                // Không detect obstacle
                Gizmos.color = new Color(0, 1, 1, 0.5f); // Cyan trong suốt
                Gizmos.DrawRay(wrecker.transform.position, Vector2.right * 50);
            }
        }

        // 4. Vẽ player và raycast (cho tham khảo)
        if (player != null)
        {
            // Vẽ player với màu theo trạng thái isObstacleNearPlayer
            Gizmos.color = isObstacleNearWrecker ? Color.red : Color.green;
            Gizmos.DrawWireSphere(player.transform.position, 0.5f);

            // Vẽ raycast từ player để tham khảo
            RaycastHit2D playerHit = Physics2D.Raycast(player.transform.position, Vector2.right, 50, obstacleLayerMask);

            if (playerHit.collider != null && playerHit.collider.gameObject.CompareTag("Obstacle"))
            {
                Gizmos.color = new Color(1, 0, 1, 0.7f); // Magenta trong suốt
                Gizmos.DrawLine(player.transform.position, playerHit.point);
            }
            else
            {
                Gizmos.color = new Color(0, 1, 1, 0.3f); // Cyan rất trong suốt
                Gizmos.DrawRay(player.transform.position, Vector2.right * 50);
            }
        }

        // 5. Vẽ bullet wrecker nếu đang active
        if (bulletWreckerGameobject != null && bulletWreckerGameobject.activeInHierarchy)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(bulletWreckerGameobject.transform.position, 0.3f);

            // Vẽ hướng bắn bullet
            if (player != null && wrecker != null)
            {
                Vector3 shootDirection = (player.transform.position - wrecker.transform.position).normalized;
                Gizmos.color = Color.red;
                Gizmos.DrawRay(bulletWreckerGameobject.transform.position, shootDirection * 10f);
            }
        }

        // 6. Vẽ thông tin debug text (tùy chọn)
        if (wrecker != null)
        {
            // Có thể thêm Handles.Label để hiển thị text debug
#if UNITY_EDITOR
            UnityEditor.Handles.color = Color.white;
            UnityEditor.Handles.Label(wrecker.transform.position + Vector3.up * 1f,
                $"Moving: {isMoving}\nObstacle Near: {isObstacleNearWrecker}");
#endif
        }
    }
}
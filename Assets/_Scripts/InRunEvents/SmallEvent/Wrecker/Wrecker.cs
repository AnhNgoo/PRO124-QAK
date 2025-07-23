using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Wrecker : MonoBehaviour
{
    public GameObject wrecker;
    public float raycastFindSafeFlightDistance = 100f;
    public float raycastWreckerDistance = 50f; // Khoảng cách raycast để kiểm tra obstacle

    public LayerMask obstacleLayerMask;
    public List<Transform> flightPoints;

    [Header("Attack Settings")]
    public int attackCount = 3;
    public float attackInterval = 2f; // Thời gian giữa các lần attack
    public float attackSpeed = 0.5f; // Tốc độ di chuyển khi attack
    public float returnSpeed = 1f; // Tốc độ quay về

    private Transform player;
    private bool isMoving = false;
    private bool isObstacleNearPlayer = false; // Biến kiểm tra có obstacle gần player hay không
    private Vector3 safePosition; // Lưu vị trí an toàn hiện tại

    void OnEnable()
    {
        GetComponent();
        StartMove();
    }

    private void Update()
    {
        CheckObstacleNearPlayer();
    }

    void GetComponent()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void StartMove()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
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
            StartCoroutine(MainBehavior());
        });

        sequence.AppendInterval(20f);

        sequence.AppendCallback(() =>
        {
            isMoving = false;
        });

        sequence.Append(wrecker.transform.DOMove(transform.position, 1)
                                          .SetEase(Ease.InBack));

        sequence.AppendInterval(1f);

        sequence.AppendCallback(() =>
        {
            gameObject.SetActive(false); // Tắt Wrecker sau khi hoàn thành hành vi
        });


    }

    private IEnumerator MainBehavior()
    {
        for (int i = 0; i < attackCount; i++)
        {
            // Trạng thái 1: Di chuyển né obstacle trong 2 giây
            yield return StartCoroutine(AvoidancePhase());

            // Trạng thái 2: Attack player rồi quay về
            yield return StartCoroutine(AttackPhase());
        }
        isMoving = false; // Kết thúc hành vi chính
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
        if (isObstacleNearPlayer)
        {
            Debug.Log("Bỏ qua");
            yield break; // Không attack nếu có obstacle gần player
        }

        isMoving = false;

        // Lưu vị trí hiện tại trước khi attack
        Vector3 currentSafePos = wrecker.transform.position;

        // Di chuyển đến player
        wrecker.transform.DOMove(player.position, attackSpeed)
            .SetEase(Ease.InOutBack);

        yield return new WaitForSeconds(attackSpeed);

        // Tìm vị trí an toàn mới
        Transform newSafePoint = FindSafeFlight();
        if (newSafePoint != null)
        {
            safePosition = newSafePoint.position;
        }
        else
        {
            // Nếu không tìm thấy điểm an toàn mới, quay về vị trí cũ
            safePosition = currentSafePos;
        }

        // Quay về vị trí an toàn
        wrecker.transform.DOMove(safePosition, returnSpeed)
            .SetEase(Ease.OutBack);

        yield return new WaitForSeconds(returnSpeed);

    }

    private void CheckObstacleNearPlayer()
    {

        RaycastHit2D hit = Physics2D.Raycast(player.position, Vector2.right, 50, obstacleLayerMask);

        if (hit.collider != null && hit.collider.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Khi true");
            isObstacleNearPlayer = true;
        }
        else
        {
            Debug.Log("Khi false");
            isObstacleNearPlayer = false;
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

    private void OnDrawGizmos()
    {
        if (flightPoints == null) return;

        // Vẽ flight points với raycast check obstacle
        foreach (var flightPoint in flightPoints)
        {
            if (flightPoint == null) continue;

            RaycastHit2D hit = Physics2D.Raycast(flightPoint.position, Vector2.right, raycastFindSafeFlightDistance, obstacleLayerMask);

            if (hit.collider == null || !hit.collider.gameObject.CompareTag("Obstacle"))
            {
                Gizmos.color = Color.green; // An toàn
                                            // Vẽ full distance nếu không có obstacle
                Gizmos.DrawRay(flightPoint.position, Vector2.right * raycastFindSafeFlightDistance);
            }
            else
            {
                Gizmos.color = Color.red; // Có vật cản
                                          // Vẽ đến điểm hit
                Gizmos.DrawLine(flightPoint.position, hit.point);
                // Vẽ điểm hit
                Gizmos.DrawWireSphere(hit.point, 0.2f);
            }
        }

        // Vẽ trạng thái wrecker
        if (wrecker != null)
        {
            Gizmos.color = isMoving ? Color.red : Color.blue;
            Gizmos.DrawWireSphere(wrecker.transform.position, 0.5f);

            // Vẽ raycast từ wrecker với LayerMask và distance đúng
            RaycastHit2D wreckerHit = Physics2D.Raycast(wrecker.transform.position, Vector2.right, raycastWreckerDistance, obstacleLayerMask);

            if (wreckerHit.collider != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(wrecker.transform.position, wreckerHit.point);
                Gizmos.DrawWireSphere(wreckerHit.point, 0.2f);
            }
            else
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(wrecker.transform.position, Vector2.right * raycastWreckerDistance);
            }
        }

        // Vẽ raycast check obstacle near player
        if (player != null)
        {
            RaycastHit2D playerHit = Physics2D.Raycast(player.position, Vector2.right, 50, obstacleLayerMask);

            if (playerHit.collider != null && playerHit.collider.gameObject.CompareTag("Obstacle"))
            {
                // Có obstacle gần player
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(player.position, playerHit.point);

                // Vẽ điểm hit
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(playerHit.point, 0.3f);
            }
            else
            {
                // Không có obstacle gần player
                Gizmos.color = Color.cyan;
                Gizmos.DrawRay(player.position, Vector2.right * 50);
            }

            // Vẽ điểm player
            Gizmos.color = isObstacleNearPlayer ? Color.red : Color.green;
            Gizmos.DrawWireSphere(player.position, 0.4f);
        }
    }
}
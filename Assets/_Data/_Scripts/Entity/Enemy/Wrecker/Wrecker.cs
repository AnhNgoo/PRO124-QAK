using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

/// <summary>
/// -Khi Wrecker được kích hoạt, gọi startmove để wrecker di chuyển <br/>
/// -StartMove: <br/>
/// +đợi 3 giây <br/>
/// +gọi FindSafeFlight để tìm vùng an toàn <br/>
/// +gọi MainBehaviorWithCallback để thực hiện các hành vi như né, tấn công <br/>
/// -MainBehavior:
/// +Nếu ở gần wrecker có vật cản thì bỏ qua việc tấn công mà đi né vật cản <br/>
/// +Nếu không có vật cản thì random 1 player và tính toán hướng bắn rồi bắn về phía player <br/>
/// +Nếu xong hành vi thì gọi StartExitSequence để di chuyển wrecker về <br/>
/// -Stop: Nếu player chết thì gọi Stop
/// </summary>
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

    private List<GameObject> player = new();
    private bool isMoving = false;
    private bool isObstacleNearWrecker = false; // Biến kiểm tra có obstacle gần wrecker hay không
    private bool isStopped = false;
    private Vector3 safePosition; // Vị trí an toàn để di chuyển đến
    private BulletWrecker bulletWrecker;
    private Collider2D wreckerCollider;

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
        player = GameObject.FindGameObjectsWithTag("Player").Select(x => x).ToList();
        bulletWrecker = bulletWreckerGameobject.GetComponent<BulletWrecker>();
        wreckerCollider = wrecker.GetComponent<Collider2D>();
        GameEvent.Instance.RegisterEvent("PlayerDeath", Stop);
    }

    private void StopOnMajorEvent()
    {
        if (InRunEventsManager.Instance.isBigEventActive) Stop();
    }

    /// <summary>
    /// Đợi 3s, sau đó tìm vị trí an toàn để di chuyển đến, sau đó gọi hàm thực hiện hành vi chính
    /// </summary>
    private void StartMove()
    {
        if (isStopped) return;
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() =>
        {
            InRunEventsManager.Instance.obstacleBlock.SetActive(true);
            bulletWreckerGameobject.SetActive(false);
            wrecker.transform.position = transform.position;
            wreckerCollider.enabled = false;
        });

        //Đợi 3s
        sequence.AppendInterval(3f);

        //Tìm vị trí an toàn và di chuyển đến đó
        sequence.AppendCallback(() =>
        {
            Transform safeFlightPoint = FindSafeFlight();

            if (safeFlightPoint != null)
            {
                safePosition = safeFlightPoint.position;
                wrecker.transform.DOMove(safePosition, 1f)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() =>
                    {
                        InRunEventsManager.Instance.obstacleBlock.SetActive(false);
                        wreckerCollider.enabled = true;
                    });
            }
        });

        //Gọi MainBehaviorWithCallback để thực hiện các hành vi như né, tấn công
        sequence.AppendCallback(() =>
        {
            StartCoroutine(MainBehaviorWithCallback());
        });

    }

    /// <summary>
    /// Bắt đầu hành vi né và tấn công <br/>
    /// Khi vừa né xong, nếu trước mặt wrecker không có obstacle thì thực hiện attack (isObstacleNearWrecker = false) và cộng attackDone
    /// Nếu có vật cản thì lặp lại và không cộng attackDone
    /// </summary>
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
        }

        isMoving = false;
        StartExitSequence();
    }

    /// <summary>
    /// Di chuyển wrecker đi vào khi kết thúc hành vi
    /// </summary>
    private void StartExitSequence()
    {
        Sequence exitSequence = DOTween.Sequence();

        exitSequence.Append(wrecker.transform.DOMove(transform.position, 1f)
                                             .SetEase(Ease.InBack));

        exitSequence.AppendInterval(1f);

        exitSequence.AppendCallback(() =>
        {
            gameObject.SetActive(false);
        });
    }

    /// <summary>
    /// Sau mỗi 0.1s, nó sẽ gọi DetectAndAvoidObstacle để kiểm tra và né tránh vật cản
    /// </summary>
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

    /// <summary>
    /// Nếu ở gần wrecker có vật cản thì bỏ qua việc tấn công mà đi né vật cản <br/>
    /// Nếu không có vật cản thì random 1 player và tính toán hướng bắn rồi bắn về phía player
    /// </summary>
    private IEnumerator AttackPhase()
    {
        if (player == null || wrecker == null) yield break;
        if (isObstacleNearWrecker) yield break; // Không attack nếu có obstacle gần player
        bulletWreckerGameobject.SetActive(false);

        int playerIndex = Random.Range(0, player.Count);
        Vector3 shootDirection = (player[playerIndex].transform.position - wrecker.transform.position).normalized;

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
            isObstacleNearWrecker = true;
        }
        else
        {
            isObstacleNearWrecker = false;
        }

    }

    /// <summary>
    /// Bắn tia raycast từ wrecker đến phía trước xem có vật cản không <br/>
    /// Nếu có thì gọi hàm FindSafeFlight để tìm vị trí an toàn và di chuyển đến vị trí an toàn
    /// </summary>
    private void DetectAndAvoidObstacle()
    {
        if (!isMoving) return; // Không né obstacle khi đang attack

        RaycastHit2D hit = Physics2D.Raycast(wrecker.transform.position, Vector2.right, raycastWreckerDistance);

        if (hit.collider != null && hit.collider.gameObject.CompareTag("Obstacle"))
        {
            Transform safeFlightPoint = FindSafeFlight();
            if (safeFlightPoint != null &&
                Vector3.Distance(wrecker.transform.position, safeFlightPoint.position) > 0.5f) // Nếu wrecker ở xa vị trị an toàn thì di chuyển lại safepos
            {
                safePosition = safeFlightPoint.position;
                wrecker.transform.DOMove(safePosition, 0.8f)
                    .SetEase(Ease.OutQuad);
            }
        }
    }

    /// <summary>
    /// Tìm vị trí an toàn trong danh sách flightPoints rồi trả về vị trí đó 
    /// </summary>
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

    /// <summary>
    /// Dừng khi player chết
    /// </summary>
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
}